using System.Text.Json;

namespace Senior2.Api.Services;

public class WeatherService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public WeatherService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<object> GetWeather(string location)
    {
        try
        {
            var apiKey = _config["AccuWeather:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return new
                {
                    success = false,
                    error = "AccuWeather API key is missing."
                };
            }

            var cleanLocation = CleanLocation(location);

            // Step 1: Search location to get AccuWeather Location Key
            var searchUrl =
                $"https://dataservice.accuweather.com/locations/v1/cities/search?apikey={apiKey}&q={Uri.EscapeDataString(cleanLocation)}&details=false";

            var searchResponse = await _http.GetAsync(searchUrl);
            var searchJson = await searchResponse.Content.ReadAsStringAsync();

            if (!searchResponse.IsSuccessStatusCode)
            {
                return new
                {
                    success = false,
                    error = "AccuWeather location search failed.",
                    searchedLocation = cleanLocation,
                    raw = searchJson
                };
            }

            using var searchDoc = JsonDocument.Parse(searchJson);
            var searchRoot = searchDoc.RootElement;

            if (searchRoot.ValueKind != JsonValueKind.Array || searchRoot.GetArrayLength() == 0)
            {
                return new
                {
                    success = false,
                    error = "Location not found.",
                    searchedLocation = cleanLocation
                };
            }

            var firstLocation = searchRoot[0];

            var locationKey = firstLocation.GetProperty("Key").GetString();

            var cityName = firstLocation.TryGetProperty("LocalizedName", out var nameProp)
                ? nameProp.GetString()
                : cleanLocation;

            var countryName = "";

            if (firstLocation.TryGetProperty("Country", out var countryProp) &&
                countryProp.TryGetProperty("LocalizedName", out var countryNameProp))
            {
                countryName = countryNameProp.GetString() ?? "";
            }

            // Step 2: Get current weather using Location Key
            var weatherUrl =
                $"https://dataservice.accuweather.com/currentconditions/v1/{locationKey}?apikey={apiKey}&details=true";

            var weatherResponse = await _http.GetAsync(weatherUrl);
            var weatherJson = await weatherResponse.Content.ReadAsStringAsync();

            if (!weatherResponse.IsSuccessStatusCode)
            {
                return new
                {
                    success = false,
                    error = "AccuWeather current weather failed.",
                    raw = weatherJson
                };
            }

            using var weatherDoc = JsonDocument.Parse(weatherJson);
            var weatherRoot = weatherDoc.RootElement;

            if (weatherRoot.ValueKind != JsonValueKind.Array || weatherRoot.GetArrayLength() == 0)
            {
                return new
                {
                    success = false,
                    error = "Weather data not found.",
                    raw = weatherJson
                };
            }

            var current = weatherRoot[0];

            var condition = current.TryGetProperty("WeatherText", out var weatherTextProp)
                ? weatherTextProp.GetString()
                : "Unknown";

            var temp = current
                .GetProperty("Temperature")
                .GetProperty("Metric")
                .GetProperty("Value")
                .GetDouble();

            var iconNumber = current.TryGetProperty("WeatherIcon", out var iconProp)
                ? iconProp.GetInt32()
                : 1;

            var humidity = current.TryGetProperty("RelativeHumidity", out var humidityProp)
                ? humidityProp.GetInt32()
                : 0;

            var wind = 0.0;

            if (current.TryGetProperty("Wind", out var windProp) &&
                windProp.TryGetProperty("Speed", out var speedProp) &&
                speedProp.TryGetProperty("Metric", out var metricProp) &&
                metricProp.TryGetProperty("Value", out var windValueProp))
            {
                wind = windValueProp.GetDouble();
            }

            return new
            {
                success = true,
                city = cityName,
                country = countryName,
                temp,
                condition,
                icon = GetAccuWeatherIcon(iconNumber),
                humidity,
                wind
            };
        }
        catch (Exception ex)
        {
            return new
            {
                success = false,
                error = ex.Message
            };
        }
    }

    private string CleanLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            return "Beirut";

        var clean = location.Trim();

        clean = clean.Replace("Lebanon Lebanon", "Lebanon");
        clean = clean.Replace("،", ",");
        clean = clean.Replace("—", "-");

        clean = clean.Replace("Ra’s", "Beirut");
        clean = clean.Replace("Bayrut", "Beirut");

        // If location is a full address, try to extract a useful city name
        var knownCities = new[]
        {
            "Beirut", "Batroun", "Byblos", "Jbeil", "Baalbek",
            "Tyre", "Sour", "Sidon", "Saida", "Tripoli",
            "Jounieh", "Jeita", "Zahle", "Aley", "Bcharre",
            "Cedars", "Kfardebian", "Faraya", "Ehden", "Anjar",
            "Jiyeh", "Chekka", "Mina", "Koura"
        };

        foreach (var city in knownCities)
        {
            if (clean.Contains(city, StringComparison.OrdinalIgnoreCase))
                return city;
        }

        // If no known city found, take the first part before comma
        if (clean.Contains(","))
        {
            var parts = clean.Split(",", StringSplitOptions.RemoveEmptyEntries);
            clean = parts[0].Trim();
        }

        clean = clean.Replace("Lebanon", "", StringComparison.OrdinalIgnoreCase).Trim();

        // If it looks like a plus code/address code, fallback to Beirut
        if (clean.Contains("+") || clean.Length < 3)
            return "Beirut";

        return clean;
    }

    private string GetAccuWeatherIcon(int iconNumber)
    {
        var icon = iconNumber.ToString("D2");
        return $"https://developer.accuweather.com/sites/default/files/{icon}-s.png";
    }
}