using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projec.Services;

namespace projec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerStats(int id)
        {
            var profile = await _playerService.GetPlayerProfile(id);
            if (profile == null) return NotFound("Oyuncu bulunamadı.");

            return Ok(profile);
        }
        
    
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            return await GetPlayerStats(userId);
        }
    }
}
