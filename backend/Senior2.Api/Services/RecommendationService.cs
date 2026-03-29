using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Senior2.Api.Data;
using Senior2.Api.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Senior2.Api.Services
{
    public class RecommendationService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _openAiApiKey;

        public RecommendationService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _httpClient = new HttpClient();
            _openAiApiKey = config["OpenAi:ApiKey"]; // stored in appsettings.json
        }

        // Existing method for navigation bar recommendations
        public async Task<List<object>> GetRecommendations(int userId)
        {
            var result = new List<object>();

            // 1️⃣ Get user preferences
            var pref = await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            // 2️⃣ Popular places from your platform
            var dbPlaces = await _context.Places
                .Include(p => p.Category)
                .Take(8)
                .ToListAsync();

            result.Add(new
            {
                title = "Popular on Platform",
                places = dbPlaces.Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    imageUrl = string.IsNullOrEmpty(p.ImageUrl) ? "/images/default-place.jpg" : p.ImageUrl,
                    city = p.Location
                })
            });

            // 3️⃣ Restaurants from database
            var restaurants = await _context.Places
                .Include(p => p.Category)
                .Where(p => p.Category.Slug == "restaurants")
                .Take(8)
                .ToListAsync();

            result.Add(new
            {
                title = "Best Restaurants For You",
                places = restaurants.Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    imageUrl = p.ImageUrl,
                    city = p.Location
                })
            });

            // 4️⃣ Personalized recommendations based on preferences
            if (pref != null)
            {
                var prefs = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(pref.PreferencesJson);

                if (prefs != null && prefs.ContainsKey("activities"))
                {
                    var activities = prefs["activities"];

                    if (activities.Any(a => a.Contains("hike")))
                    {
                        var hiking = await _context.Places
                            .Include(p => p.ActivityType)
                            .Where(p => p.ActivityType != null && p.ActivityType.Slug == "hiking")
                            .Take(6)
                            .ToListAsync();

                        result.Add(new
                        {
                            title = "Hiking Adventures",
                            places = hiking.Select(p => new
                            {
                                id = p.Id,
                                name = p.Name,
                                imageUrl = p.ImageUrl,
                                city = p.Location
                            })
                        });
                    }

                    if (activities.Any(a => a.Contains("beach")))
                    {
                        var swimming = await _context.Places
                            .Include(p => p.ActivityType)
                            .Where(p => p.ActivityType != null && p.ActivityType.Slug == "swimming")
                            .Take(6)
                            .ToListAsync();

                        result.Add(new
                        {
                            title = "Best Swimming Spots",
                            places = swimming.Select(p => new
                            {
                                id = p.Id,
                                name = p.Name,
                                imageUrl = p.ImageUrl,
                                city = p.Location
                            })
                        });
                    }

                    if (activities.Any(a => a.Contains("ski")))
                    {
                        var skiing = await _context.Places
                            .Include(p => p.ActivityType)
                            .Where(p => p.ActivityType != null && p.ActivityType.Slug == "skiing")
                            .Take(6)
                            .ToListAsync();

                        result.Add(new
                        {
                            title = "Skiing Adventures",
                            places = skiing.Select(p => new
                            {
                                id = p.Id,
                                name = p.Name,
                                imageUrl = p.ImageUrl,
                                city = p.Location
                            })
                        });
                    }
                }
            }

            return result;
        }

        // ✅ Cleaned up method for itinerary recommendations using OpenAI
        public async Task<List<object>> GetItineraryRecommendation(string region, string tripType, decimal budget, string travelerType)
        {
            var prompt = $@"Suggest 5 {tripType} activities in {region} for a {travelerType} traveler 
with a budget of ${budget}/day. 
Return ONLY a JSON array of objects with fields: name, location, category, description.";

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "You are a travel itinerary assistant. Respond ONLY with valid JSON." },
                    new { role = "user", content = prompt }
                }
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiApiKey);

            var response = await _httpClient.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            );

            var result = await response.Content.ReadAsStringAsync();

            var jsonDoc = JsonDocument.Parse(result);
            var content = jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            // 🔒 Safety guard: clean up response
            content = content?.Trim();

            // Remove code fences if present
            if (!string.IsNullOrEmpty(content) && content.StartsWith("```"))
            {
                var firstNewline = content.IndexOf('\n');
                var lastFence = content.LastIndexOf("```");
                if (firstNewline >= 0 && lastFence > firstNewline)
                {
                    content = content.Substring(firstNewline + 1, lastFence - firstNewline - 1).Trim();
                }
            }

            // If still not valid JSON, return raw
            if (string.IsNullOrEmpty(content) || !(content.StartsWith("{") || content.StartsWith("[")))
            {
                return new List<object> { new { error = "Invalid AI response", raw = content } };
            }

            var places = JsonSerializer.Deserialize<List<RecommendedPlace>>(content);

            return places?.Select(p => new
            {
                id = 0, // AI suggestions won’t have DB IDs
                name = p.Name,
                imageUrl = "/images/default-place.jpg",
                city = p.Location,
                category = p.Category,
                description = p.Description
            }).Cast<object>().ToList() ?? new List<object>();
        }
    }

    // ✅ RecommendedPlace model
    public class RecommendedPlace
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}