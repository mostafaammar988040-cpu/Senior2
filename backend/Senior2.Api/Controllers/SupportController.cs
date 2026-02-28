using Microsoft.AspNetCore.Mvc;
using Senior2.Api.Data;
using Senior2.Api.Models;
using Senior2.Api.Services;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly AppDbContext _context;

        public SupportController(
            EmailService emailService,
            AppDbContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendSupport(
            [FromBody] SupportRequest request)
        {
            _context.Set<SupportRequest>().Add(request);
            await _context.SaveChangesAsync();

            var body = $@"
                <h3>New Support Request</h3>
                <p><b>Name:</b> {request.Name}</p>
                <p><b>Email:</b> {request.Email}</p>
                <p><b>Category:</b> {request.Category}</p>
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