using System.Text.Json;

namespace Senior2.Api.Services
{
    public class GeoapifyService
    {
        private readonly HttpClient _http;

        private readonly string _apiKey = "2b72f726eece4a3cadc79b655335bc7e";

        // PEXELS API KEY
        private readonly string _pexelsKey = "1PSQZG60qCZnV9cq6ac8ic42t9d0enMU7WsZaQJSOnEKjVwwNs1TQggS";

        public GeoapifyService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<dynamic>> GetLebanonPlaces(string category)
        {
            var url =
  $"https://api.geoapify.com/v2/places?categories={category}&filter=circle:35.5018,33.8938,120000&limit=30&apiKey={_apiKey}";

            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<dynamic>();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            var places = new List<dynamic>();

            if (!doc.RootElement.TryGetProperty("features", out var features))
                return places;

            var seen = new HashSet<string>();

            foreach (var item in features.EnumerateArray())
            {
                var prop = item.GetProperty("properties");

                if (prop.TryGetProperty("country_code", out var cc))
                {
                    if (cc.GetString() != "lb")
                        continue;
                }

                double lat = prop.GetProperty("lat").GetDouble();
                double lon = prop.GetProperty("lon").GetDouble();

                if (lat < 33.0 || lat > 34.7 || lon < 35.0 || lon > 36.7)
                    continue;

                string name = prop.TryGetProperty("name", out var n)
                    ? n.GetString()
                    : "Unknown place";

                if (seen.Contains(name))
                    continue;

                seen.Add(name);

                string city = prop.TryGetProperty("city", out var c)
                    ? c.GetString()
                    : "Lebanon";

                // 🔥 Get image
                var image = await GetPlaceImage(name);

                places.Add(new
                {
                    id = Guid.NewGuid(),
                    name = name,
                    city = city,
                    imageUrl = image
                });
            }

            return places.Take(10).ToList();
        }

        // IMAGE RESOLVER
        private async Task<string> GetPlaceImage(string placeName)
        {
            // 1️⃣ WIKIPEDIA
            try
            {
                var searchQuery = Uri.EscapeDataString($"{placeName} Lebanon");

                var wikiUrl =
                    $"https://en.wikipedia.org/w/api.php?action=query&generator=search&gsrsearch={searchQuery}&gsrlimit=1&prop=pageimages&piprop=thumbnail&pithumbsize=600&format=json";

                var response = await _http.GetAsync(wikiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(json);

                    if (doc.RootElement.TryGetProperty("query", out var query) &&
                        query.TryGetProperty("pages", out var pages))
                    {
                        foreach (var page in pages.EnumerateObject())
                        {
                            var value = page.Value;

                            if (value.TryGetProperty("thumbnail", out var thumb) &&
                                thumb.TryGetProperty("source", out var src))
                            {
                                return src.GetString();
                            }
                        }
                    }
                }
            }
            catch { }

            // 2️⃣ WIKIMEDIA
            try
            {
                var commonsQuery = Uri.EscapeDataString($"{placeName} Lebanon");

                var commonsUrl =
                    $"https://commons.wikimedia.org/w/api.php?action=query&generator=search&gsrsearch={commonsQuery}&gsrlimit=1&prop=imageinfo&iiprop=url&format=json";

                var response = await _http.GetAsync(commonsUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(json);

                    if (doc.RootElement.TryGetProperty("query", out var query) &&
                        query.TryGetProperty("pages", out var pages))
                    {
                        foreach (var page in pages.EnumerateObject())
                        {
                            var value = page.Value;

                            if (value.TryGetProperty("imageinfo", out var imgInfo))
                            {
                                var url = imgInfo[0].GetProperty("url").GetString();

                                if (url.EndsWith(".jpg") || url.EndsWith(".jpeg") || url.EndsWith(".png"))
                                {
                                    return url;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            // 3️⃣ PEXELS
            try
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"https://api.pexels.com/v1/search?query={Uri.EscapeDataString(placeName + " Lebanon")}&per_page=1"
                );

                request.Headers.Add("Authorization", _pexelsKey);

                var response = await _http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(json);

                    var photos = doc.RootElement.GetProperty("photos");

                    if (photos.GetArrayLength() > 0)
                    {
                        var image = photos[0]
                            .GetProperty("src")
                            .GetProperty("large")
                            .GetString();

                        return image;
                    }
                }
            }
            catch { }

            // 4️⃣ FALLBACK
            return "/images/fallback-place.jpg";
        }
    }
}