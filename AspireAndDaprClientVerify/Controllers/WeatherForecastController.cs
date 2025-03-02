using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace AspireAndDaprClientVerify.Controllers;

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
        var result= await _daprClient.InvokeMethodAsync<IEnumerable< WeatherForecast>>(httpMethod: HttpMethod.Get, "aspireanddaprverify", "weatherforecast");
        await _daprClient.PublishEventAsync("servicebus-pubsub", "ganeshmahadev", result.First());
        return result;
    }
}
