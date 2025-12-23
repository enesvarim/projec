using projec.Dtos;
using projec.Models;

namespace projec.Services
{
    public interface IAuthService
    {
        Task<User> Register(RegisterDto request);
        Task<string> Login(LoginDto request);
    }
}
