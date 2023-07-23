using System;
namespace userService.DTO
{
    public class CheatMealCreationDTO
    {
        public string? Name { get; set; }
        public string Calories { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}

