using System.Net.Http;
using System.Text.Json;

namespace Senior2.Api.Services;

public class GooglePlacesService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GooglePlacesService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["Google:ApiKey"];
    }

    public async Task<List<GooglePlaceResult>> SearchAsync(string query)
    {
        var url = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={query}&key={_apiKey}";

        var response = await _httpClient.GetStringAsync(url);

        var json = JsonDocument.Parse(response);

        var results = new List<GooglePlaceResult>();

        foreach (var item in json.RootElement.GetProperty("results").EnumerateArray())
        {
            results.Add(new GooglePlaceResult
            {
                Name = item.GetProperty("name").GetString(),
                Address = item.GetProperty("formatted_address").GetString(),
                Types = item.GetProperty("types").EnumerateArray().Select(t => t.GetString()).ToList()
            });
        }

        return results;
    }
}

public class GooglePlaceResult
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public List<string?> Types { get; set; } = new();
}