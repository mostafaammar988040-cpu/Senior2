using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Senior2.Api.Data;
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
        private readonly string _googleApiKey;

        public RecommendationService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _httpClient = new HttpClient();
            _openAiApiKey = config["OpenAi:ApiKey"];
            _googleApiKey = config["Google:ApiKey"];
        }

        // ================================
        // 🌍 MAIN RECOMMENDATIONS - GOOGLE ONLY
        // ================================
        public async Task<List<object>> GetRecommendations(int userId)
        {
            var result = new List<object>();

            var pref = await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            List<string> activityPrefs = new();
            List<string> interestPrefs = new();
            List<string> foodPrefs = new();

            if (pref != null && !string.IsNullOrWhiteSpace(pref.PreferencesJson))
            {
                var prefs = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(pref.PreferencesJson);

                if (prefs != null)
                {
                    if (prefs.ContainsKey("activities") && prefs["activities"] != null)
                        activityPrefs = prefs["activities"];

                    if (prefs.ContainsKey("interests") && prefs["interests"] != null)
                        interestPrefs = prefs["interests"];

                    if (prefs.ContainsKey("food") && prefs["food"] != null)
                        foodPrefs = prefs["food"];
                }
            }

            // Always include one general Lebanon section
            var exploreLebanon = await GetGooglePlaces("top tourist attractions in Lebanon");
            result.Add(new
            {
                title = "Explore Lebanon",
                places = exploreLebanon
            });

            // Food preferences
            if (foodPrefs.Any(f => f.Contains("lebanese", StringComparison.OrdinalIgnoreCase)))
            {
                var lebaneseFood = await GetGooglePlaces("best lebanese restaurants in Lebanon");
                result.Add(new
                {
                    title = "Lebanese Food Picks",
                    places = lebaneseFood
                });
            }

            // Activity preferences
            if (activityPrefs.Any(a => a.Contains("hike", StringComparison.OrdinalIgnoreCase)))
            {
                var hiking = await GetGooglePlaces("best hiking places in Lebanon");
                result.Add(new
                {
                    title = "Hiking Adventures",
                    places = hiking
                });
            }

            if (activityPrefs.Any(a => a.Contains("beach", StringComparison.OrdinalIgnoreCase) ||
                                       a.Contains("swim", StringComparison.OrdinalIgnoreCase)))
            {
                var beaches = await GetGooglePlaces("best beaches in Lebanon");
                result.Add(new
                {
                    title = "Beach Escapes",
                    places = beaches
                });
            }

            if (activityPrefs.Any(a => a.Contains("ski", StringComparison.OrdinalIgnoreCase)))
            {
                var skiing = await GetGooglePlaces("best ski resorts in Lebanon");
                result.Add(new
                {
                    title = "Skiing Adventures",
                    places = skiing
                });
            }

            // Interest preferences
            if (interestPrefs.Any(i => i.Contains("histor", StringComparison.OrdinalIgnoreCase)))
            {
                var historical = await GetGooglePlaces("historical sites in Lebanon");
                result.Add(new
                {
                    title = "Historical Treasures",
                    places = historical
                });
            }

            if (interestPrefs.Any(i => i.Contains("cultur", StringComparison.OrdinalIgnoreCase)))
            {
                var cultural = await GetGooglePlaces("cultural landmarks in Lebanon");
                result.Add(new
                {
                    title = "Cultural Highlights",
                    places = cultural
                });
            }

            if (interestPrefs.Any(i => i.Contains("night", StringComparison.OrdinalIgnoreCase)))
            {
                var nightlife = await GetGooglePlaces("best nightlife places in Lebanon");
                result.Add(new
                {
                    title = "Nightlife Hotspots",
                    places = nightlife
                });
            }

            // Fallback if user has no preferences
            if (result.Count == 1)
            {
                var restaurants = await GetGooglePlaces("best restaurants in Lebanon");
                result.Add(new
                {
                    title = "Popular Restaurants",
                    places = restaurants
                });

                var nature = await GetGooglePlaces("beautiful nature places in Lebanon");
                result.Add(new
                {
                    title = "Nature Spots",
                    places = nature
                });
            }

            return result;
        }

        // ================================
        // 🌍 GOOGLE PLACES METHOD
        // ================================
        private async Task<List<object>> GetGooglePlaces(string query)
        {
            var url =
                $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={Uri.EscapeDataString(query)}&key={_googleApiKey}";

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new List<object>();
            }

            var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("status", out var statusElement) ||
                statusElement.GetString() != "OK")
            {
                return new List<object>();
            }

            if (!doc.RootElement.TryGetProperty("results", out var resultsElement))
            {
                return new List<object>();
            }

            var places = new List<object>();

            foreach (var place in resultsElement.EnumerateArray().Take(8))
            {
                string name = place.TryGetProperty("name", out var nameEl)
                    ? nameEl.GetString() ?? "Unknown Place"
                    : "Unknown Place";

                string imageUrl = "/images/default-place.jpg";

                if (place.TryGetProperty("photos", out var photosEl) &&
                    photosEl.ValueKind == JsonValueKind.Array &&
                    photosEl.GetArrayLength() > 0)
                {
                    var firstPhoto = photosEl[0];

                    if (firstPhoto.TryGetProperty("photo_reference", out var photoRefEl))
                    {
                        var photoReference = photoRefEl.GetString();

                        if (!string.IsNullOrWhiteSpace(photoReference))
                        {
                            imageUrl =
                                $"https://maps.googleapis.com/maps/api/place/photo?maxwidth=800&photo_reference={photoReference}&key={_googleApiKey}";
                        }
                    }
                }

                string city = "Lebanon";
                if (place.TryGetProperty("formatted_address", out var addressEl))
                {
                    city = addressEl.GetString() ?? "Lebanon";
                }

                double? lat = null;
                double? lng = null;

                if (place.TryGetProperty("geometry", out var geometryEl) &&
                    geometryEl.TryGetProperty("location", out var locationEl))
                {
                    if (locationEl.TryGetProperty("lat", out var latEl))
                        lat = latEl.GetDouble();

                    if (locationEl.TryGetProperty("lng", out var lngEl))
                        lng = lngEl.GetDouble();
                }

                places.Add(new
                {
                    id = 0,
                    name,
                    imageUrl,
                    city,
                    lat,
                    lng
                });
            }

            return places;
        }

        // ================================
        // 🤖 ITINERARY RECOMMENDATION (UNCHANGED)
        // ================================
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

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _openAiApiKey);

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

            content = content?.Trim();

            if (!string.IsNullOrEmpty(content) && content.StartsWith("```"))
            {
                var firstNewline = content.IndexOf('\n');
                var lastFence = content.LastIndexOf("```");
                if (firstNewline >= 0 && lastFence > firstNewline)
                {
                    content = content.Substring(firstNewline + 1, lastFence - firstNewline - 1).Trim();
                }
            }

            if (string.IsNullOrEmpty(content) || !(content.StartsWith("{") || content.StartsWith("[")))
            {
                return new List<object> { new { error = "Invalid AI response", raw = content } };
            }

            var places = JsonSerializer.Deserialize<List<RecommendedPlace>>(content);

            return places?.Select(p => new
            {
                id = 0,
                name = p.Name,
                imageUrl = "/images/default-place.jpg",
                city = p.Location,
                category = p.Category,
                description = p.Description
            }).Cast<object>().ToList() ?? new List<object>();
        }
    }

    public class RecommendedPlace
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}