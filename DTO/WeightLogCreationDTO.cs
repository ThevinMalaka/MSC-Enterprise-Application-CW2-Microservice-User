using System;
namespace userService.DTO
{
    public class WeightLogCreationDTO
    {
        public DateTime Date { get; set; }
        public double Weight { get; set; }
        public int UserId { get; set; }
    }
}

