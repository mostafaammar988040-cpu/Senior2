using Microsoft.AspNetCore.Mvc;
using Senior2.Api.Services;

namespace Senior2.Api.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase
{
    private readonly WeatherService _weather;

    public WeatherController(WeatherService weather)
    {
        _weather = weather;
    }

    [HttpGet("{city}")]
    public async Task<IActionResult> Get(string city)
    {
        var result = await _weather.GetWeather(city);
        return Ok(result);
    }
}