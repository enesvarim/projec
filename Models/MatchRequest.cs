using System.ComponentModel.DataAnnotations.Schema;

namespace projec.Models
{
    public class MatchRequest
    {
        public int Id { get; set; }

        public int RequesterTeamId { get; set; }
        [ForeignKey("RequesterTeamId")]
        public Team RequesterTeam { get; set; }

        public int OpponentTeamId { get; set; }
        [ForeignKey("OpponentTeamId")]
        public Team OpponentTeam { get; set; }

        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public Field Field { get; set; }

        public DateTime MatchDate { get; set; }

        public int? RequesterScore { get; set; }
        public int? OpponentScore { get; set; }

        public MatchStatus Status { get; set; } = MatchStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
