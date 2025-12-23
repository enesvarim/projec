namespace projec.Dtos
{
    public class MatchResultDto
    {
        public int MatchId { get; set; }
        public int RequesterScore { get; set; }
        public int OpponentScore { get; set; }
        public List<PlayerStatDto> PlayerStats { get; set; } // Oynayan tüm oyuncuların istatistikleri
    }
}
