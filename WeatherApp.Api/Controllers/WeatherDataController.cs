using Microsoft.AspNetCore.Mvc;
using WeatherApp.Api.Models.Api;
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

        //[HttpGet]
        //[Route("weatherreport")]
        //public async Task<ActionResult<IEnumerable<WeatherPayload>>> GetWeatherReportsAsync(CancellationToken cancellationToken)
        //{
        //    var result = await _weatherDataService.GetWeatherReportsAsync(cancellationToken);
        //    return Ok(result);
        //}

        //[HttpGet]
        //[Route("weatherreport/city/{city:string}")]
        //public async Task<ActionResult<IEnumerable<WeatherPayload>>> GetWeatherReportsByCityAsync([FromRoute] string city, CancellationToken cancellationToken)
        //{
        //    var result = await _weatherDataService.GetWeatherReportsByCityAsync(city, cancellationToken);
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("weather")]
        public async Task<ActionResult<IEnumerable<WeatherResponse>>> GetWeatherAsync(CancellationToken cancellationToken)
        {
            var result = await _weatherDataService.GetWeatherAsync(cancellationToken);
            return Ok(result);
        }


        [HttpGet]
        [Route("weather/city/{city}")]
        public async Task<ActionResult<WeatherResponse>> GetWeatherByCityAsync([FromRoute] string city, CancellationToken cancellationToken)
        {
            var result = await _weatherDataService.GetWeatherByCityAsync(city, cancellationToken);
            return Ok(result);
        }

        //[HttpPost]
        //[Route("weather/fetch")]
        //public async Task<ActionResult<IEnumerable<WeatherPayload>>> FetchWeatherAsync(CancellationToken cancellationToken)
        //{
        //    var result = await _weatherDataService.Fetch(cancellationToken);
        //    return Ok(result);
        //}
    }
}
