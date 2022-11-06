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
//[Authorize]
    public class PaginationController : ControllerBase
    {

        private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public PaginationController(IAppRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public ActionResult<int> Get()
        {
            try
            {
                var count = _repository.GetCountProducts();
                return count;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{page}")]
        [Authorize]
        public async Task<ActionResult<ProductModel[]>> Get(int page)
        {
            try
            {
                
                // var products = await _repository.GetProductsByPage(page);
                var products = await _repository.GetAllProductsAsync();
                if (products == null) return NotFound();

                List<Product> productsBypage = new List<Product>();
                for (int i = 0; i < products.Length; i++)
                {
                    if ((i >= page*10) && (i < page * 10 + 10))
                    {
                        productsBypage.Add(products[i]);
                    }
                }
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
