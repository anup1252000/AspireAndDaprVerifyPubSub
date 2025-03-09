using AspireAndDaprVerify.Actors;
using Dapr.Actors.Client;
using Dapr.Actors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspireAndDaprVerify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly ActorProxyFactory actorProxy;

        public ActorController(ActorProxyFactory actorProxy)
        {
            this.actorProxy = actorProxy;
        }

        [HttpGet("{actorId}")]
        public async Task<IEnumerable<WeatherForecast>> Get(string actorId)
        {
            var actorIds=new ActorId(actorId);
            var actor = actorProxy.
                 CreateActorProxy<ISampleActor>(actorIds, "SampleActor");
            var weatherforecast = await actor.GetWeatherForecast();
            await Task.Delay(20000);
            return weatherforecast;
        }
    }
}
