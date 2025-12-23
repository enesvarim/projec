using projec.Dtos;
using projec.Models;

namespace projec.Services
{
    public interface ITeamService
    {
        Task<Team> CreateTeam(int captainId, TeamDto request);
        Task<string> AddPlayerToTeam(int captainId, AddPlayerDto request);
        Task<string> RemovePlayerFromTeam(int captainId, RemovePlayerDto request);
        Task<string> DeleteTeam(int captainId, int teamId);
        Task<string> LeaveTeam(int userId, int teamId);
        Task<List<Team>> GetMyTeams(int userId);
    }
}
