using ChauffeurApp.Application.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChauffeurApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailConfirmationController : BaseAPIController
    {
        private readonly IAuthService _authService;

        public EmailConfirmationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _authService.VerifyEmail(userId, token);

            if (result.IsSuccess && result.Value is bool && (bool)(object)result.Value == true)
            {
                return Ok("Email verified successfully");
            }
            else
            {
                return BadRequest("Email verification failed");
            }
        }
    }
}
