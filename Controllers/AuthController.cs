using Microsoft.AspNetCore.Mvc;
using projec.Dtos;
using projec.Services;

namespace projec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var user = await _authService.Register(request);
            if (user == null)
            {
                return BadRequest("Bu kullanıcı adı zaten alınmış.");
            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var result = await _authService.Login(request);
            if (result == "Kullanıcı bulunamadı." || result == "Yanlış şifre.")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
