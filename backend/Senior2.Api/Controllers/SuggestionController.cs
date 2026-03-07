using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.Models;
using Senior2.Api.DTOs.Suggestion;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/suggestion")]
    public class SuggestionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SuggestionController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

            return Ok(suggestion);
        }

        // =========================
        // GET USER SUGGESTIONS
        // =========================
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

        // =========================
        // GET ALL SUGGESTIONS (ADMIN)
        // =========================
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
                        : "Unknown User"
                })
                .ToListAsync();

            return Ok(suggestions);
        }
    }
}