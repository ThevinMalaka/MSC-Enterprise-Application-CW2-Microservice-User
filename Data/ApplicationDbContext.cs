
using Microsoft.EntityFrameworkCore;
using userService;
using userService.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<UserWeightModel> UserWeightsLogs { get; set; }
    public DbSet<CheatMealModel> CheatMeals { get; set; }
}

