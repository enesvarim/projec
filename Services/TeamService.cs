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

            // Kaptanı da takıma oyuncu olarak ekleyelim
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
