using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace AspireAndDaprVerify.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly DaprClient _daprClient;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var value = await _daprClient.GetStateAsync<IEnumerable<WeatherForecast>>("statestore", "weather1");
        if (value != null) return value;
        else
        {
            var weather = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
            await _daprClient.SaveStateAsync("statestore", "weather1", weather);
            return weather;
        }
    }
}
