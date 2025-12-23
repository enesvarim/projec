namespace projec.Dtos
{
    public class MatchRequestDto
    {
        public int RequesterTeamId { get; set; }
        public int OpponentTeamId { get; set; }
        public int FieldId { get; set; }
        public DateTime MatchDate { get; set; }
    }
}
