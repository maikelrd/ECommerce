﻿using AutoMapper;
using ECommerce.Data;
using ECommerce.Data.Entities;

using ECommerce.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<UsersEcommerce> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        //private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly IAppRepository _repository;

      //  UserDTO userDTO = new UserDTO();

        public UsersController(UserManager<UsersEcommerce> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, IMapper mapper, LinkGenerator linkGenerator, IAppRepository repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            //_repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _repository = repository;
        }

        [HttpGet("Email")]
        public async Task<IActionResult> Get(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return BadRequest("User not exist");
            }
            Address address = new ()
            {
                Street = user.Street,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode
            };
            address.Email = Email;
            if (address.Street == null)
            {
                address.Street = "";
            }
            if (address.City == null)
            {
                address.City = "";
            }
            if (address.State == null)
            {
                address.State = "";
            }
            if (address.ZipCode == null)
            {
                address.ZipCode = "";
            }
            return Ok(address);
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            UsersEcommerce user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.FirstName,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            UsersEcommerce user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.FirstName,
                FirstName=model.FirstName,
                LastName=model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("Address")]
        public async Task<IActionResult> Address([FromBody] Address model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("User not exist");
            }

            user.Street = model.Street;
            user.City = model.City;
            user.State = model.State;
            user.ZipCode = model.ZipCode;
            await _userManager.UpdateAsync(user);
            var user1 = await _userManager.FindByEmailAsync(model.Email);
            model.Email = user1.Email;
            model.Street = user1.Street;
            model.State = user1.State;
            model.ZipCode = user1.ZipCode;
            return Ok(model);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);
                // UserDTO userDTO = new UserDTO(user.UserName, user.Email);
                UserDTO userDTO = new UserDTO();
                userDTO.UserName = user.UserName;
                userDTO.Email = user.Email;
                userDTO.IsAuthenticated = true;
                userDTO.Token = new JwtSecurityTokenHandler().WriteToken(token);

                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken.Token;
                user.RefreshTokenCreated = refreshToken.Created;
                user.RefreshTokenExpires = refreshToken.Expires;
                await _userManager.UpdateAsync(user);
                // SetRefreshToken(refreshToken);
                userDTO.RefreshToken = refreshToken.Token;
                  return Ok(userDTO);
                //return Ok(new
                //{
                //    UserName = user.UserName,
                //    Email = user.Email,
                //    Token = new JwtSecurityTokenHandler().WriteToken(token),
                //    IsAuthenticated = true
                //    //expiration = token.ValidTo
                //    //token = new JwtSecurityTokenHandler().WriteToken(token),
                //    //expiration = token.ValidTo
                //});
            }
            return Unauthorized();
        }

        private async Task<TokenResponse> Authenticate(string username, Claim[] claims)
        {
            TokenResponse tokenResponse = new TokenResponse();
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            tokenResponse.Token = new JwtSecurityTokenHandler().WriteToken(token);

            // tokenResponse.RefreshToken = GenerateRefreshToken().Token;
            var response = GenerateRefreshToken();
            tokenResponse.RefreshToken =response.Token;
            var user = await _userManager.FindByNameAsync(username);
            user.RefreshToken = response.Token;
            user.RefreshTokenCreated = response.Created;
            user.RefreshTokenExpires = response.Expires;
            await _userManager.UpdateAsync(user);

            return tokenResponse;
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenResponse token)
        {
            try
            {
                //if (token.Token == "")
                //{
                //    return Unauthorized();
                //}
                var tokenhandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenhandler.ValidateToken(token.Token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
                }, out securityToken);
                var _token = securityToken as JwtSecurityToken;
                if (_token == null || !_token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                {
                    return Unauthorized();
                }
                var username = principal.Identity.Name;
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return Unauthorized();
                }
                if (user.RefreshToken != token.RefreshToken)
                {
                    return Unauthorized();
                }
                TokenResponse _result = await Authenticate(username, principal.Claims.ToArray());
                return Ok(_result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, " Invalid JWT");
            }
        }
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddMinutes(5),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        //private RefreshToken GenerateRefreshToken()
        //{
        //    var refreshToken = new RefreshToken
        //    {
        //        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        Created = DateTime.UtcNow
        //    };
        //    return refreshToken;
        //}

        //private void SetRefreshToken(RefreshToken newRefreshToken)
        //{
        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Expires = newRefreshToken.Expires
        //    };
        //    Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        //   // userDTO.RefreshToken = newRefreshToken.Token;
        //   // userDTO.RefreshTokenCreated = newRefreshToken.Created;
        //    //userDTO.RefreshTokenExpires = newRefreshToken.Expires;
        //}
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

       
        
    }
}
