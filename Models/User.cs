using System.ComponentModel.DataAnnotations;

namespace projec.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int BirthYear { get; set; }

        public Position Position { get; set; }

        public int MatchCount { get; set; }

        public int GoalCount { get; set; }

        public int AssistCount { get; set; }

        public UserRole Role { get; set; } = UserRole.User;
    }
}
