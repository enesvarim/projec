using System.ComponentModel.DataAnnotations.Schema;

namespace projec.Models
{
    public class TeamPlayer
    {
        public int Id { get; set; }

        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;
    }
}
