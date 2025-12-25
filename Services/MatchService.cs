using Microsoft.EntityFrameworkCore;
using projec.Data;
using projec.Dtos;
using projec.Models;

namespace projec.Services
{
    public class MatchService : IMatchService
    {
        private readonly AppDbContext _context;

        public MatchService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> SendMatchRequest(int captainId, MatchRequestDto request)
        {
            var requestingTeam = await _context.Teams.FindAsync(request.RequesterTeamId);
            if (requestingTeam == null) 
                return "Kendi takiminiz bulunamadi.";

            if (requestingTeam.CaptainId != captainId) 
                return "Sadece takım kaptani maç isteği gönderebilir.";

            var opponentTeam = await _context.Teams.FindAsync(request.OpponentTeamId);
            if (opponentTeam == null)
                return "Rakip takim bulunamadı.";

            if (requestingTeam.Id == opponentTeam.Id) 
                return "Kendi kendinize maç yapamazsiniz.";

            // Kişi sayısı kontrolü
            if (requestingTeam.PlayerCount != opponentTeam.PlayerCount)
                return $"Takimlarin oyuncu sayilarıieşit değil! ({requestingTeam.Name}: {requestingTeam.PlayerCount} - {opponentTeam.Name}: {opponentTeam.PlayerCount})";

            var field = await _context.Fields.FindAsync(request.FieldId);
            if (field == null) 
                return "Seçilen saha bulunamadı.";
            if (!field.IsActive) 
                return "Seçilen saha aktif değil.";

            var matchRequest = new MatchRequest
            {
                RequesterTeamId = request.RequesterTeamId,
                OpponentTeamId = request.OpponentTeamId,
                FieldId = request.FieldId,
                MatchDate = request.MatchDate,
                Status = MatchStatus.Pending
            };

            _context.MatchRequests.Add(matchRequest);
            await _context.SaveChangesAsync();

            return "Maç isteği gönderildi!";
        }

        public async Task<string> RespondToMatchRequest(int captainId, int requestId, bool isAccepted)
        {
            var matchRequest = await _context.MatchRequests
                .Include(m => m.OpponentTeam)
                .FirstOrDefaultAsync(m => m.Id == requestId);

            if (matchRequest == null) return "Maç isteği bulunamadı.";

            // İsteği cevaplayacak kişi, RAKİP takımın kaptanı olmalı
            if (matchRequest.OpponentTeam.CaptainId != captainId)
                return "Bu isteği cevaplama yetkiniz yok. Sadece rakip takımın kaptanı cevaplayabilir.";

            if (matchRequest.Status != MatchStatus.Pending) return "Bu istek zaten sonuçlanmış.";

            matchRequest.Status = isAccepted ? MatchStatus.Accepted : MatchStatus.Rejected;
            await _context.SaveChangesAsync();

            return isAccepted ? "Maç isteği kabul edildi! Sahada görüşürüz. ⚽" : "Maç isteği reddedildi.";
        }

        public async Task<List<MatchRequest>> GetMyTeamRequests(int userId)
        {
            // Kullanıcının üye olduğu takımların ID'lerini bul
            var myTeamIds = await _context.TeamPlayers
                .Where(tp => tp.UserId == userId)
                .Select(tp => tp.TeamId)
                .ToListAsync();

            // Bu takımların gönderdiği veya aldığı istekleri getir
            var requests = await _context.MatchRequests
                .Include(m => m.RequesterTeam)
                .Include(m => m.OpponentTeam)
                .Include(m => m.Field)
                .Where(m => myTeamIds.Contains(m.RequesterTeamId) || myTeamIds.Contains(m.OpponentTeamId))
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return requests;
        }

        public async Task<List<MatchRequest>> GetAcceptedMatches(int userId)
        {
            // Kullanıcının üye olduğu takımlar
            var myTeamIds = await _context.TeamPlayers
                .Where(tp => tp.UserId == userId)
                .Select(tp => tp.TeamId)
                .ToListAsync();

            // Kabul edilmiş (Status == Accepted) ve bu takımların içinde olduğu maçlar
            var matches = await _context.MatchRequests
                .Include(m => m.RequesterTeam)
                .Include(m => m.OpponentTeam)
                .Include(m => m.Field)
                .Where(m => (myTeamIds.Contains(m.RequesterTeamId) || myTeamIds.Contains(m.OpponentTeamId)) 
                            && m.Status == MatchStatus.Accepted)
                .OrderByDescending(m => m.MatchDate)
                .ToListAsync();

            return matches;
        }

        public async Task<string> CompleteMatch(int captainId, MatchResultDto result)
        {
            var match = await _context.MatchRequests
                .Include(m => m.RequesterTeam)
                .Include(m => m.OpponentTeam)
                .FirstOrDefaultAsync(m => m.Id == result.MatchId);

            if (match == null) return "Maç bulunamadı.";

            // Sadece maçı talep eden veya kabul eden takım kaptanı sonucu girebilsin (Basitlik için: İki kaptan da girebilir diyelim)
            if (match.RequesterTeam.CaptainId != captainId && match.OpponentTeam.CaptainId != captainId)
                return "Maç sonucunu sadece takım kaptanları girebilir.";

            if (match.Status != MatchStatus.Accepted) return "Tamamlanacak maç 'Kabul Edildi' durumunda olmalıdır.";

            // Maç Skorunu Güncelle
            match.RequesterScore = result.RequesterScore;
            match.OpponentScore = result.OpponentScore;
            match.Status = MatchStatus.Completed;

            // İstatistikleri Güncelle
            if (result.PlayerStats != null)
            {
                foreach (var stat in result.PlayerStats)
                {
                    var user = await _context.Users.FindAsync(stat.UserId);
                    if (user != null)
                    {
                        user.MatchCount++; // Maç sayısı 1 artar
                        user.GoalCount += stat.Goals; // Goller eklenir
                        user.AssistCount += stat.Assists; // Asistler eklenir
                    }
                }
            }

            await _context.SaveChangesAsync();
            return "Maç sonucu ve istatistikler kaydedildi";
        }
    }
}
