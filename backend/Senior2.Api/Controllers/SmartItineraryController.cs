using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.Models;
using Senior2.Api.Services; // <-- make sure to include this
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmartItineraryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RecommendationService _recommendationService;

        public SmartItineraryController(AppDbContext context, RecommendationService recommendationService)
        {
            _context = context;
            _recommendationService = recommendationService;
        }

        // =========================
        // GENERATE TRIP
        // =========================
        [HttpPost]
        public async Task<IActionResult> GenerateTrip([FromBody] SmartItineraryRequest request)
        {
            var allPlaces = await _context.Places
                .Include(p => p.ActivityType)
                .ToListAsync();

            var activitiesRequested =
                JsonSerializer.Deserialize<List<string>>(request.ActivitiesJson)
                ?? new List<string>();

            // FILTER ACTIVITIES
            var activities = allPlaces
                .Where(p =>
                    p.CategoryId == 1 &&
                    p.ActivityType != null &&
                    activitiesRequested.Any(a =>
                        a.ToLower() == p.ActivityType.Name.ToLower()))
                .ToList();

            // RESTAURANTS
            var restaurants = allPlaces
                .Where(p => p.CategoryId == 2)
                .ToList();

            // HOTELS / GUESTHOUSES
            var hotels = allPlaces
                .Where(p => p.CategoryId == 3)
                .ToList();

            // INCLUDE SAVED PLACES
            if (request.IncludeSavedPlaces)
            {
                var savedPlaces = await _context.TripPlanPlaces
                    .Include(p => p.Place)
                    .ThenInclude(pl => pl.ActivityType)
                    .Where(p => p.TripPlan.UserId == request.UserId)
                    .Select(p => p.Place)
                    .ToListAsync();

                activities = activities
                    .Concat(savedPlaces)
                    .GroupBy(p => p.Id)
                    .Select(g => g.First())
                    .ToList();
            }

            // SELECT ONE HOTEL
            var stay = hotels.FirstOrDefault();

            // CALCULATE TRIP DAYS
            var totalDays = (request.EndDate - request.StartDate).Days + 1;
            if (totalDays <= 0) totalDays = 1;

            // Shuffle activities for variety
            var random = new Random();
            activities = activities.OrderBy(a => random.Next()).ToList();

            // GROUP ACTIVITIES BY REGION
            var groupedByRegion = activities
                .GroupBy(p =>
                {
                    if (p.Location.ToLower().Contains("beirut")) return "Beirut";
                    if (p.Location.ToLower().Contains("tripoli")) return "North";
                    if (p.Location.ToLower().Contains("byblos")) return "Mount Lebanon";
                    if (p.Location.ToLower().Contains("baalbek")) return "Bekaa";
                    return "Other";
                })
                .ToList();

            var itinerary = new List<object>();

            if (groupedByRegion.Count == 0)
            {
                // Fallback: call AI directly
                var aiSuggestions = await _recommendationService.GetItineraryRecommendation(
                    "Lebanon",                // or request.TripType / region
                    request.TripType,
                    request.BudgetPerDay,
                    request.Travelers.ToString()
                );

                return Ok(new
                {
                    stay = (object)null,
                    itinerary = aiSuggestions
                });
            }
            for (int day = 1; day <= totalDays; day++)
            {
                var regionGroup = groupedByRegion
                    .ElementAtOrDefault((day - 1) % groupedByRegion.Count);

                List<object> dayActivities = new List<object>();

                if (regionGroup != null)
                {
                    // Rotate activities so each day gets different ones
                    dayActivities = regionGroup
                        .Skip(((day - 1) * 2) % regionGroup.Count())
                        .Take(2)
                        .Select(p => (object)new
                        {
                            id = p.Id,
                            name = p.Name,
                            location = p.Location,
                            imageUrl = p.ImageUrl,
                            activityType = p.ActivityType?.Name
                        })
                        .ToList();

                    if (!dayActivities.Any())
                    {
                        dayActivities = regionGroup
                            .Take(2)
                            .Select(p => (object)new
                            {
                                id = p.Id,
                                name = p.Name,
                                location = p.Location,
                                imageUrl = p.ImageUrl,
                                activityType = p.ActivityType?.Name
                            })
                            .ToList();
                    }
                }

                // Cycle restaurants properly
                var restaurant = restaurants.ElementAtOrDefault((day - 1) % restaurants.Count);

                // 🔥 NEW: Call OpenAI for extra itinerary suggestions
                var aiSuggestions = await _recommendationService.GetItineraryRecommendation(
     regionGroup?.Key ?? "Lebanon",
     request.TripType,
     (int)request.BudgetPerDay,   // 👈 cast here
     request.Travelers.ToString()
 );

                itinerary.Add(new
                {
                    day,
                    region = regionGroup?.Key,
                    stay = stay != null ? new
                    {
                        stay.Id,
                        stay.Name,
                        stay.Location,
                        stay.ImageUrl
                    } : null,
                    activities = dayActivities,
                    restaurant = restaurant != null ? new
                    {
                        restaurant.Id,
                        restaurant.Name,
                        restaurant.Location
                    } : null,
                    aiRecommendations = aiSuggestions // <-- merged AI suggestions
                });
            }

            request.Status = "Active";
            request.ItineraryJson = JsonSerializer.Serialize(itinerary);

            _context.Set<SmartItineraryRequest>().Add(request);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                stay,
                itinerary
            });
        }

        // =========================
        // GET ALL TRIPS (ADMIN)
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
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
                        trip.CreatedAt,
                        trip.ItineraryJson
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