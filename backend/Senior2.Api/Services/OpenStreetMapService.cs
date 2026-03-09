using System.Text.Json;
using Senior2.Api.Models;

namespace Senior2.Api.Services
{
    public class OpenStreetMapService
    {
        private readonly HttpClient _http;

        public OpenStreetMapService(HttpClient http)
        {
            _http = http;
        }
        public async Task<List<PlaceResult>> SearchPlaces(string query, int limit)
        {
            var url = $"https://nominatim.openstreetmap.org/search?q={query}&countrycodes=lb&format=json&limit={limit}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("User-Agent", "Senior2TourismApp/1.0 (contact@example.com)");

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return new List<PlaceResult>();

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<List<NominatimResult>>(json) ?? new List<NominatimResult>();

            return data.Select(p => new PlaceResult
            {
                Name = p.display_name ?? "",
                Type = "Place",
                City = p.display_name ?? ""
            }).ToList();
        }
        private class NominatimResult
        {
            public string? display_name { get; set; }
        }
    }
}