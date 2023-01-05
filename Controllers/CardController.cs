using AutoMapper;
using ECommerce.Data.Entities;
using ECommerce.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using ECommerce.Model;
using System.Threading.Tasks;
using System;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly UserManager<UsersEcommerce> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        //private readonly IAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly IAppRepository _repository;

        public CardController(UserManager<UsersEcommerce> userManager,
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

        [HttpGet("userEmail")]
        public async Task<ActionResult<CardModel[]>> Get(string userEmail)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    return NotFound("Invalid user to return Cards");
                }
                var results = await _repository.GetCardsByUserAsync(userEmail);
                if (results == null)
                {
                    return BadRequest($"There's not Card for this user: {userEmail}");
                }

                var temp = _mapper.Map<CardModel[]>(results);
                return temp;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CardModel>> Get(int id)
        {
            try
            {
                var result = await _repository.GetCardByIdAsync(id);
                if (result == null)
                {
                    return NotFound($"There is not Card for CardId: {id}");
                }

                return _mapper.Map<CardModel>(result);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        

        [HttpPost]
        [Route("AddCard")]
        public async Task<IActionResult> Card([FromBody] CardModel model)
        {
            try
            {
                var result = await _repository.GetCardByCardNumberAsync(model.Email, model.CardNumber);
                if (result != null)
                {
                    return BadRequest("This Card  already exist");
                }
                var card = _mapper.Map<Card>(model);
                _repository.Add(card);
                if (await _repository.SaveChangesAsync())

                {                    
                    Console.WriteLine("creating new Card");

                }

                var location = _linkGenerator.GetPathByAction("Get", "Card",
                                                            new { id = card.CardId });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current CardId");
                }
                return Created(location, _mapper.Map<CardModel>(card));
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut("{id}")]
        // [Authorize]
        public async Task<ActionResult<CardModel>> Put(int id, CardModel model)
        {
            try
            {
                var oldCard = await _repository.GetCardByIdAsync(id);
                if (oldCard == null) return NotFound($"Could not find card with id of {id}");

                _mapper.Map(model, oldCard);
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<CardModel>(oldCard);
                }

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        // [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var oldCard = await _repository.GetCardByIdAsync(id);
                if (oldCard == null) return NotFound($"Could not find card with id of {id}");

                _repository.Delete(oldCard);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest("Failed to delete the card");
        }
    }
}
