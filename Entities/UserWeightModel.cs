using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace userService.Entities
{
	public class UserWeightModel
	{
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Weight { get; set; }
        public UserModel User { get; set; }

        [ForeignKey("UserModel")]
        public int UserId { get; set; }
    }
}

