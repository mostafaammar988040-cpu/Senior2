using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.Models;
using Senior2.Api.DTOs.Suggestion;
using Senior2.Api.Services;


namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/suggestion")]
    public class SuggestionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;

        public SuggestionController(
       AppDbContext context,
       IWebHostEnvironment env,
       EmailService emailService,
       IConfiguration config)
        {
            _context = context;
            _env = env;
            _emailService = emailService;
            _config = config;
        }

        // =========================
        // CREATE SUGGESTION
        // =========================
        [HttpPost]
        public async Task<IActionResult> CreateSuggestion(
            [FromForm] CreateSuggestionDto dto,
            IFormFile? image)
        {
            string? imageUrl = null;

            if (image != null && image.Length > 0)
            {
                var folder = Path.Combine(_env.WebRootPath, "suggestions");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imageUrl = $"/suggestions/{fileName}";
            }

            var suggestion = new Suggestion
            {
                UserId = dto.UserId,
                Type = dto.Type,
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.Suggestions.Add(suggestion);
            await _context.SaveChangesAsync();

            var user = await _context.Users
           .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            var userName = user != null
                ? $"{user.FirstName} {user.LastName}"
                : "Unknown User";

            var userEmail = user?.Email ?? "Unknown Email";

            var receiverEmail = _config["EmailSettings:SenderEmail"];

            var emailBody = $@"
    <h2>New User Suggestion Received</h2>

    <p><b>User:</b> {userName}</p>
    <p><b>Email:</b> {userEmail}</p>
    <p><b>Type:</b> {dto.Type}</p>
    <p><b>Title:</b> {dto.Title}</p>
    <p><b>Description:</b> {dto.Description}</p>
    <p><b>Location:</b> {(string.IsNullOrWhiteSpace(dto.Location) ? "Not provided" : dto.Location)}</p>

    <hr />

    <p>This suggestion was submitted from AHLA BHAL TALLEH platform.</p>
";

            try
            {
                if (!string.IsNullOrWhiteSpace(receiverEmail))
                {
                    await _emailService.SendEmailAsync(
                        receiverEmail,
                        $"New Suggestion: {dto.Title}",
                        emailBody
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Suggestion email failed: {ex.Message}");
            }

            return Ok(new
            {
                message = "Suggestion submitted successfully",
                suggestion
            });
        }

       

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserSuggestions(int userId)
        {
            var suggestions = await _context.Suggestions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Description,
                    s.Type,
                    s.Location,
                    s.ImageUrl,
                    s.CreatedAt
                })
                .ToListAsync();

            return Ok(suggestions);
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllSuggestions()
        {
            var suggestions = await _context.Suggestions
                .Include(s => s.User)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Description,
                    s.Type,
                    s.Location,
                    s.ImageUrl,
                    s.CreatedAt,
                    userName = s.User != null
                        ? s.User.FirstName + " " + s.User.LastName
                        : "Unknown User",
                    userEmail = s.User != null
                        ? s.User.Email
                        : "Unknown Email"
                })
                .ToListAsync();

            return Ok(suggestions);
        }
    }
}