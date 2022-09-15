using AutoMapper;
using ECommerce.Data;
using ECommerce.Data.Entities;
using ECommerce.Model;
using Microsoft.AspNetCore.Authorization;
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
    public class ProductsController : ControllerBase
    {
        private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public ProductsController(IAppRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]       
        public async Task<ActionResult<ProductModel[]>> Get()
        {
            try
            {
                var results = await _repository.GetAllProductsAsync();
                return _mapper.Map<ProductModel[]>(results);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> Get(int id)
        {
            try
            {
                var product = await _repository.GetProductById(id);
                if (product == null) return NotFound();
                return _mapper.Map<ProductModel>(product);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
       // [Authorize]
        public async Task<ActionResult<ProductModel>> Post(ProductModel model)
        {
            try
            {
                var existing = await _repository.GetProductByName(model.ProductName);
                if (existing !=null)
                {
                    return BadRequest("Name is in use");
                }

                
                //Create new product
                var product = _mapper.Map<Product>(model);
                _repository.Add(product);

                if (await _repository.SaveChangesAsync())

                {
                    var location = _linkGenerator.GetPathByAction("Get", "Products",
                                                               new { id = product.ProductId });
                    if (string.IsNullOrWhiteSpace(location))
                    {
                        return BadRequest("Could not use current ProductId");
                    }
                    return Created(location, _mapper.Map<ProductModel>(product));
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
       // [Authorize]
        public async Task<ActionResult<ProductModel>> Put(int id, ProductModel model)
        {
            try
            {
                var oldProduct = await _repository.GetProductById(id);
                if(oldProduct == null) return NotFound($"Could not find product with id of {id}");
               
                _mapper.Map(model, oldProduct);
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<ProductModel>(oldProduct);
                }

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var oldProduct = await _repository.GetProductById(id);
                if (oldProduct == null) return NotFound($"Could not find product with id of {id}");

                _repository.Delete(oldProduct);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest("Failed to delete the product");
        }

    }
}
