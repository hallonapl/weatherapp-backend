using Azure;
using WeatherApp.Api.Models.Api;
using WeatherApp.Core.Clients;
using WeatherApp.Core.Exceptions;
using WeatherApp.Core.Models;
using WeatherApp.Core.Models.Entities;
using WeatherApp.Core.Services;

namespace WeatherApp.Api.Services
{
    public interface IWeatherDataService
    {
        Task<IEnumerable<WeatherResponse>> GetWeatherAsync(CancellationToken cancellationToken);

        Task<WeatherResponse?> GetWeatherByCityAsync(string city, CancellationToken cancellationToken);
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

        public async Task<IEnumerable<WeatherResponse>> GetWeatherAsync(CancellationToken cancellationToken)
        {
            var response = await _weatherRepository.GetWeatherDataAsync(cancellationToken);

            List<WeatherResponse> result = new();

            var weatherByCity = response.GroupBy(x => x.Name);
            foreach (var weather in weatherByCity)
            {
                var cityReport = MapToWeatherResponse(weather);
                if (cityReport != null)
                {
                    result.Add(cityReport);
                }
            }
            return result;
        }

        public async Task<WeatherResponse?> GetWeatherByCityAsync(string cityName, CancellationToken cancellationToken)
        {
            var response = await _weatherRepository.GetWeatherDataByCityAsync(cityName, cancellationToken);

            var result = MapToWeatherResponse(response);
            return result;
        }

        private WeatherResponse? MapToWeatherResponse(IEnumerable<WeatherEntity> weatherEntities)
        {
            if (!weatherEntities.Any())
            {
                return null;
            }
            return new WeatherResponse(
                LastUpdated: weatherEntities.Max(x => x.FetchedTimeStamp),
                Country: weatherEntities.First().Country,
                City: weatherEntities.First().Name,
                WeatherMeasurements: weatherEntities.Select(x =>
                    new WeatherMeasurement
                    (
                        TimeStamp: x.FetchedTimeStamp,
                        Temperature: x.Temperature,
                        TemperatureFeelsLike: x.TemperatureFeelsLike,
                        TemperatureMin: x.TemperatureMin,
                        TemperatureMax: x.TemperatureMax,
                        Pressure: x.Pressure,
                        Humidity: x.Humidity,
                        SeaLevel: x.SeaLevel,
                        GroundLevel: x.GroundLevel
                    )
                ).ToList()
            );
        }

        public async Task StoreWeatherDataAsync(WeatherPayload weatherData, CancellationToken cancellationToken)
        {
            var report = new WeatherEntity
            (
                Id: Guid.Empty,
                FetchedTimeStamp: DateTime.UtcNow,
                Country: weatherData.Sys.Country,
                Name: weatherData.Name,
                Temperature: weatherData.Main.Temp,
                TemperatureFeelsLike: weatherData.Main.Feels_Like,
                TemperatureMin: weatherData.Main.Temp_Min,
                TemperatureMax: weatherData.Main.Temp_Max,
                Pressure: weatherData.Main.Pressure,
                Humidity: weatherData.Main.Humidity,
                SeaLevel: weatherData.Main.Sea_Level,
                GroundLevel: weatherData.Main.Grnd_Level
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
