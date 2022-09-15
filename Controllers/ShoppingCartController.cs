using AutoMapper;
using ECommerce.Data;
using ECommerce.Data.Entities;
using ECommerce.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public ShoppingCartController(IAppRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCartModel[]>> Get()
        {
            try
            {
                var results = await _repository.GetAllCartsAsync();
                return _mapper.Map<ShoppingCartModel[]>(results);

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
      
        [HttpGet("email")]
        public async Task<ActionResult<ShoppingCartModel[]>> Get(string email)
        {
            try
            {
                var user = await _repository.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound("Invalid user to return shopping cart");
                }
                var results = await _repository.GetCartsByUserAsync(user.UserId);
                return _mapper.Map<ShoppingCartModel[]>(results);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCartModel>> Post(ShoppingCartModel shoppingCartModel)
        {
            try
            {
                var user = await _repository.GetUserByEmailAsync(shoppingCartModel.UserEmail); 
               // var product = _mapper.Map<Product>(shoppingCartModel.Product);
                var shoppingCartItem =  _repository.GetCartByNotProduct(shoppingCartModel.Product);
                if (shoppingCartItem == null)
                {
                    //var addShoppingCart = _mapper.Map<ShoppingCartItem>(shoppingCartModel);
                     shoppingCartItem = new ShoppingCartItem
                    {
                        ShoppingCartItemId = 0,
                        ProductId = shoppingCartModel.ProductId,
                        UserId = user.UserId,
                        Amount = shoppingCartModel.Amount
                    };
                    _repository.Add(shoppingCartItem);
                    if (await _repository.SaveChangesAsync())
                    {
                        var location = _linkGenerator.GetPathByAction("Get", "ShoppingCart",
                                                           new  { email = shoppingCartModel.UserEmail });
                       
                        return Created(location, _mapper.Map<ShoppingCartModel>(shoppingCartItem));
                    }
                }
                else
                {
                    shoppingCartItem.Amount++;
                    if (await _repository.SaveChangesAsync())
                    {
                        return Ok("Update amount");
                    }
                }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

    }
}
