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
    public class UsersController : ControllerBase
    {
        private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public UsersController(IAppRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<UserModel[]>> Get()
        {
            try
            {
                var results = await _repository.GetAllUserAsync();
                return _mapper.Map<UserModel[]>(results);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserModel>> Get(int id)
        {
            try
            {
                var result = await _repository.GetUserByIdAsync(id);
                if (result == null)
                {
                    return NotFound($"Could not find User with id {id}");
                }
                return _mapper.Map<UserModel>(result);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }



        [HttpPost]
        public async Task<ActionResult<UserModel>> Post(UserModel model)
        {
            try
            {
                var existing = await _repository.GetUserByNameAsync(model.UserName);
                if (existing != null)
                {
                    return BadRequest("UserName in use");
                }

                var user = _mapper.Map<User>(model);
                _repository.Add(user);

                if (await _repository.SaveChangesAsync())
                {
                    var location = _linkGenerator.GetPathByAction("Get", "Users",
                                                        new { id = user.UserId });
                    if (string.IsNullOrWhiteSpace(location))
                    {
                        return BadRequest("Could not use current UserId");
                    }
                    return Created(location, _mapper.Map<UserModel>(user));
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserModel>> Put(int id, UserModel model)
        {
            try
            {
                var oldUser = await _repository.GetUserByIdAsync(id);
                if (oldUser == null)
                {
                    return NotFound($"Could not find user with id {id}");
                }

                _mapper.Map(model, oldUser);
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<UserModel>(oldUser);
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }
    }
}
