using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace userService.Entities
{
    public class CheatMealModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Calories { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public UserModel User { get; set; }

        [ForeignKey("UserModel")]
        public int UserId { get; set; }
    }
}

