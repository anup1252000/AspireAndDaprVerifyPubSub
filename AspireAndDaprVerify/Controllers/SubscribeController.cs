using Dapr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspireAndDaprVerify.Controllers
{
    [Route("subscribe")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        [HttpPost]
        [Topic("azure-servicebus-subscription", "ganeshmahadev")]
        public IActionResult SubscribeToQueue([FromBody] WeatherForecast message)
        {

            // Process the message
            Console.WriteLine($"Received message: {message}");
            return Ok();
        }
    }
}
