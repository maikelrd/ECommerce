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
    public class DepartmentsController : ControllerBase
    {
        private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public DepartmentsController(IAppRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<DepartmentModel[]>> Get(bool includeCategories)
        {
            try
            {
                var results = await _repository.GetAllDepartmentsAsync(includeCategories);
                return _mapper.Map<DepartmentModel[]>(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentModel>> Get(int id, bool includeCategories)
        {
            try
            {
                var result = await _repository.GetDepartmentById(id, includeCategories);
                if (result == null) return NotFound($"Could not find department with id {id}");

                return _mapper.Map<DepartmentModel>(result);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DepartmentModel>> Post(DepartmentModel model)
        {
            try
            {
                var existing = await _repository.GetDepartmentByName(model.DepartmentName);
                if (existing != null)
                {
                    return BadRequest("Name is in use");
                }

                var department = _mapper.Map<Department>(model);
                _repository.Add(department);

                if (await _repository.SaveChangesAsync())
                {
                    var location = _linkGenerator.GetPathByAction("Get", "Departments",
                                                                 new { id = department.DepartmentId });
                    return Created(location, _mapper.Map<DepartmentModel>(department));
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentModel>> Put(int id, DepartmentModel model)
        {
            try
            {
                var oldDepartment = await _repository.GetDepartmentById(id);
                if (oldDepartment == null)
                {
                    return NotFound($"Could not find Department with id of {id}");
                }

                _mapper.Map(model, oldDepartment);
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<DepartmentModel>(oldDepartment);
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DepartmentModel>> Delete(int id)
        {
            try
            {
                var oldDepartment = await _repository.GetDepartmentById(id);
                if (oldDepartment == null)
                {
                    return NotFound($"Could not find Department with id of {id}");
                }

                _repository.Delete(oldDepartment);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest("Failed to delete the department");
        }
    }
}
