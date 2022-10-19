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
    public class Shopping_CartController : ControllerBase
    {
        private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public Shopping_CartController(IAppRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<Shopping_CartModel[]>> Get()
        {
            try
            {
                var results = await _repository.GetAllShoppingCartsAsync();
                return _mapper.Map<Shopping_CartModel[]>(results);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("id")]
        public async Task<ActionResult<Shopping_CartModel>> Get(int  id)
        {
            try
            {
                var result = await _repository.GetShoppingCartByIdAsync(id);
                if (result == null)
                {
                    return NotFound("Invalid id to return shopping cart");
                }
                
                return _mapper.Map<Shopping_CartModel>(result);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("userEmail")]
        public async Task<ActionResult<Shopping_CartModel[]>> Get(string userEmail)
        {
            try
            {
                var user = await _repository.GetUserByEmailAsync(userEmail);
                if (user == null)
                {
                    return NotFound("Invalid user to return shopping cart");
                }
                var results =  _repository.GetShoppingCartByUserAsync(user.UserId);
                if (results == null)
                {
                    return BadRequest($"There's not shopping Cart for this user: {userEmail}");
                }
                return _mapper.Map<Shopping_CartModel[]>(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Shopping_CartModel>> Post(Shopping_CartModel shopping_CartModel)
        {
            try
            {
                var user = await _repository.GetUserByEmailAsync(shopping_CartModel.UserEmail);
                if (user == null)
                {
                    return BadRequest($"Incorrect user email: {shopping_CartModel.UserEmail}");
                }

                var shoppingCart = _repository.GetShoppingCartByUserAsync(user.UserId);
                if (shoppingCart == null)
                {
                    
                      shoppingCart = new ShoppingCart
                    {
                        ShoppingCartId = 0,                       
                        UserId = user.UserId                   
                    };
                    _repository.Add(shoppingCart);
                    await _repository.SaveChangesAsync();                    
                }

                for (int i = 0; i < shopping_CartModel.ProductsInShoppingCart.Count(); i++)
                {
                    
                    var productShoppingCart = await _repository.GetProductShoppingCartAsyn(shoppingCart.ShoppingCartId, shopping_CartModel.ProductsInShoppingCart.ElementAt(i).ProductShoppingCart.ProductId);

                    if (productShoppingCart == null)
                    {
                        productShoppingCart = new ProductShoppingCart
                        {
                            ProductShoppingCartId = 0,
                            ShoppingCartId = shoppingCart.ShoppingCartId,
                            ProductId = shopping_CartModel.ProductsInShoppingCart.ElementAt(i).ProductShoppingCart.ProductId,
                            Quantity = shopping_CartModel.ProductsInShoppingCart.ElementAt(i).Quantity
                        };

                        _repository.Add(productShoppingCart);
                        if (await _repository.SaveChangesAsync())
                        {


                        }
                    }
                    else
                    {
                        productShoppingCart.Quantity++;
                        if (await _repository.SaveChangesAsync())
                        {

                        }

                    }
                }
                    Shopping_Cart shopping_Cart = new Shopping_Cart();                  
                    // shoppingCart = _repository.GetShoppingCartByUserAsync(user.UserId);
                    shopping_Cart.ShoppingCartId = shoppingCart.ShoppingCartId;
                    shopping_Cart.UserEmail = shoppingCart.User.Email;
                    var productsShoppingCart = await _repository.GetProductShoppingCartByShoppingCartIdAsyn(shoppingCart.ShoppingCartId);
                    
                    List<ProductInShoppingCart> products = new List<ProductInShoppingCart>();

                for (int i = 0; i < productsShoppingCart.Length; i++)
                {
                    ProductInShoppingCart productInShoppingCart = new ProductInShoppingCart();
                    productInShoppingCart.ProductShoppingCart = productsShoppingCart[i].Product;
                    productInShoppingCart.Quantity = productsShoppingCart[i].Quantity;
                    var prueba = productInShoppingCart;
                    products.Add(prueba);                  

                }
                shopping_Cart.ProductsInShoppingCart = products;
                

                var location = _linkGenerator.GetPathByAction("Get", "Shopping_Cart",
                                                       new { email = shopping_CartModel.UserEmail });

                    return Created(location, _mapper.Map<Shopping_CartModel>(shopping_Cart));
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            //return BadRequest();
        }


        [HttpDelete("userEmail")]
        public async Task<ActionResult<ProductShoppingCartModel>> Delete(string userEmail)
        {
            try
            {

                var user = await _repository.GetUserByEmailAsync(userEmail);
                if (user == null)
                {
                    return NotFound($"Invalid user to  delete ShoppingCart para el userEmail: {userEmail}");
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

                _repository.Delete(shoppingCart);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok("Deleting ShoppingCart successed");
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
