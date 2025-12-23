using projec.Dtos;
using projec.Models;

namespace projec.Services
{
    public interface IFriendService
    {
        Task<string> SendFriendRequest(int requesterId, string receiverUsername);
        Task<string> AcceptFriendRequest(int requestId, int userId);
        Task<string> RejectFriendRequest(int requestId, int userId);
        Task<List<User>> GetFriends(int userId);
        Task<List<Friendship>> GetPendingRequests(int userId);
    }
}
