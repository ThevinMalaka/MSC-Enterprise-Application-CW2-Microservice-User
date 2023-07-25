using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using userService.DTO;
using userService.Entities;
using userService.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace userService.Controllers
{
    [Route("[controller]")]
    public class CheatMealController : Controller
    {
        private readonly CheatMealService _cheatMealService;

        public CheatMealController(CheatMealService cheatMealService)
        {
            _cheatMealService = cheatMealService;
        }

        // GET: api/CheatMeals/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCheatMealById(int id)
        {
            var cheatMeal = await _cheatMealService.GetCheatMealByIdAsync(id);

            if (cheatMeal == null)
            {
                return NotFound();
            }

            return Ok(cheatMeal);
        }

        // POST: api/CheatMeals
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CheatMealModel>> CreateCheatMeal([FromBody] CheatMealCreationDTO cheatMeal)
        {
            var createdCheatMeal = await _cheatMealService.CreateCheatMealAsync(new CheatMealModel
            {
                Name = cheatMeal.Name,
                Calories = cheatMeal.Calories,
                Description = cheatMeal.Description,
                Type = cheatMeal.Type,
                Date = cheatMeal.Date,
                UserId = cheatMeal.UserId
            });
            return CreatedAtAction(nameof(GetCheatMealById), new { id = createdCheatMeal.Id }, createdCheatMeal);
        }

        // PUT: api/CheatMeals/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCheatMeal(int id, [FromBody] CheatMealModel cheatMeal)
        {
            if (id != cheatMeal.Id)
            {
                return BadRequest();
            }

            await _cheatMealService.UpdateCheatMealAsync(cheatMeal);
            return NoContent();
        }

    }
}

