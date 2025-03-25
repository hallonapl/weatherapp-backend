using Microsoft.AspNetCore.Mvc;
using WeatherApp.Api.Models;
using WeatherApp.Api.Services;

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
        public async Task<ActionResult<IEnumerable<WeatherDatum>>> GetWeatherData()
        {
            var result = await _weatherDataService.GetWeatherDataAsync();
            return Ok(result);
        }
    }
}
