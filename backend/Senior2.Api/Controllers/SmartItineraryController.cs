using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.Models;
using Senior2.Api.Services;
using System.Text.Json;

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
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(errors);
            }
            var allPlaces = await _context.Places
                .Include(p => p.ActivityType)
                .ToListAsync();

            var random = new Random();

            var activitiesRequested =
                JsonSerializer.Deserialize<List<string>>(request.ActivitiesJson)
                ?? new List<string>();

            // =========================
            // FILTER ACTIVITIES
            // =========================
            var activities = allPlaces
                .Where(p =>
                    p.CategoryId == 1 &&
                    p.ActivityType != null &&
                    activitiesRequested.Any(a =>
                        a.ToLower() == p.ActivityType.Name.ToLower()))
                .ToList();

            // =========================
            // RESTAURANTS
            // =========================
            var restaurants = allPlaces
                .Where(p => p.CategoryId == 2)
                .ToList();

            // =========================
            // HOTELS
            // =========================
            var hotels = allPlaces
                .Where(p => p.CategoryId == 3)
                .ToList();

            // =========================
            // INCLUDE SAVED PLACES
            // =========================
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

            // =========================
            // RANDOM HOTEL
            // =========================
            var stay = hotels
                .OrderBy(x => random.Next())
                .FirstOrDefault();

            // =========================
            // CALCULATE DAYS
            // =========================
            var totalDays = (request.EndDate - request.StartDate).Days + 1;
            if (totalDays <= 0) totalDays = 1;

            // =========================
            // GROUP BY REGION
            // =========================
            var groupedByRegion = activities
                .GroupBy(p =>
                {
                    var loc = p.Location.ToLower();

                    if (loc.Contains("beirut")) return "Beirut";
                    if (loc.Contains("tripoli") || loc.Contains("north")) return "North";
                    if (loc.Contains("byblos") || loc.Contains("jbeil")) return "Mount Lebanon";
                    if (loc.Contains("baalbek") || loc.Contains("bekaa")) return "Bekaa";

                    return "Other";
                })
                .ToList();

            // =========================
            // FALLBACK IF NO DATA
            // =========================
            if (groupedByRegion.Count == 0)
            {
                var aiSuggestions = await _recommendationService.GetItineraryRecommendation(
                    "Lebanon",
                    request.TripType,
                    (int)request.BudgetPerDay,
                    request.Travelers.ToString()
                );

                // ✅ SAVE EVEN IN FALLBACK
                request.Status = "Active";
                request.ItineraryJson = JsonSerializer.Serialize(aiSuggestions);

                _context.Set<SmartItineraryRequest>().Add(request);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    stay = (object)null,
                    itinerary = aiSuggestions
                });
            }

            // =========================
            // BUILD ITINERARY
            // =========================
            var itinerary = new List<object>();

            // 🔥 Track used places (CRITICAL FIX)
            var usedPlaceIds = new HashSet<int>();

            for (int day = 1; day <= totalDays; day++)
            {
                var regionGroup = groupedByRegion
                    .ElementAtOrDefault((day - 1) % groupedByRegion.Count);

                List<object> dayActivities = new List<object>();

                if (regionGroup != null)
                {
                    // REMOVE USED
                    var availableActivities = regionGroup
                        .Where(p => !usedPlaceIds.Contains(p.Id))
                        .ToList();

                    // If empty → reuse but reshuffle
                    if (!availableActivities.Any())
                    {
                        availableActivities = regionGroup.ToList();
                    }

                    var selectedActivities = availableActivities
                        .OrderBy(x => random.Next())
                        .Take(2)
                        .ToList();

                    foreach (var p in selectedActivities)
                    {
                        usedPlaceIds.Add(p.Id);
                    }

                    dayActivities = selectedActivities.Select(p => (object)new
                    {
                        id = p.Id,
                        name = p.Name,
                        location = p.Location,
                        imageUrl = p.ImageUrl,
                        activityType = p.ActivityType?.Name
                    }).ToList();
                }

                // =========================
                // RESTAURANT SELECTION
                // =========================
                var availableRestaurants = restaurants
                    .Where(r => !usedPlaceIds.Contains(r.Id))
                    .ToList();

                if (!availableRestaurants.Any())
                {
                    availableRestaurants = restaurants;
                }

                var restaurant = availableRestaurants
                    .OrderBy(x => random.Next())
                    .FirstOrDefault();

                if (restaurant != null)
                {
                    usedPlaceIds.Add(restaurant.Id);
                }

                // =========================
                // AI SUGGESTIONS
                // =========================
                var aiSuggestions = await _recommendationService.GetItineraryRecommendation(
                    regionGroup?.Key ?? "Lebanon",
                    request.TripType,
                    (int)request.BudgetPerDay,
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
                    aiRecommendations = aiSuggestions
                });
            }

            // =========================
            // SAVE TRIP
            // =========================
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
        // GET TRIPS
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _context.Set<SmartItineraryRequest>()
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            foreach (var trip in trips)
            {
                if (trip.Status == "Active" && trip.EndDate < DateTime.UtcNow)
                {
                    trip.Status = "Completed";
                }
            }

            await _context.SaveChangesAsync();

            return Ok(trips.Select(trip => new
            {
                trip.Id,
                trip.UserId,
                userName = trip.User.FirstName + " " + trip.User.LastName,
                trip.Travelers,
                trip.StartDate,
                trip.EndDate,
                trip.BudgetPerDay,
                trip.TripType,
                trip.Transport,
                trip.Status,
                trip.CreatedAt,
                trip.ItineraryJson
            }));
        }

        // =========================
        // DELETE TRIP
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var trip = await _context.Set<SmartItineraryRequest>().FindAsync(id);

            if (trip == null)
                return NotFound("Trip not found");

            _context.Remove(trip);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Trip deleted successfully" });
        }

        // =========================
        // CANCEL TRIP
        // =========================
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelTrip(int id)
        {
            var trip = await _context.Set<SmartItineraryRequest>().FindAsync(id);

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