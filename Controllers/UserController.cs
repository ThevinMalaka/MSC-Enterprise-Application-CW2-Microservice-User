using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using userService.DTO;
using userService.Entities;
using userService.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace userService.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly UserWeightService _userWeightService;
        private readonly ApplicationDbContext _context;

        public UserController(UserService userService, UserWeightService userWeightService, ApplicationDbContext context)
        {
            _userService = userService;
            _userWeightService = userWeightService;
            _context = context;
        }

        // Action to create a new user
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] UserRegistrationDTO user)
        {
            var createdUser = await _userService.CreateAsync(user);

            //create a new weight log
            var weightLogResult = await _userWeightService.CreateWeightLogAsync(
                new WeightLogCreationDTO
                {
                    UserId = createdUser.Id,
                    Weight = createdUser.Weight,
                    Date = DateTime.Now
                }
            );

            return Ok(weightLogResult);
        }

        // Action to login
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel user)
        {
            UserModel foundUser = _userService.Login(user);
            if (foundUser == null)
            {
                return NotFound("User not found");
            }

            string token = _userService.GenerateJwtToken(foundUser);

            return Ok(new { user = foundUser, token = token });
        }

        // Action to get all users
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }


        // Get all users data from XML formet
        [HttpGet("XML")]
        [Authorize]
        public string GetAllUserWeightsXML()
        {
            var users = _context.Users.ToList();
            var usersDto = users.Select(user => MapToDTO(user)).ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<UserModelDTO>));

            using StringWriter writer = new StringWriter();
            serializer.Serialize(writer, usersDto);

            return writer.ToString();
        }

        private UserModelDTO MapToDTO(UserModel user)
        {
            return new UserModelDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Weight = user.Weight,
                Height = user.Height,
                DateOfBirth = user.DateOfBirth
            };
        }
    }
}
