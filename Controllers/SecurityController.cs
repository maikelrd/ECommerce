using AutoMapper;
using ECommerce.Data;
using ECommerce.Data.Entities;
using ECommerce.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IAppRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
      //  private readonly ILogger _logger;

        public SecurityController(IAppRepository repository,IConfiguration configuration, IMapper mapper)
        {
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
           // _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<Object> Login([FromBody] LoginModel model)
        {
            try
            {
                var appUser = await _repository.GetUserByEmailAsync(model.Email);
                if (appUser != null)
                {
                    if (model.Password == appUser.Password)
                    {
                        var user = new UserDTO(appUser.FirstName, appUser.Email);
                        user.IsAuthenticated = true;
                        
                        user.Token = GenerateToken(user);
                        return await Task.FromResult(user);
                    }
                    else
                    {
                        return BadRequest("Invalid password");
                    }
                    
                }
                else
                {
                    return NotFound("Invalid email");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

       
        private string GenerateToken(UserDTO user)
        {
            //var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //Create satandard JWT claims
            //Create the JwtSecurityToken pbject
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            //Create a string representation of the Jwt token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
