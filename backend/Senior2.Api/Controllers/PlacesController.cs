using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlacesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlacesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetPlaces(
            string? category,
            int? activityType)
        {
            var query = _context.Places
                .Include(p => p.Category)
                .AsQueryable();

            // Category filter
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Slug == category);
            }

            // 🔥 NEW — Activity filter
            if (activityType.HasValue)
            {
                query = query.Where(p =>
                    p.ActivityTypeId == activityType.Value);
            }

            var places = await query
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Location,
                    p.Price,
                    p.ImageUrl,
                    Category = p.Category.Name,
                    Slug = p.Category.Slug
                })
                .ToListAsync();

            return Ok(places);
        }
    }
}
