using Microsoft.EntityFrameworkCore;
using projec.Data;
using projec.Dtos;

namespace projec.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly AppDbContext _context;

        public PlayerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PlayerProfileDto> GetPlayerProfile(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            return new PlayerProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Position = user.Position.ToString(),
                MatchCount = user.MatchCount,
                GoalCount = user.GoalCount,
                AssistCount = user.AssistCount,
                GoalsPerMatch = user.MatchCount > 0 ? Math.Round((double)user.GoalCount / user.MatchCount, 2) : 0,
                AssistsPerMatch = user.MatchCount > 0 ? Math.Round((double)user.AssistCount / user.MatchCount, 2) : 0
            };
        }
    }
}
