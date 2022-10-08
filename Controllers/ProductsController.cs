using AutoMapper;
using ECommerce.Data;
using ECommerce.Data.Entities;
using ECommerce.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        [Produces("application/json")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        // [Authorize]
        public async Task<ActionResult<ProductModel>> Post([FromForm] FileModel fileObj)
        {
            try
            {
                
                ProductModel productModel = JsonConvert.DeserializeObject<ProductModel>(fileObj.Product);
                var existing = await _repository.GetProductByName(productModel.ProductName);
                if (existing != null)
                {
                    return BadRequest("Name is in use");
                }

                var product = _mapper.Map<Product>(productModel);
                _repository.Add(product);
                if (await _repository.SaveChangesAsync())

                {
                    //productImages.ProductId = product.ProductId;
                    Console.WriteLine("creating new product");

                }



                ImageModel ImageModel = new ImageModel();

                for (int i = 0; i < fileObj.ImageFile.Length; i++)
                {
                    if (fileObj.ImageFile[i].Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            fileObj.ImageFile[i].CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            var temp = Convert.ToBase64String(fileBytes);
                            ImageModel.PicByte = temp;
                        }
                        ImageModel.Name = fileObj.ImageFile[i].FileName;
                        ImageModel.Type = fileObj.ImageFile[i].ContentType;

                        var image = _mapper.Map<Image>(ImageModel);
                        image.ProductId = product.ProductId;
                        _repository.Add(image);
                        if (await _repository.SaveChangesAsync())
                        {
                            Console.WriteLine("Saving Image in database");
                            // productModel.ImageId = image.Id;
                            // productModel.Image = null;
                        }

                       
                    }

                }
                var location = _linkGenerator.GetPathByAction("Get", "Products",
                                                              new { id = product.ProductId });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current ProductId");
                }
                return Created(location, _mapper.Map<ProductModel>(product));

            }
            catch (Exception ex )
            {
                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
           // return BadRequest();
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
       // [Authorize]
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
