using Microsoft.EntityFrameworkCore;
using projec.Data;
using projec.Dtos;
using projec.Models;

namespace projec.Services
{
    public class TeamService : ITeamService
    {
        private readonly AppDbContext _context;

        public TeamService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Team> CreateTeam(int captainId, TeamDto request)
        {
            var team = new Team
            {
                Name = request.Name,
                CaptainId = captainId,
                PlayerCount = request.PlayerCount,
                CreatedAt = DateTime.Now
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            // oluşturan oyuncuyu kaptan olarak atama
            var teamPlayer = new TeamPlayer
            {
                TeamId = team.Id,
                UserId = captainId
            };
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();

            return team;
        }

        public async Task<string> AddPlayerToTeam(int captainId, AddPlayerDto request)
        {
            var team = await _context.Teams.FindAsync(request.TeamId);
            if (team == null) return "Takım bulunamadı.";
            
            if (team.CaptainId != captainId) return "Bu takıma oyuncu ekleme yetkiniz yok. Sadece kaptan ekleyebilir.";

            // Takım dolu mu?
            var currentCount = await _context.TeamPlayers.CountAsync(tp => tp.TeamId == request.TeamId);
            if (currentCount >= team.PlayerCount)
            {
                return $"Takım kapasitesi dolu! ({team.PlayerCount} kişilik)";
            }

            var userToAdd = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (userToAdd == null) return "Kullanıcı bulunamadı.";

            // Zaten takımda mı?
            var isAlreadyMember = await _context.TeamPlayers.AnyAsync(tp => tp.TeamId == request.TeamId && tp.UserId == userToAdd.Id);
            if (isAlreadyMember) return "Bu kullanıcı zaten takımda.";

            var teamPlayer = new TeamPlayer
            {
                TeamId = request.TeamId,
                UserId = userToAdd.Id
            };

            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();

            return "Oyuncu takıma eklendi!";
        }

        public async Task<string> RemovePlayerFromTeam(int captainId, RemovePlayerDto request)
        {
            var team = await _context.Teams.FindAsync(request.TeamId);
            if (team == null) return "Takım bulunamadı.";

            if (team.CaptainId != captainId) return "Bu takımdan oyuncu çıkarma yetkiniz yok. Sadece kaptan çıkarabilir.";

            var userToRemove = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (userToRemove == null) return "Kullanıcı bulunamadı.";

            if (userToRemove.Id == captainId) return "Kendinizi takımdan atamazsınız! Takımı dağıtmak için silmelisiniz.";

            var teamPlayer = await _context.TeamPlayers.FirstOrDefaultAsync(tp => tp.TeamId == request.TeamId && tp.UserId == userToRemove.Id);
            if (teamPlayer == null) return "Bu oyuncu zaten takımda değil.";

            _context.TeamPlayers.Remove(teamPlayer);
            await _context.SaveChangesAsync();

            return "Oyuncu takımdan çıkarıldı.";
        }

        public async Task<string> DeleteTeam(int captainId, int teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null) return "Takım bulunamadı.";

            if (team.CaptainId != captainId) return "Takımı silme yetkiniz yok. Sadece kaptan silebilir.";

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return "Takım başarıyla silindi.";
        }

        public async Task<string> LeaveTeam(int userId, int teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null) return "Takım bulunamadı.";

            if (team.CaptainId == userId) return "Kaptan takımdan çıkamaz! Takımı silmelisiniz.";

            var teamPlayer = await _context.TeamPlayers.FirstOrDefaultAsync(tp => tp.TeamId == teamId && tp.UserId == userId);
            if (teamPlayer == null) return "Zaten bu takımda değilsiniz.";

            _context.TeamPlayers.Remove(teamPlayer);
            await _context.SaveChangesAsync();

            return "Takımdan ayrıldınız.";
        }

        public async Task<List<Team>> GetMyTeams(int userId)
        {
            // Kullanıcının oyuncu olarak bulunduğu takımlar
            var teamIds = await _context.TeamPlayers
                .Where(tp => tp.UserId == userId)
                .Select(tp => tp.TeamId)
                .ToListAsync();

            var teams = await _context.Teams
                .Where(t => teamIds.Contains(t.Id))
                .Include(t => t.Players)
                .ThenInclude(tp => tp.User)
                .ToListAsync();

            return teams;
        }
    }
}
