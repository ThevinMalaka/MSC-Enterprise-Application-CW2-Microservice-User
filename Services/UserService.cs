using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using userService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using userService.DTO;

namespace userService.Services
{
    public class UserService
    {
        private ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel> CreateAsync(UserRegistrationDTO user)
        {

            // Check if user already exists
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                throw new ArgumentException("User already exists");
            }

            var newUser = new UserModel
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Height = user.Height,
                Weight = user.Weight,
                DateOfBirth = user.DateOfBirth
            };

            // Add user to the database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public UserModel Login(UserLoginModel user)
        {
            UserModel foundUser = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
            if (foundUser == null)
            {
                return null;
            }

            return foundUser;
        }

        public List<UserModel> GetAll()
        {
            return _context.Users.ToList();
        }

        public string GenerateJwtToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsMySuperSecretKeyForFitnessAppInMyMSCourseWork"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "thevinmalaka.com",
                audience: "thevinmalaka.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
