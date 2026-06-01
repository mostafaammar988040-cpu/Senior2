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

        public SmartItineraryController(
            AppDbContext context,
            RecommendationService recommendationService)
        {
            _context = context;
            _recommendationService = recommendationService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateTrip([FromBody] SmartItineraryRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(errors);
            }

            var totalDays = (request.EndDate - request.StartDate).Days + 1;

            if (totalDays <= 0)
                totalDays = 1;

            var activityTypes = ParseActivities(request.ActivitiesJson);

            if (!activityTypes.Any())
            {
                activityTypes = BuildDefaultActivitiesFromTripType(request.TripType);
            }

            var regions = BuildRegionsForTrip(totalDays);

            var itinerary = new List<object>();

            var usedPlaceNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var usedRestaurantNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            ItineraryGooglePlaceResult? tripStay = null;

            for (int day = 1; day <= totalDays; day++)
            {
                var region = regions[(day - 1) % regions.Count];

                var firstActivityType = activityTypes[(day - 1) % activityTypes.Count];
                var secondActivityType = GetSecondActivityType(firstActivityType, activityTypes, request.TripType, day);

                var dayActivities = new List<object>();

                var firstActivity = await PickGooglePlaceAsync(
                    region,
                    firstActivityType,
                    request.TripType,
                    usedPlaceNames
                );

                if (firstActivity != null)
                {
                    usedPlaceNames.Add(NormalizeName(firstActivity.Name));

                    dayActivities.Add(new
                    {
                        id = firstActivity.Id,
                        name = firstActivity.Name,
                        location = firstActivity.Location,
                        imageUrl = firstActivity.ImageUrl,
                        activityType = firstActivityType,
                        rating = firstActivity.Rating,
                        lat = firstActivity.Lat,
                        lng = firstActivity.Lng,
                        source = firstActivity.Source
                    });
                }

                var secondActivity = await PickGooglePlaceAsync(
                    region,
                    secondActivityType,
                    request.TripType,
                    usedPlaceNames
                );

                if (secondActivity != null)
                {
                    usedPlaceNames.Add(NormalizeName(secondActivity.Name));

                    dayActivities.Add(new
                    {
                        id = secondActivity.Id,
                        name = secondActivity.Name,
                        location = secondActivity.Location,
                        imageUrl = secondActivity.ImageUrl,
                        activityType = secondActivityType,
                        rating = secondActivity.Rating,
                        lat = secondActivity.Lat,
                        lng = secondActivity.Lng,
                        source = secondActivity.Source
                    });
                }

                var anchorPlace = firstActivity ?? secondActivity;

                var restaurant = await PickGoogleRestaurantAsync(
                    region,
                    request.BudgetPerDay,
                    usedRestaurantNames,
                    anchorPlace
                );

                if (restaurant != null)
                {
                    usedRestaurantNames.Add(NormalizeName(restaurant.Name));
                }

                if (tripStay == null)
                {
                    tripStay = await PickGoogleStayAsync(
                        region,
                        request.BudgetPerDay,
                        anchorPlace
                    );
                }

                var aiSuggestions = await _recommendationService.GetItineraryRecommendation(
                    region,
                    request.TripType,
                    request.BudgetPerDay,
                    $"{request.Travelers} traveler(s)"
                );

                itinerary.Add(new
                {
                    day,
                    region,

                    stay = tripStay != null ? new
                    {
                        tripStay.Id,
                        tripStay.Name,
                        tripStay.Location,
                        tripStay.ImageUrl,
                        tripStay.Rating,
                        tripStay.Lat,
                        tripStay.Lng,
                        tripStay.Source
                    } : null,

                    activities = dayActivities,

                    restaurant = restaurant != null ? new
                    {
                        restaurant.Id,
                        restaurant.Name,
                        restaurant.Location,
                        restaurant.ImageUrl,
                        restaurant.Rating,
                        restaurant.Lat,
                        restaurant.Lng,
                        restaurant.Source
                    } : null,

                    aiRecommendations = aiSuggestions
                });
            }

            request.Status = "Active";
            request.ItineraryJson = JsonSerializer.Serialize(itinerary);

            _context.Set<SmartItineraryRequest>().Add(request);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                stay = tripStay,
                itinerary
            });
        }

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
                userName = trip.User != null
                    ? trip.User.FirstName + " " + trip.User.LastName
                    : "Unknown User",
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

        private List<string> ParseActivities(string? activitiesJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(activitiesJson))
                    return new List<string>();

                var activities = JsonSerializer.Deserialize<List<string>>(
                    activitiesJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return activities?
                    .Where(a => !string.IsNullOrWhiteSpace(a))
                    .Select(a => NormalizeActivityName(a))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList() ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        private List<string> BuildDefaultActivitiesFromTripType(string? tripType)
        {
            var type = (tripType ?? string.Empty).ToLower();

            if (type.Contains("adventure"))
            {
                return new List<string>
                {
                    "hiking",
                    "nature attraction",
                    "outdoor activity",
                    "mountain viewpoint"
                };
            }

            if (type.Contains("relax"))
            {
                return new List<string>
                {
                    "beach",
                    "resort",
                    "scenic place",
                    "nature attraction"
                };
            }

            if (type.Contains("culture") || type.Contains("histor"))
            {
                return new List<string>
                {
                    "historical site",
                    "museum",
                    "cultural landmark",
                    "old souk"
                };
            }

            if (type.Contains("family"))
            {
                return new List<string>
                {
                    "family attraction",
                    "park",
                    "museum",
                    "tourist attraction"
                };
            }

            return new List<string>
            {
                "tourist attraction",
                "historical site",
                "nature attraction",
                "cultural landmark"
            };
        }

        private List<string> BuildRegionsForTrip(int totalDays)
        {
            var baseRegions = new List<string>
            {
                "Beirut",
                "Mount Lebanon",
                "North Lebanon",
                "Bekaa",
                "South Lebanon"
            };

            if (totalDays <= baseRegions.Count)
                return baseRegions.Take(totalDays).ToList();

            return baseRegions;
        }

        private string GetSecondActivityType(
            string firstActivityType,
            List<string> activityTypes,
            string tripType,
            int day)
        {
            var differentActivities = activityTypes
                .Where(a => !a.Equals(firstActivityType, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (differentActivities.Any())
                return differentActivities[day % differentActivities.Count];

            var first = firstActivityType.ToLower();

            if (first.Contains("ski"))
                return "mountain viewpoint";

            if (first.Contains("beach") || first.Contains("swim"))
                return "coastal attraction";

            if (first.Contains("hike"))
                return "nature attraction";

            if (first.Contains("histor"))
                return "museum";

            if (first.Contains("museum"))
                return "old souk";

            if ((tripType ?? string.Empty).ToLower().Contains("adventure"))
                return "outdoor attraction";

            if ((tripType ?? string.Empty).ToLower().Contains("relax"))
                return "scenic place";

            return "tourist attraction";
        }

        private async Task<ItineraryGooglePlaceResult?> PickGooglePlaceAsync(
            string region,
            string activityType,
            string tripType,
            HashSet<string> usedNames)
        {
            var places = await _recommendationService.GetGooglePlacesForItinerary(
                region,
                activityType,
                tripType,
                8
            );

            var selected = places
                .Where(p => !string.IsNullOrWhiteSpace(p.Name))
                .Where(p => !usedNames.Contains(NormalizeName(p.Name)))
                .OrderByDescending(p => p.Rating ?? 0)
                .FirstOrDefault();

            if (selected != null)
            {
                selected.Category = activityType;
                selected.Description = $"{activityType} recommended from Google Places.";
                return selected;
            }

            return places
                .Where(p => !string.IsNullOrWhiteSpace(p.Name))
                .OrderByDescending(p => p.Rating ?? 0)
                .FirstOrDefault();
        }

        private async Task<ItineraryGooglePlaceResult?> PickGoogleRestaurantAsync(
     string region,
     decimal budget,
     HashSet<string> usedRestaurantNames,
     ItineraryGooglePlaceResult? anchorPlace)
        {
            List<ItineraryGooglePlaceResult> restaurants;

            if (anchorPlace != null)
            {
                restaurants = await _recommendationService.GetGoogleRestaurantsNearActivity(
                    anchorPlace.Name,
                    anchorPlace.Location,
                    region,
                    budget,
                    8
                );
            }
            else
            {
                restaurants = await _recommendationService.GetGoogleRestaurantsForItinerary(
                    region,
                    budget,
                    8
                );
            }

            var selected = restaurants
                .Where(p => !string.IsNullOrWhiteSpace(p.Name))
                .Where(p => !usedRestaurantNames.Contains(NormalizeName(p.Name)))
                .OrderByDescending(p => p.Rating ?? 0)
                .FirstOrDefault();

            if (selected != null)
            {
                selected.Category = "Restaurant";
                selected.Description = "Restaurant recommended near the selected activity.";
                return selected;
            }

            selected = restaurants
                .Where(p => !string.IsNullOrWhiteSpace(p.Name))
                .OrderByDescending(p => p.Rating ?? 0)
                .FirstOrDefault();

            if (selected != null)
            {
                selected.Category = "Restaurant";
                selected.Description = "Restaurant recommended near the selected activity.";
            }

            return selected;
        }
        private async Task<ItineraryGooglePlaceResult?> PickGoogleStayAsync(
         string region,
         decimal budget,
         ItineraryGooglePlaceResult? anchorPlace)
        {
            List<ItineraryGooglePlaceResult> stays;

            if (anchorPlace != null)
            {
                stays = await _recommendationService.GetGoogleStaysNearActivity(
                    anchorPlace.Name,
                    anchorPlace.Location,
                    region,
                    budget,
                    8
                );
            }
            else
            {
                stays = await _recommendationService.GetGoogleStaysForItinerary(
                    region,
                    budget,
                    8
                );
            }

            var selected = stays
                .Where(p => !string.IsNullOrWhiteSpace(p.Name))
                .OrderByDescending(p => p.Rating ?? 0)
                .FirstOrDefault();

            if (selected != null)
            {
                selected.Category = "Stay";
                selected.Description = "Stay recommended near the selected activity.";
            }

            return selected;
        }

        private string NormalizeName(string? name)
        {
            return (name ?? string.Empty)
                .Trim()
                .ToLower()
                .Replace(" ", "");
        }

        private string NormalizeActivityName(string activity)
        {
            var value = activity.Trim().ToLower();

            if (value.Contains("ski"))
                return "ski resort";

            if (value.Contains("hike"))
                return "hiking";

            if (value.Contains("beach") || value.Contains("swim"))
                return "beach";

            if (value.Contains("histor"))
                return "historical site";

            if (value.Contains("culture"))
                return "cultural landmark";

            if (value.Contains("museum"))
                return "museum";

            if (value.Contains("food"))
                return "food experience";

            return value;
        }
    }
}