using projec.Dtos;
using projec.Models;

namespace projec.Services
{
    public interface ITeamService
    {
        Task<Team> CreateTeam(int captainId, TeamDto request);
        Task<string> AddPlayerToTeam(int captainId, AddPlayerDto request);
        Task<List<Team>> GetMyTeams(int userId);
    }
}
