using System.Text.Json;

namespace Senior2.Api.Services;

public class WeatherService
{
    private readonly HttpClient _http;

    public WeatherService(HttpClient http)
    {
        _http = http;
    }

    public async Task<object> GetWeather(string city)
    {
        var apiKey = "92272defb03d4946b04232106261703";

        var url = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}";

        var response = await _http.GetAsync(url);

        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return new { error = "Weather API failed", raw = json };
        }

        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;

        return new
        {
            temp = root.GetProperty("current").GetProperty("temp_c").GetDouble(),
            condition = root.GetProperty("current").GetProperty("condition").GetProperty("text").GetString(),
            icon = root.GetProperty("current").GetProperty("condition").GetProperty("icon").GetString()
        };
    }
}