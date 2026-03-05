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

        // =========================
        // GENERATE TRIP
        // =========================
        [HttpPost]
        public async Task<IActionResult> GenerateTrip(
            [FromBody] SmartItineraryRequest request)
        {
            var allPlaces = await _context.Places
                .Include(p => p.ActivityType)
                .ToListAsync();

            var activities =
                JsonSerializer.Deserialize<List<string>>(request.ActivitiesJson)
                ?? new List<string>();

            var places = allPlaces
                .Where(p =>
                    p.CategoryId == 1 &&
                    p.ActivityType != null &&
                    activities.Any(a =>
                        a.ToLower() == p.ActivityType.Name.ToLower()))
                .Take(6)
                .ToList();

            // default status
            request.Status = "Active";

            _context.Set<SmartItineraryRequest>().Add(request);

            await _context.SaveChangesAsync();

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

            return Ok(new
            {
                recommendedPlaces = result
            });
        }

        // =========================
        // GET ALL TRIPS (ADMIN)
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            // load trips (entities) to update statuses
            var tripEntities = await _context.Set<SmartItineraryRequest>()
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            foreach (var trip in tripEntities)
            {
                if (trip.Status == "Active" && trip.EndDate < DateTime.UtcNow)
                {
                    trip.Status = "Completed";
                }
            }

            await _context.SaveChangesAsync();

            // return trips joined with users to get username
            var trips = await _context.Set<SmartItineraryRequest>()
                .Join(_context.Users,
                    trip => trip.UserId,
                    user => user.Id,
                    (trip, user) => new
                    {
                        trip.Id,
                        trip.UserId,
                        userName = user.FirstName + " " + user.LastName,
                        trip.Travelers,
                        trip.StartDate,
                        trip.EndDate,
                        trip.BudgetPerDay,
                        trip.TripType,
                        trip.ActivitiesJson,
                        trip.Transport,
                        trip.SpecialRequirements,
                        trip.Status,
                        trip.CreatedAt
                    })
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return Ok(trips);
        }
        // =========================
        // DELETE TRIP
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var trip = await _context.Set<SmartItineraryRequest>()
                .FindAsync(id);

            if (trip == null)
                return NotFound("Trip not found");

            _context.Remove(trip);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Trip deleted successfully"
            });
        }

        // =========================
        // CANCEL TRIP
        // =========================
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelTrip(int id)
        {
            var trip = await _context.Set<SmartItineraryRequest>()
                .FindAsync(id);

            if (trip == null)
                return NotFound("Trip not found");

            if (trip.Status == "Completed")
                return BadRequest("Completed trips cannot be cancelled");

            trip.Status = "Cancelled";

            await _context.SaveChangesAsync();

            return Ok(trip);
        }
    }
}