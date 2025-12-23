using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projec.Dtos;
using projec.Services;

namespace projec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Sadece giriş yapmış kullanıcılar
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpPost("send-request")]
        public async Task<IActionResult> SendRequest(FriendRequestDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _friendService.SendFriendRequest(userId, request.ReceiverUsername);
            
            if (result == "İstek gönderildi.") return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("accept/{requestId}")]
        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _friendService.AcceptFriendRequest(requestId, userId);
            
            if (result == "Arkadaşlık kabul edildi!") return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("reject/{requestId}")]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _friendService.RejectFriendRequest(requestId, userId);

            if (result == "İstek reddedildi.") return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("my-friends")]
        public async Task<IActionResult> GetMyFriends()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var friends = await _friendService.GetFriends(userId);
            return Ok(friends);
        }

        [HttpGet("pending-requests")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var requests = await _friendService.GetPendingRequests(userId);
            return Ok(requests);
        }
    }
}
