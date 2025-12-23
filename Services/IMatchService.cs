using projec.Dtos;
using projec.Models;

namespace projec.Services
{
    public interface IMatchService
    {
        Task<string> SendMatchRequest(int captainId, MatchRequestDto request);
        Task<string> RespondToMatchRequest(int captainId, int requestId, bool isAccepted); // True: Accept, False: Reject
        Task<List<MatchRequest>> GetMyTeamRequests(int userId);
        Task<List<MatchRequest>> GetAcceptedMatches(int userId);
        Task<string> CompleteMatch(int captainId, MatchResultDto result);
    }
}
