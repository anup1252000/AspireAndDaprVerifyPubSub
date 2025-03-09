using Dapr.Actors.Runtime;
using Dapr.Client;

namespace AspireAndDaprVerify.Actors
{
    public class SampleActor : Actor, ISampleActor
    {
        private readonly DaprClient _daprClient;

        public SampleActor(ActorHost host,DaprClient daprClient) : base(host)
        {
            _daprClient = daprClient;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
        {
            var weatherForecast = await _daprClient.
                 GetStateAsync<IEnumerable<WeatherForecast>>("statestore", "weather1");
            if (weatherForecast != null) return weatherForecast;
            return default;
        }
    }
}
