using Dapr.Actors;

namespace AspireAndDaprVerify.Actors
{
    public interface ISampleActor:IActor
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecast();
    }
}