using System.ComponentModel.DataAnnotations.Schema;

namespace projec.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int PlayerCount { get; set; }

        public int CaptainId { get; set; }

        [ForeignKey("CaptainId")]
        public User Captain { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property - Bir takımın birden fazla oyuncusu olur
        public List<TeamPlayer> Players { get; set; }
    }
}
