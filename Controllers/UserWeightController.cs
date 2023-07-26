
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using userService.DTO;
using userService.Entities;
using userService.Services;

namespace userService.Controllers
{
    [Route("[controller]")]
    public class UserWeightController : Controller
    {
        private UserWeightService _userWeightService;

        public UserWeightController(UserWeightService userWeightService)
        {
            _userWeightService = userWeightService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserWeightModel>> GetUserWeight(int id)
        {
            var userWeight = await _userWeightService.GetUserWeight(id);

            if (userWeight == null)
            {
                return NotFound();
            }

            return userWeight;
        }

        [HttpGet("all/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserWeightModel>>> GetAllUserWeights(int id)
        {
            var userWeights = await _userWeightService.GetAllUserWeights(id);

            if (userWeights == null)
            {
                return NotFound();
            }

            return userWeights.ToList();
        }

        [HttpGet("latestWeight/{id}")]
        //[Authorize]
        public async Task<IActionResult> GetLatestWeightAsync(int id)
        {
            var latestWeight = await _userWeightService.GetLatestWeightLogAsync(id);

            if (latestWeight == null)
            {
                // Handle the case where there is no weight log for the user.
                return NotFound("No weight log found for this user.");
            }

            return Ok(latestWeight);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUserWeight(int id, UserWeightModel userWeight)
        {
            bool result = await _userWeightService.UpdateUserWeight(id, userWeight);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostUserWeight([FromBody] UserWeightModel userWeight)
        {
            var newUserWeight = await _userWeightService.CreateUserWeight(userWeight);

            return CreatedAtAction(nameof(GetUserWeight), new { id = newUserWeight.UserId }, newUserWeight);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserWeight(int id)
        {
            var result = await _userWeightService.DeleteUserWeight(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}


