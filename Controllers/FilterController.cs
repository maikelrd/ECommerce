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
    public class FilterController : ControllerBase
    {
        private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public FilterController(IAppRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{filterText}")]
        public async Task<ActionResult<FilterModel>> Get( string filterText)
        {
            try
            {
                FilterModel filterModel = new FilterModel();
                ProductModel productModel = new ProductModel();

                var filter = filterText.ToLower();
                var results = await _repository.GetProductsFilter( filter);
                filterModel.count = results.Count();

                filterModel.Products = _mapper.Map<ProductModel[]>(results);

                return filterModel;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{categoryId}/{filterText}")]
        public async Task<ActionResult<FilterModel>> Get(int categoryId, string filterText)
        {
            try
            {
                FilterModel filterModel = new FilterModel();
                ProductModel productModel = new ProductModel();
               
                var filter = filterText.ToLower();
                var results = await _repository.GetProductsByCategoryFilter(categoryId, filter);
                filterModel.count = results.Count();

                filterModel.Products = _mapper.Map<ProductModel[]>(results);
               
                return filterModel;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
