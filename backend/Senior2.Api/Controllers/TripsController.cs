using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.Models;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TripsController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // ADD PLACE TO TRIP
        // =========================
        [HttpPost("add-place")]
        public async Task<IActionResult> AddPlaceToTrip(
            int userId,
            int placeId)
        {
            var trip = await _context.TripPlans
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (trip == null)
            {
                trip = new TripPlan
                {
                    UserId = userId,
                    Title = "My Lebanon Trip",
                        ItineraryJson = "[]" // empty itinerary

                };

                _context.TripPlans.Add(trip);
                await _context.SaveChangesAsync();
            }

            var exists = await _context.TripPlanPlaces
                .AnyAsync(p =>
                    p.TripPlanId == trip.Id &&
                    p.PlaceId == placeId);

            if (exists)
                return BadRequest("Place already added");

            var item = new TripPlanPlace
            {
                TripPlanId = trip.Id,
                PlaceId = placeId
            };

            _context.TripPlanPlaces.Add(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Place added to trip!" });
        }


        // =========================
        // GET USER TRIP PLACES
        // =========================
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetTripPlaces(int userId)
        {
            var trip = await _context.TripPlans
.SingleOrDefaultAsync(t => t.UserId == userId);
            if (trip == null)
                return Ok(new List<object>());

            var places = await _context.TripPlanPlaces
                .Include(p => p.Place)
                .Where(p => p.TripPlanId == trip.Id)
                .Select(p => new
                {
                    p.Place.Id,
                    p.Place.Name,
                    p.Place.ImageUrl,
                    p.Place.Location
                })
                .ToListAsync();

            return Ok(places);
        }
    }
}