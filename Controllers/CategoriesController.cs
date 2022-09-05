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
    public class CategoriesController : ControllerBase
    {
        private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public CategoriesController(IAppRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<CategoryModel[]>> Get(bool includeProducts = true)
        {
            try
            {
                var results = await _repository.GetAllCategoriesAsync(includeProducts);
                return _mapper.Map<CategoryModel[]>(results);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> Get(int id, bool includeProducts = true)
        {
            try
            {
                var result = await _repository.GetCategoryById(id, includeProducts);
                if (result == null) return NotFound();
                return _mapper.Map<CategoryModel>(result);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoryModel>> Post(CategoryModel model)
        {
            try
            {
                var existing = await _repository.GetCategoryByName(model.CategoryName);
                if (existing != null)
                {
                    return BadRequest("Name is in use");
                }

                var category = _mapper.Map<Category>(model);
                _repository.Add(category);
                if (await _repository.SaveChangesAsync())
                {
                    var location = _linkGenerator.GetPathByAction("Get", "Categories",
                                                                  new { id = category.CategoryId });
                    if (string.IsNullOrWhiteSpace(location))
                    {
                        return BadRequest("Could not use current CategoryId");
                    }
                    return Created(location, _mapper.Map<CategoryModel>(category));
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryModel>> Put(int id, CategoryModel model)
        {
            try
            {
                var oldCategory = await _repository.GetCategoryById(id);
                if(oldCategory == null) return NotFound($"Could not find Category with Id  {id}");

                _mapper.Map(model, oldCategory);
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<CategoryModel>(oldCategory);
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var oldCategory = await _repository.GetCategoryById(id);
                if(oldCategory == null) return NotFound($"Could not find Category with Id  {id}");

                _repository.Delete(oldCategory);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok("Delete Category successed");
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest("Failed to delete the Category");
        }
    }
}
