using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.Models;
using System.Text.Json;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmartItineraryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SmartItineraryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateTrip(
            [FromBody] SmartItineraryRequest request)
        {
            var allPlaces = await _context.Places
                .Include(p => p.ActivityType)
                .ToListAsync();

            var places = allPlaces
                .Where(p =>
                    p.CategoryId == 1 &&
                    p.ActivityType != null &&
                    request.ActivitiesJson
                        .ToLower()
                        .Contains(p.ActivityType.Name.ToLower()))
                .Take(6)
                .ToList();

            var result = places.Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Location,
                p.Price,
                p.ImageUrl,
                ActivityType = p.ActivityType.Name
            });

            // 🔥 SAVE TRIP TO DATABASE
            _context.Set<SmartItineraryRequest>().Add(request);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                recommendedPlaces = result
            });
        }
    }
}