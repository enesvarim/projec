using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projec.Dtos;
using projec.Models;
using projec.Services;

namespace projec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : ControllerBase
    {
        private readonly IFieldService _fieldService;

        public FieldController(IFieldService fieldService)
        {
            _fieldService = fieldService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")] // Sadece Adminler ekleyebilir
        public async Task<IActionResult> AddField(FieldDto request)
        {
            var field = await _fieldService.AddField(request);
            return Ok(field);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFields()
        {
            var fields = await _fieldService.GetAllFields();
            return Ok(fields);
        }
    }
}
