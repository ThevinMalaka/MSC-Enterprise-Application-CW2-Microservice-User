using System;
using Microsoft.EntityFrameworkCore;
using userService.Entities;

namespace userService.Services
{
	public class CheatMealService
    {
        private readonly ApplicationDbContext _context;

        public CheatMealService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CheatMealModel>> GetCheatMealByIdAsync(int id)
        {
            return await _context.CheatMeals
                          .Include(cm => cm.User) // Include User in the returned data
                          .Where(cm => cm.UserId == id) // Filter by UserId
                          .OrderByDescending(cm => cm.Id) // Order by Id
                          .ToListAsync();
        }


        public async Task<CheatMealModel> CreateCheatMealAsync(CheatMealModel cheatMeal)
        {
            _context.CheatMeals.Add(cheatMeal);
            await _context.SaveChangesAsync();
            return cheatMeal;
        }

        public async Task UpdateCheatMealAsync(CheatMealModel cheatMeal)
        {
            _context.CheatMeals.Update(cheatMeal);
            await _context.SaveChangesAsync();
        }

    }
}

