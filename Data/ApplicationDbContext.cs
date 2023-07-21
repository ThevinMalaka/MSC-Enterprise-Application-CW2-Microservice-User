//using System;
//namespace userService.Data
//{
//	public class ApplicationDbContext
//	{
//		public ApplicationDbContext()
//		{
//		}
//	}
//}


using Microsoft.EntityFrameworkCore;
using userService;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
}

