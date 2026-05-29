using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.DTOs;
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
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllSupportRequests()
        {
            var requests = await _context.Set<SupportRequest>()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(requests);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("reply")]
        public async Task<IActionResult> ReplyToSupport(int id, [FromBody] ReplyDto dto)
        {
            var request = await _context.Set<SupportRequest>().FindAsync(id);

            if (request == null)
                return NotFound();

            await _emailService.SendEmailAsync(
                request.Email,
                "Reply to your support request",
                dto.Message
            );

            request.IsReplied = true;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}