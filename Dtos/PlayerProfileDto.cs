using projec.Models;

namespace projec.Dtos
{
    public class PlayerProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Position { get; set; } // Enum string representation
        public int MatchCount { get; set; }
        public int GoalCount { get; set; }
        public int AssistCount { get; set; }
        public double GoalsPerMatch { get; set; }
        public double AssistsPerMatch { get; set; }
    }
}
