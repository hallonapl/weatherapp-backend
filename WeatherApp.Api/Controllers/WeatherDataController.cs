using Microsoft.AspNetCore.Mvc;
using WeatherApp.Api.Services;
using WeatherApp.Core.Models;

namespace WeatherApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherDataController : ControllerBase
    {
        private readonly ILogger<WeatherDataController> _logger;
        private readonly IWeatherDataService _weatherDataService;

        public WeatherDataController(ILogger<WeatherDataController> logger, IWeatherDataService weatherDataService)
        {
            _logger = logger;
            _weatherDataService = weatherDataService;
        }

        [HttpGet]
        [Route("weather")]
        public async Task<ActionResult<IEnumerable<WeatherPayload>>> GetWeatherDataAsync(CancellationToken cancellationToken)
        {
            var result = await _weatherDataService.GetWeatherDataAsync(cancellationToken);
            return Ok(result);
        }


        [HttpPost]
        [Route("weather/fetch")]
        public async Task<ActionResult<IEnumerable<WeatherPayload>>> FetchWeatherAsync(CancellationToken cancellationToken)
        {
            var result = await _weatherDataService.Fetch(cancellationToken);
            return Ok(result);
        }
    }
}
