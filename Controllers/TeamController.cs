using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projec.Dtos;
using projec.Services;

namespace projec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTeam(TeamDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var team = await _teamService.CreateTeam(userId, request);
            return Ok(team);
        }

        [HttpPost("add-player")]
        public async Task<IActionResult> AddPlayer(AddPlayerDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _teamService.AddPlayerToTeam(userId, request);
            
            if (result == "Oyuncu takıma eklendi!") return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("remove-player")]
        public async Task<IActionResult> RemovePlayer(RemovePlayerDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _teamService.RemovePlayerFromTeam(userId, request);

            if (result == "Oyuncu takımdan çıkarıldı.") return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _teamService.DeleteTeam(userId, id);

            if (result == "Takım başarıyla silindi.") return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("leave/{teamId}")]
        public async Task<IActionResult> LeaveTeam(int teamId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _teamService.LeaveTeam(userId, teamId);

            if (result == "Takımdan ayrıldınız.") return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("my-teams")]
        public async Task<IActionResult> GetMyTeams()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var teams = await _teamService.GetMyTeams(userId);
            return Ok(teams);
        }
    }
}
