using WeatherApp.Api.Models;
using WeatherApp.Core.Services;

namespace WeatherApp.Api.Services
{
    public interface IWeatherDataService
    {
        Task<IEnumerable<WeatherDatum>> GetWeatherDataAsync();
    }

    public class WeatherDataService : IWeatherDataService
    {
        private readonly ILogger<WeatherDataService> _logger;
        private readonly IWeatherDataRepository _dataPersistenceService;

        public WeatherDataService(ILogger<WeatherDataService> logger, IWeatherDataRepository dataPersistenceService)
        {
            _logger = logger;
            _dataPersistenceService = dataPersistenceService;
        }

        public async Task<IEnumerable<WeatherDatum>> GetWeatherDataAsync()
        {
            var data = await _dataPersistenceService.GetWeatherData();
            var result = data.Select(d => new WeatherDatum(
                d.City,
                d.Description,
                d.Temperature,
                d.WindSpeed,
                d.Humidity));
            return result;
        }
    }

}
