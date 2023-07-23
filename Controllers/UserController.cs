using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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

        public UserController(UserService userService, UserWeightService userWeightService)
        {
            _userService = userService;
            _userWeightService = userWeightService;
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
    }
}













//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using userService.Entities;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace userService.Controllers
//{
//    [Route("[controller]")]
//    public class UserController : Controller
//    {
//        private ApplicationDbContext _context;

//        public UserController(ApplicationDbContext context)
//        {
//            _context = context;
//        }


//        // Action to create a new user
//        [HttpPost("create")]
//        public async Task<IActionResult> CreateAsync([FromBody] UserModel user)
//        // public async Task<ActionResult<UserModel>> Post(UserModel user)
//        {
//            // Check if the user already exists
//            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
//            if (existingUser != null)
//            {
//                return BadRequest(new { message = "User already exists" });
//            }

//            // Create the user
//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            // Return the user
//            return Ok(user); 

//        }

//        [HttpPost("login")]
//        public IActionResult Login([FromBody] UserLoginModel user)
//        {
//            // Find the user with the given email and password
//            UserModel foundUser = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
//            if (foundUser == null)
//            {
//                return NotFound("User not found");
//            }

//            // Generate the JWT token
//            string token = GenerateJwtToken(foundUser);

//            // Return the user and the token
//            return Ok(new { user = foundUser, token = token });
//        }

//        // Action to get all users
//        [HttpGet("all")]
//        public IActionResult GetAll()
//        {
//            List<UserModel> users = _context.Users.ToList();
//            return Ok(users);  // Return all users as JSON
//        }



//        // JWT Authentication
//        private string GenerateJwtToken(UserModel user)
//        {
//            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsMySuperSecretKeyForFitnessAppInMyMSCourseWork"));
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//            var claims = new List<Claim>
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
//                new Claim("id", user.Id.ToString()),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
//            };

//            var token = new JwtSecurityToken(
//                issuer: "thevinmalaka.com",
//                audience: "thevinmalaka.com",
//                claims: claims,
//                expires: DateTime.Now.AddMinutes(30),
//                signingCredentials: credentials);

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}

