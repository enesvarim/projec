using projec.Dtos;

namespace projec.Services
{
    public interface IPlayerService
    {
        Task<PlayerProfileDto> GetPlayerProfile(int userId);
    }
}
