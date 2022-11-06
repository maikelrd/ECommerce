using AutoMapper;
using ECommerce.Data;
using ECommerce.Data.Entities;
using ECommerce.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<UsersEcommerce> _userManager;
        private readonly SignInManager<UsersEcommerce> _signInManager;

        //private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly IAppRepository _repository;

        public UsersController(IConfiguration configuration,UserManager<UsersEcommerce> userManager,SignInManager<UsersEcommerce> signInManager, IMapper mapper, LinkGenerator linkGenerator, IAppRepository repository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            //_repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _repository = repository;
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult<RegisterModel>> RegisterUser(UserModel userModel)
        {
            try
            {
                //UserName must be equal to Email, o there will be an error
                var user = new UsersEcommerce() { FirstName = userModel.FirstName, LastName = userModel.LastName,UserName= userModel.Email, Email = userModel.Email };
                var result = await _userManager.CreateAsync(user, userModel.Password);
                if (result.Succeeded)
                {
                    var appUser = await _userManager.FindByEmailAsync(userModel.Email);
                    //var appuser1 = await _repository.GetUserAspNetByIdAsync(appUser.Id);
                    var registerModel = new RegisterModel(appUser.FirstName, appUser.Email);
                    return await Task.FromResult(registerModel);
                }

                var error = string.Join(",", result.Errors.Select(x => x.Description).ToArray());
                return BadRequest(error);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
                
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.FindByEmailAsync(model.Email);
                        var user = new UserDTO(appUser.FirstName, appUser.Email);
                        user.IsAuthenticated = true;
                        user.Token = GenerateToken(appUser);
                        return await Task.FromResult(user);
                    }              

                }
                return BadRequest("Invalid Email or password");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        private string GenerateToken(UsersEcommerce user)
        {
            //var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //Create satandard JWT claims
            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.NameIdentifier, user.Id),
            //    new Claim(JwtRegisteredClaimNames. Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
            //    new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
            //};
            //Create the JwtSecurityToken object
            //var token = new JwtSecurityToken(
            //    new JwtHeader(
            //        new SigningCredentials(key, SecurityAlgorithms.HmacSha256)),
            //    new JwtPayload(claims));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                 expires: DateTime.Now.AddMinutes(60),
                 signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            //Create a string representation of the Jwt token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //private string GenerateToken(UsersEcommerce user)
        //{
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new System.Security.Claims.ClaimsIdentity(new[]
        //        {
        //            new System.Security.Claims.Claim(JwtRegisteredClaimNames.NameId,user.Id),
        //            new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email,user.Email),
        //            new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
        //        }),
        //        Expires = DateTime.UtcNow.AddHours(12),
        //        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
        //        Audience = _configuration["Jwt:Audience"],
        //        Issuer = _configuration["Jwt:Issuer"]
        //    };
        //    var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        //    return jwtTokenHandler.WriteToken(token);

        //}

        //    [HttpGet]
        //    public async Task<ActionResult<UserModel[]>> Get()
        //    {
        //        try
        //        {
        //            var results = await _repository.GetAllUserAsync();
        //            return _mapper.Map<UserModel[]>(results);
        //        }
        //        catch (Exception)
        //        {

        //            return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        //        }
        //    }

        //    [HttpGet("{id:int}")]
        //    public async Task<ActionResult<UserModel>> Get(int id)
        //    {
        //        try
        //        {
        //            var result = await _repository.GetUserByIdAsync(id);
        //            if (result == null)
        //            {
        //                return NotFound($"Could not find User with id {id}");
        //            }
        //            return _mapper.Map<UserModel>(result);
        //        }
        //        catch (Exception)
        //        {

        //            return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        //        }
        //    }



        //    [HttpPost("RegisterUser")]
        //    public async Task<ActionResult<RegisterModel>> Post(UserModel model)
        //    {
        //        try
        //        {
        //            var existing = await _repository.GetUserByEmailAsync(model.Email);
        //            if (existing != null)
        //            {
        //                return BadRequest("Email in use");
        //            }

        //            var user = _mapper.Map<User>(model);
        //            _repository.Add(user);

        //            if (await _repository.SaveChangesAsync())
        //            {
        //                var location = _linkGenerator.GetPathByAction("Get", "Users",
        //                                                    new { id = user.UserId });
        //                if (string.IsNullOrWhiteSpace(location))
        //                {
        //                    return BadRequest("Could not use current UserId");
        //                }
        //                //Here i put registerModel, because if return a UserModel, then send back the password to the backend.
        //                return Created(location, _mapper.Map<RegisterModel>(user));
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex);
        //            return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        //        }
        //        return BadRequest();
        //    }

        //    [HttpPut("{id}")]
        //    public async Task<ActionResult<UserModel>> Put(int id, UserModel model)
        //    {
        //        try
        //        {
        //            var oldUser = await _repository.GetUserByIdAsync(id);
        //            if (oldUser == null)
        //            {
        //                return NotFound($"Could not find user with id {id}");
        //            }

        //            _mapper.Map(model, oldUser);
        //            if (await _repository.SaveChangesAsync())
        //            {
        //                return _mapper.Map<UserModel>(oldUser);
        //            }
        //        }
        //        catch (Exception)
        //        {

        //            return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        //        }
        //        return BadRequest();
        //    }
    }
}
