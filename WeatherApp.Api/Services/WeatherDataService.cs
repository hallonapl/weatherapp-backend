using WeatherApp.Api.Models.Api;
using WeatherApp.Core.Clients;
using WeatherApp.Core.Exceptions;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services;

namespace WeatherApp.Api.Services
{
    public interface IWeatherDataService
    {
        Task<IEnumerable<WeatherResponse>> GetWeatherDataAsync(CancellationToken cancellationToken);

        Task StoreWeatherDataAsync(WeatherPayload weatherData, CancellationToken cancellationToken);

        Task<WeatherPayload> Fetch(CancellationToken cancellationToken);

    }

    public class WeatherDataService : IWeatherDataService
    {
        private readonly ILogger<WeatherDataService> _logger;
        private readonly IWeatherDataRepository _weatherRepository;
        private readonly IWeatherDataClient _client;

        public WeatherDataService(ILogger<WeatherDataService> logger, IWeatherDataRepository weatherRepository, IWeatherDataClient client)
        {
            _logger = logger;
            _weatherRepository = weatherRepository;
            _client = client;
        }

        public async Task<IEnumerable<WeatherResponse>> GetWeatherDataAsync(CancellationToken cancellationToken)
        {
            var data = await _weatherRepository.GetWeatherDataAsync(cancellationToken);
            var result = data.Select(d => new WeatherResponse(
                d.Id,
                d.FetchedTimeStamp,
                d.Country,
                d.Name,
                d.Temperature,
                d.TemperatureFeelsLike,
                d.TemperatureMin,
                d.TemperatureMax,
                d.Pressure,
                d.Humidity,
                d.SeaLevel,
                d.GroundLevel));
            return result;
        }

        public async Task StoreWeatherDataAsync(WeatherPayload weatherData, CancellationToken cancellationToken)
        {
            var report = new WeatherReport
            (
                Id: Guid.Empty,
                FetchedTimeStamp: DateTime.UtcNow,
                Country: weatherData.Sys.Country,
                Name: weatherData.Name,
                Temperature: weatherData.Main.Temp,
                TemperatureFeelsLike: weatherData.Main.FeelsLike,
                TemperatureMin: weatherData.Main.TempMin,
                TemperatureMax: weatherData.Main.TempMax,
                Pressure: weatherData.Main.Pressure,
                Humidity: weatherData.Main.Humidity,
                SeaLevel: weatherData.Main.SeaLevel,
                GroundLevel: weatherData.Main.GrndLevel
            );
            await _weatherRepository.StoreWeatherDataAsync(report, cancellationToken);
        }

        public async Task<WeatherPayload> Fetch(CancellationToken cancellationToken)
        {
            var cityName = "London";
            try
            {
                var response = await _client.GetWeatherReportForCityAsync(cityName, cancellationToken);
                return response;
            }
            catch (WeatherDataClientFetchException)
            {
                _logger.LogWarning("Failed to fetch weather data for {city}", cityName);
                throw;
            }
        }
    }

}
