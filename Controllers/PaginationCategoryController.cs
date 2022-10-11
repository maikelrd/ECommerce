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
    public class PaginationCategoryController : ControllerBase
    {

        private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public PaginationCategoryController(IAppRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<int>> Get(int id)
        {
            try
            {
                // var count = _repository.GetCountProductsByCategory(id);
                var products = await _repository.GetAllProductsByCategoryAsync(id);
                var count = products.Count();
                return count;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id}/{page}")]
        public async Task<ActionResult<ProductModel[]>> Get(int id, int page)
        {
            try
            {

                var products = await _repository.GetAllProductsByCategoryAsync(id);
                if (products == null) return NotFound();

                List<Product> productsBypage = new List<Product>();
                for (int i = 0; i < products.Length; i++)
                {
                     if ((i >= page * 10) && (i < page * 10 + 10))                   
                    {
                        productsBypage.Add(products[i]);
                    }
                };
               
                return _mapper.Map<ProductModel[]>(productsBypage);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
