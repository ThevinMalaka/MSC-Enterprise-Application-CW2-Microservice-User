using System;
namespace userService.Entities
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public DateTime DateOfBirth { get; set; }

        public ICollection<UserWeightModel> WeightLog { get; set; }
    }

    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginLog
    {
        public string Email { get; set; }
        public DateTime LoginTime { get; set; }
        public string IPAddress { get; set; }
    }

    public class UserLoginFailedLog
    {
        public string AttemptedEmail { get; set; }
        public DateTime AttemptTime { get; set; }
        public string IPAddress { get; set; }
    }
}

