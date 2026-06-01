using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Senior2.Api.Data;
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
        private readonly string _openAiModel;
        private readonly string _googleApiKey;

        public RecommendationService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _httpClient = new HttpClient();

            _openAiApiKey = config["OpenAI:ApiKey"] ?? string.Empty;
            _openAiModel = config["OpenAI:Model"] ?? "gpt-4o-mini";
            _googleApiKey = config["Google:ApiKey"] ?? string.Empty;

            Console.WriteLine($"OpenAI key loaded: {!string.IsNullOrWhiteSpace(_openAiApiKey)}");
            Console.WriteLine($"Google key loaded: {!string.IsNullOrWhiteSpace(_googleApiKey)}");
        }

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
                try
                {
                    var prefs = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(
                        pref.PreferencesJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    if (prefs != null)
                    {
                        activityPrefs = GetPreferenceList(prefs, "activities", "activity", "activityPrefs", "selectedActivities");
                        interestPrefs = GetPreferenceList(prefs, "interests", "interest", "interestPrefs", "selectedInterests");
                        foodPrefs = GetPreferenceList(prefs, "food", "foods", "foodPrefs", "cuisine", "cuisines");
                    }
                }
                catch
                {
                    activityPrefs = new List<string>();
                    interestPrefs = new List<string>();
                    foodPrefs = new List<string>();
                }
            }

            Console.WriteLine("===== USER RECOMMENDATION DEBUG =====");
            Console.WriteLine($"User ID: {userId}");
            Console.WriteLine($"Raw PreferencesJson: {pref?.PreferencesJson}");
            Console.WriteLine($"Activities: {string.Join(", ", activityPrefs)}");
            Console.WriteLine($"Interests: {string.Join(", ", interestPrefs)}");
            Console.WriteLine($"Food: {string.Join(", ", foodPrefs)}");
            Console.WriteLine("=====================================");

            result.Add(new
            {
                title = "Explore Lebanon",
                places = Personalize(await GetGooglePlaces("top tourist attractions in Lebanon"), userId, "Explore Lebanon")
            });

            if (HasPreference(activityPrefs, "beach", "beaches", "sea", "swim", "summer", "coast", "coastal"))
            {
                result.Add(new
                {
                    title = "Beach Escapes",
                    places = Personalize(await GetGooglePlaces("best beaches in Lebanon"), userId, "Beach Escapes")
                });

                result.Add(new
                {
                    title = "Coastal Restaurants",
                    places = Personalize(await GetGooglePlaces("best seafood restaurants near beaches in Lebanon"), userId, "Coastal Restaurants")
                });

                result.Add(new
                {
                    title = "Seaside Stays",
                    places = Personalize(await GetGooglePlaces("beach resorts and seaside guesthouses in Lebanon"), userId, "Seaside Stays")
                });
            }

            if (HasPreference(activityPrefs, "hike", "hiking", "trail", "nature", "mountain", "forest", "adventure"))
            {
                result.Add(new
                {
                    title = "Hiking Adventures",
                    places = Personalize(await GetGooglePlaces("best hiking trails in Lebanon"), userId, "Hiking Adventures")
                });

                result.Add(new
                {
                    title = "Mountain Villages",
                    places = Personalize(await GetGooglePlaces("beautiful mountain villages in Lebanon"), userId, "Mountain Villages")
                });

                result.Add(new
                {
                    title = "Nature Guesthouses",
                    places = Personalize(await GetGooglePlaces("guesthouses near nature reserves in Lebanon"), userId, "Nature Guesthouses")
                });
            }

            if (HasPreference(activityPrefs, "ski", "snow", "winter", "skiing"))
            {
                result.Add(new
                {
                    title = "Skiing Adventures",
                    places = Personalize(await GetGooglePlaces("best ski resorts in Lebanon"), userId, "Skiing Adventures")
                });

                result.Add(new
                {
                    title = "Mountain Stays",
                    places = Personalize(await GetGooglePlaces("chalets and mountain hotels in Lebanon"), userId, "Mountain Stays")
                });

                result.Add(new
                {
                    title = "Cozy Winter Restaurants",
                    places = Personalize(await GetGooglePlaces("cozy restaurants in Faraya Mzaar Lebanon"), userId, "Cozy Winter Restaurants")
                });
            }

            if (HasPreference(interestPrefs, "history", "historical", "heritage", "ancient", "old", "archaeology"))
            {
                result.Add(new
                {
                    title = "Historical Treasures",
                    places = Personalize(await GetGooglePlaces("historical sites in Lebanon"), userId, "Historical Treasures")
                });

                result.Add(new
                {
                    title = "Museums",
                    places = Personalize(await GetGooglePlaces("best museums in Lebanon"), userId, "Museums")
                });

                result.Add(new
                {
                    title = "Old Souks",
                    places = Personalize(await GetGooglePlaces("old souks and heritage places in Lebanon"), userId, "Old Souks")
                });
            }

            if (HasPreference(interestPrefs, "culture", "cultural", "art", "local", "traditional", "village", "heritage"))
            {
                result.Add(new
                {
                    title = "Cultural Highlights",
                    places = Personalize(await GetGooglePlaces("cultural landmarks in Lebanon"), userId, "Cultural Highlights")
                });

                result.Add(new
                {
                    title = "Art & Local Experiences",
                    places = Personalize(await GetGooglePlaces("art galleries and cultural experiences in Beirut Lebanon"), userId, "Art & Local Experiences")
                });

                result.Add(new
                {
                    title = "Traditional Villages",
                    places = Personalize(await GetGooglePlaces("traditional Lebanese villages to visit"), userId, "Traditional Villages")
                });
            }

            if (HasPreference(interestPrefs, "night", "nightlife", "party", "bar", "music", "club", "rooftop"))
            {
                result.Add(new
                {
                    title = "Nightlife Hotspots",
                    places = Personalize(await GetGooglePlaces("best nightlife places in Lebanon"), userId, "Nightlife Hotspots")
                });

                result.Add(new
                {
                    title = "Rooftop Places",
                    places = Personalize(await GetGooglePlaces("best rooftop lounges in Beirut Lebanon"), userId, "Rooftop Places")
                });

                result.Add(new
                {
                    title = "Live Music Spots",
                    places = Personalize(await GetGooglePlaces("live music places in Lebanon"), userId, "Live Music Spots")
                });
            }

            if (HasPreference(foodPrefs, "lebanese", "traditional", "restaurant", "food", "mezze", "grill", "dessert", "coffee"))
            {
                result.Add(new
                {
                    title = "Lebanese Food Picks",
                    places = Personalize(await GetGooglePlaces("best Lebanese restaurants in Lebanon"), userId, "Lebanese Food Picks")
                });

                result.Add(new
                {
                    title = "Dessert & Coffee Spots",
                    places = Personalize(await GetGooglePlaces("best Lebanese desserts and coffee shops in Lebanon"), userId, "Dessert & Coffee Spots")
                });

                result.Add(new
                {
                    title = "Traditional Food Experiences",
                    places = Personalize(await GetGooglePlaces("traditional Lebanese food experiences in Lebanon"), userId, "Traditional Food Experiences")
                });
            }

            if (result.Count == 1)
            {
                result.Add(new
                {
                    title = "Popular Places",
                    places = Personalize(await GetGooglePlaces("popular tourist places in Lebanon"), userId, "Popular Places")
                });

                result.Add(new
                {
                    title = "Popular Restaurants",
                    places = Personalize(await GetGooglePlaces("best restaurants in Lebanon"), userId, "Popular Restaurants")
                });
            }

            return result;
        }

        private List<string> GetPreferenceList(Dictionary<string, List<string>> prefs, params string[] possibleKeys)
        {
            foreach (var key in possibleKeys)
            {
                var match = prefs.FirstOrDefault(p =>
                    string.Equals(p.Key, key, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(match.Key) && match.Value != null)
                    return match.Value;
            }

            return new List<string>();
        }

        private bool HasPreference(List<string> prefs, params string[] keywords)
        {
            return prefs.Any(pref =>
                keywords.Any(keyword =>
                    pref.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                )
            );
        }

        private List<ItineraryGooglePlaceResult> Personalize(
            List<ItineraryGooglePlaceResult> places,
            int userId,
            string sectionTitle)
        {
            return places
                .Where(p => !string.IsNullOrWhiteSpace(p.Name))
                .GroupBy(p => p.Name.Trim().ToLower())
                .Select(g => g.First())
                .OrderBy(p => GetPersonalSortValue(p.Name + sectionTitle, userId))
                .ToList();
        }

        private int GetPersonalSortValue(string text, int userId)
        {
            return Math.Abs((text + userId).GetHashCode());
        }

        public async Task<List<ItineraryGooglePlaceResult>> GetGooglePlaces(string query, int maxResults = 8)
        {
            var places = new List<ItineraryGooglePlaceResult>();

            try
            {
                if (string.IsNullOrWhiteSpace(_googleApiKey))
                    return places;

                var url =
                    $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={Uri.EscapeDataString(query)}&key={_googleApiKey}";

                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                using var response = await _httpClient.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return places;

                using var doc = JsonDocument.Parse(json);

                if (!doc.RootElement.TryGetProperty("status", out var statusElement) ||
                    statusElement.GetString() != "OK")
                {
                    return places;
                }

                if (!doc.RootElement.TryGetProperty("results", out var resultsElement))
                    return places;

                return ParseGooglePlaces(resultsElement, maxResults);
            }
            catch
            {
                return places;
            }
        }

        public async Task<List<ItineraryGooglePlaceResult>> GetNearbyGooglePlaces(
            double lat,
            double lng,
            string keyword,
            string? type = null,
            int radius = 5000,
            int maxResults = 8)
        {
            var places = new List<ItineraryGooglePlaceResult>();

            try
            {
                if (string.IsNullOrWhiteSpace(_googleApiKey))
                    return places;

                var url =
                    $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={lat},{lng}&radius={radius}&keyword={Uri.EscapeDataString(keyword)}&key={_googleApiKey}";

                if (!string.IsNullOrWhiteSpace(type))
                {
                    url += $"&type={Uri.EscapeDataString(type)}";
                }

                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                using var response = await _httpClient.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return places;

                using var doc = JsonDocument.Parse(json);

                if (!doc.RootElement.TryGetProperty("status", out var statusElement) ||
                    statusElement.GetString() != "OK")
                {
                    return places;
                }

                if (!doc.RootElement.TryGetProperty("results", out var resultsElement))
                    return places;

                return ParseGooglePlaces(resultsElement, maxResults);
            }
            catch
            {
                return places;
            }
        }

        public async Task<List<ItineraryGooglePlaceResult>> GetGooglePlacesForItinerary(
            string region,
            string activityType,
            string tripType,
            int maxResults = 8)
        {
            var query = $"{activityType} {tripType} places in {region} Lebanon";
            return await GetGooglePlaces(query, maxResults);
        }

        public async Task<List<ItineraryGooglePlaceResult>> GetGoogleRestaurantsForItinerary(
            string region,
            decimal budget,
            int maxResults = 8)
        {
            string budgetKeyword;

            if (budget <= 30)
                budgetKeyword = "affordable";
            else if (budget <= 100)
                budgetKeyword = "good";
            else
                budgetKeyword = "best";

            var query = $"{budgetKeyword} restaurants in {region} Lebanon";
            return await GetGooglePlaces(query, maxResults);
        }

        public async Task<List<ItineraryGooglePlaceResult>> GetGoogleRestaurantsNearPlace(
            double lat,
            double lng,
            decimal budget,
            int maxResults = 8)
        {
            string budgetKeyword;

            if (budget <= 30)
                budgetKeyword = "affordable restaurant";
            else if (budget <= 100)
                budgetKeyword = "good restaurant";
            else
                budgetKeyword = "best restaurant";

            return await GetNearbyGooglePlaces(
                lat,
                lng,
                budgetKeyword,
                "restaurant",
                6000,
                maxResults
            );
        }

        public async Task<List<ItineraryGooglePlaceResult>> GetGoogleStaysForItinerary(
            string region,
            decimal budget,
            int maxResults = 8)
        {
            string budgetKeyword;

            if (budget <= 50)
                budgetKeyword = "affordable guesthouse hotel";
            else if (budget <= 150)
                budgetKeyword = "guesthouse hotel";
            else
                budgetKeyword = "luxury hotel resort";

            var query = $"{budgetKeyword} in {region} Lebanon";
            return await GetGooglePlaces(query, maxResults);
        }

        public async Task<List<ItineraryGooglePlaceResult>> GetGoogleStaysNearPlace(
            double lat,
            double lng,
            decimal budget,
            int maxResults = 8)
        {
            string budgetKeyword;

            if (budget <= 50)
                budgetKeyword = "affordable guesthouse hotel";
            else if (budget <= 150)
                budgetKeyword = "guesthouse hotel";
            else
                budgetKeyword = "luxury hotel resort";

            return await GetNearbyGooglePlaces(
                lat,
                lng,
                budgetKeyword,
                "lodging",
                10000,
                maxResults
            );
        }

        public async Task<List<ItineraryGooglePlaceResult>> GetGoogleRestaurantsNearActivity(
            string activityName,
            string activityLocation,
            string region,
            decimal budget,
            int maxResults = 8)
        {
            string budgetKeyword;

            if (budget <= 30)
                budgetKeyword = "affordable restaurants";
            else if (budget <= 100)
                budgetKeyword = "good restaurants";
            else
                budgetKeyword = "best restaurants";

            var query = $"{budgetKeyword} near {activityName} {activityLocation} Lebanon";

            var results = await GetGooglePlaces(query, maxResults);

            if (results.Any())
                return results;

            query = $"{budgetKeyword} in {region} Lebanon";

            results = await GetGooglePlaces(query, maxResults);

            if (results.Any())
                return results;

            return await GetGooglePlaces("best restaurants in Lebanon", maxResults);
        }

        public async Task<List<ItineraryGooglePlaceResult>> GetGoogleStaysNearActivity(
            string activityName,
            string activityLocation,
            string region,
            decimal budget,
            int maxResults = 8)
        {
            string budgetKeyword;

            if (budget <= 50)
                budgetKeyword = "affordable guesthouse hotel";
            else if (budget <= 150)
                budgetKeyword = "guesthouse hotel";
            else
                budgetKeyword = "luxury hotel resort";

            var query = $"{budgetKeyword} near {activityName} {activityLocation} Lebanon";

            var results = await GetGooglePlaces(query, maxResults);

            if (results.Any())
                return results;

            query = $"{budgetKeyword} in {region} Lebanon";

            results = await GetGooglePlaces(query, maxResults);

            if (results.Any())
                return results;

            return await GetGooglePlaces("hotels and guesthouses in Lebanon", maxResults);
        }

        public async Task<List<object>> GetItineraryRecommendation(
            string region,
            string tripType,
            decimal budget,
            string travelerType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_openAiApiKey))
                    return new List<object>();

                var prompt = $@"Suggest 3 short travel tips for a {tripType} trip in {region}, Lebanon for {travelerType}
with a budget of ${budget}/day.
Return ONLY a JSON array of objects with fields: name, location, category, description.";

                var requestBody = new
                {
                    model = _openAiModel,
                    messages = new[]
                    {
                        new
                        {
                            role = "system",
                            content = "You are a Lebanon travel itinerary assistant. Respond ONLY with valid JSON."
                        },
                        new
                        {
                            role = "user",
                            content = prompt
                        }
                    }
                };

                using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://api.openai.com/v1/chat/completions"
                );

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _openAiApiKey);

                request.Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                using var response = await _httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new List<object>();

                using var jsonDoc = JsonDocument.Parse(result);

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
                        content = content
                            .Substring(firstNewline + 1, lastFence - firstNewline - 1)
                            .Trim();
                    }
                }

                if (string.IsNullOrEmpty(content) || !content.StartsWith("["))
                    return new List<object>();

                var places = JsonSerializer.Deserialize<List<RecommendedPlace>>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

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
            catch
            {
                return new List<object>();
            }
        }

        private List<ItineraryGooglePlaceResult> ParseGooglePlaces(JsonElement resultsElement, int maxResults)
        {
            var places = new List<ItineraryGooglePlaceResult>();

            foreach (var place in resultsElement.EnumerateArray().Take(maxResults))
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

                string address = "Lebanon";

                if (place.TryGetProperty("formatted_address", out var addressEl))
                {
                    address = addressEl.GetString() ?? "Lebanon";
                }
                else if (place.TryGetProperty("vicinity", out var vicinityEl))
                {
                    address = vicinityEl.GetString() ?? "Lebanon";
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

                double? rating = null;

                if (place.TryGetProperty("rating", out var ratingEl) &&
                    ratingEl.ValueKind == JsonValueKind.Number)
                {
                    rating = ratingEl.GetDouble();
                }

                places.Add(new ItineraryGooglePlaceResult
                {
                    Id = 0,
                    Name = name,
                    ImageUrl = imageUrl,
                    City = address,
                    Location = address,
                    Lat = lat,
                    Lng = lng,
                    Rating = rating,
                    Source = "Google"
                });
            }

            return places;
        }
    }

    public class ItineraryGooglePlaceResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = "/images/default-place.jpg";
        public string City { get; set; } = "Lebanon";
        public string Location { get; set; } = "Lebanon";
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public double? Rating { get; set; }
        public string Source { get; set; } = "Google";
        public string Category { get; set; } = "Place";
        public string Description { get; set; } = string.Empty;
    }

    public class RecommendedPlace
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}