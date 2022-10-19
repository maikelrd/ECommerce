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
    public class ProductsShoppingCartController : ControllerBase
    { 

        private readonly IAppRepository _repository;
    private readonly IMapper _mapper;
    private readonly LinkGenerator _linkGenerator;
        public ProductsShoppingCartController(IAppRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task <ActionResult<ProductShoppingCartModel[]>> Get()
        {
            try
            {
                var results = await _repository.GetAllProductShoppingCartAsync();
                if (results == null)
                {
                    return NotFound("There is not ProductShoppingCart in the DB");
                }

                return _mapper.Map<ProductShoppingCartModel[]>(results);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductShoppingCartModel>> Get(int id)
        {
            try
            {
                var results = await _repository.GetProductShoppingCartByProductShoppingCartIdAsyn(id);
                if (results == null)
                {
                    return NotFound($"There is not ProductShoppingCart for ProductShoppinCartId: {id}");
                }

                return _mapper.Map<ProductShoppingCartModel>(results);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductShoppingCartModel>> Put(int id, int Quantity)
        {
            try
            {
                var result =await  _repository.GetProductShoppingCartByProductShoppingCartIdAsyn(id);
                if (result == null)
                {
                    return NotFound($"There is not ProductShoppingCart for ProductShoppinCartId: {id}");
                }
                result.Quantity = Quantity;
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<ProductShoppingCartModel>(result);
                }             
                
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
           
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductShoppingCartModel>> Delete(int id)
        {
            try
            {
                var result = await _repository.GetProductShoppingCartByProductShoppingCartIdAsyn(id);
                if (result == null)
                {
                    return NotFound($"There is not ProductShoppingCart for ProductShoppinCartId: {id}");
                }
                 _repository.Delete(result);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok("Delete ProductShoppingCartItem successed");
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();

        }

        [HttpDelete("userEmail")]
        public async Task<ActionResult<ProductShoppingCartModel>> Delete(string userEmail)
        {
            try
            {

                var user = await _repository.GetUserByEmailAsync(userEmail);
                if (user == null)
                {
                    return NotFound($"Invalid user to return delete ProductShoppingCart para el userEmail: {userEmail}");
                }

                var shoppingCart = _repository.GetShoppingCartByUserAsync(user.UserId);
                if (shoppingCart == null)
                {
                    return NotFound("Invalid user to return a ShoppingCart");
                }

                ////var productShopingCart = _repository.GetProductShoppingCartByShoppingCartIdAsyn(shoppingCart.ShoppingCartId);
                ////if (productShopingCart == null)
                ////{
                ////    return NotFound($"There are not product in the shoppingCart para el userEmail: {userEmail}");
                ////}

                _repository.ClearProductShoppingCartCart(shoppingCart.ShoppingCartId);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok("Deleting ProductShoppingCart successed");
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
