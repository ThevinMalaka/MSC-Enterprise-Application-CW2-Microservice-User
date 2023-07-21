using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using userService.Entities;
using userService.Services;

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
        public async Task<ActionResult<CheatMealModel>> CreateCheatMeal([FromBody] CheatMealModel cheatMeal)
        {
            var createdCheatMeal = await _cheatMealService.CreateCheatMealAsync(cheatMeal);
            return CreatedAtAction(nameof(GetCheatMealById), new { id = createdCheatMeal.Id }, createdCheatMeal);
        }

        // PUT: api/CheatMeals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCheatMeal(int id, [FromBody] CheatMealModel cheatMeal)
        {
            if (id != cheatMeal.Id)
            {
                return BadRequest();
            }

            await _cheatMealService.UpdateCheatMealAsync(cheatMeal);
            return NoContent();
        }

        // DELETE: api/CheatMeals/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCheatMeal(int id)
        //{
        //    await _cheatMealService.DeleteCheatMealAsync(id);
        //    return NoContent();
        //}
    }
}

