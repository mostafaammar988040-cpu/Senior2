using Microsoft.AspNetCore.Mvc;
using Senior2.Api.Models;
using Senior2.Api.Services;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportController : ControllerBase
    {
        private readonly EmailService _emailService;

        public SupportController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendSupport(
            [FromBody] SupportRequest request)
        {
            var body = $@"
                <h3>New Support Request</h3>
                <p><b>Name:</b> {request.Name}</p>
                <p><b>Email:</b> {request.Email}</p>
                <p><b>Message:</b><br/>{request.Message}</p>
            ";

            await _emailService.SendEmailAsync(
                "AhlaBhalTalleh@gmail.com",
                request.Subject,
                body
            );

            return Ok(new { message = "Support request sent successfully" });
        }
    }
}