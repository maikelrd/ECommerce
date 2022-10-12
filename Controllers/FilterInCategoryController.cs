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
    public class FilterInCategoryController : ControllerBase
    {
        private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public FilterInCategoryController(IAppRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{categoryId}/{filterText}")]
        public ActionResult<int> Get(int categoryId, string filterText)
        {
            try
            {
                filterText.ToLower();
                var count = _repository.GetCountProductsByCategoryFilter(categoryId,filterText);
                return count;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        [HttpGet("{categoryId}/{page}/{filterText}")]
        public async Task<ActionResult<ProductModel[]>> Get(int categoryId,int page, string filterText)
        {
            try
            {
                //FilterModel filterModel = new FilterModel();
                ProductModel productModel = new ProductModel();


                var filter = filterText.ToLower();
                var results = await _repository.GetProductsByCategoryFilter(categoryId, filter);
                if (results == null) return NotFound(); ;
                //filterModel.Products = _mapper.Map<ProductModel[]>(results);

                List<Product> productsBypage = new List<Product>();
                for (int i = 0; i < results.Length; i++)
                {
                    if ((i >= page * 10) && (i < page * 10 + 10))
                    {
                        productsBypage.Add(results[i]);
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


        //[HttpGet("{categoryId}/{filterText}")]
        //public async Task<ActionResult<FilterModel>> Get(int categoryId, string filterText)
        //{
        //    try
        //    {
        //        FilterModel filterModel = new FilterModel();
        //        ProductModel productModel = new ProductModel();

        //        var filter = filterText.ToLower();
        //        var results = await _repository.GetProductsByCategoryFilter(categoryId, filter);
        //        if (results == null) return NotFound();
        //        filterModel.count = results.Count();

        //        filterModel.Products = _mapper.Map<ProductModel[]>(results);

        //        return filterModel;

        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine(ex);
        //        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        //    }
        //}
    }
}
