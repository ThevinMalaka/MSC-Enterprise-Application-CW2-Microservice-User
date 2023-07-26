using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        private const string LogFilePath = "Logs/UserLoginLogs.xml";
        private const string FailedLoginFilePath = "Logs/FailedLoginLogs.xml";

        private XmlSerializer _loginLogSerializer = new XmlSerializer(typeof(List<UserLoginLog>));
        private XmlSerializer _failedLogSerializer = new XmlSerializer(typeof(List<UserLoginFailedLog>));

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
                LogFailedLogin(user.Email); // Log the failed login attempt
                return NotFound("User not found");
            }

            string token = _userService.GenerateJwtToken(foundUser);

            // Log the login event - successful login attempt
            LogUserLogin(foundUser.Email);

            return Ok(new { user = foundUser, token = token });
        }

        // Action to get all users
        [HttpGet("all")]
        [Authorize]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        // Log Successful Login Logs ----------------
        private void LogUserLogin(string email)
        {
            var logs = GetLoginLogs();

            // Getting the IP address of the client
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            logs.Add(new UserLoginLog { Email = email, LoginTime = DateTime.UtcNow, IPAddress = ipAddress });

            SaveLoginLogs(logs);
        }

        private List<UserLoginLog> GetLoginLogs()
        {
            EnsureDirectoryExists(LogFilePath);

            if (!System.IO.File.Exists(LogFilePath))
            {
                return new List<UserLoginLog>();
            }
            using var reader = new StreamReader(LogFilePath);
            return (List<UserLoginLog>)_loginLogSerializer.Deserialize(reader);
        }

        private void SaveLoginLogs(List<UserLoginLog> logs)
        {
            EnsureDirectoryExists(LogFilePath);

            using var writer = new StreamWriter(LogFilePath);
            _loginLogSerializer.Serialize(writer, logs);
        }

        // Log Successful Login Logs ----------------
        // Log Failed Attempts Logs -------------------
        private void LogFailedLogin(string email)
        {
            var logs = GetFailedLoginLogs();

            // Getting the IP address of the client
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            logs.Add(new UserLoginFailedLog { AttemptedEmail = email, AttemptTime = DateTime.UtcNow, IPAddress = ipAddress });

            SaveFailedLoginLogs(logs);
        }

        private List<UserLoginFailedLog> GetFailedLoginLogs()
        {
            EnsureDirectoryExists(FailedLoginFilePath);

            if (!System.IO.File.Exists(FailedLoginFilePath))
            {
                return new List<UserLoginFailedLog>();
            }
            using var reader = new StreamReader(FailedLoginFilePath);
            return (List<UserLoginFailedLog>)_failedLogSerializer.Deserialize(reader);
        }

        private void SaveFailedLoginLogs(List<UserLoginFailedLog> logs)
        {
            EnsureDirectoryExists(FailedLoginFilePath);

            using var writer = new StreamWriter(FailedLoginFilePath);
            _failedLogSerializer.Serialize(writer, logs);
        }

        // Log Failed Attempts Logs -------------------

        // Endpoint to retrieve the login logs
        [HttpGet("loginLogs")]
        [Authorize]
        public IActionResult GetUserLoginLogs()
        {
            var logs = GetLoginLogs();

            var xmlSerializer = new XmlSerializer(logs.GetType());

            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, logs);

            return new ContentResult
            {
                Content = stringWriter.ToString(),
                ContentType = "application/xml",
                StatusCode = 200
            };
        }

        [HttpGet("failedLoginLogs")]
        [Authorize]
        public IActionResult GetUserFailedLoginLogs()
        {
            var logs = GetFailedLoginLogs();

            var xmlSerializer = new XmlSerializer(logs.GetType());

            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, logs);

            return new ContentResult
            {
                Content = stringWriter.ToString(),
                ContentType = "application/xml",
                StatusCode = 200
            };
        }

        private void EnsureDirectoryExists(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }
        }


    }
}
