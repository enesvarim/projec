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
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpPost("send-request")]
        public async Task<IActionResult> SendRequest(MatchRequestDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _matchService.SendMatchRequest(userId, request);

            if (result == "Maç isteği gönderildi!") return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("respond/{requestId}")]
        public async Task<IActionResult> RespondToRequest(int requestId, [FromQuery] bool accept)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _matchService.RespondToMatchRequest(userId, requestId, accept);

            if (result.Contains("kabul edildi") || result.Contains("reddedildi")) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var requests = await _matchService.GetMyTeamRequests(userId);
            return Ok(requests);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetMatchHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var history = await _matchService.GetAcceptedMatches(userId);
            return Ok(history);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteMatch(MatchResultDto result)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var response = await _matchService.CompleteMatch(userId, result);
            
            if (response.Contains("kaydedildi")) return Ok(response);
            return BadRequest(response);
        }
    }
}
