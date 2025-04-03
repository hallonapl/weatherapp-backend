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
    }

    public class WeatherDataService : IWeatherDataService
    {
        private readonly ILogger<WeatherDataService> _logger;
        private readonly IWeatherDataRepository _weatherRepository;
        private readonly IWeatherDataClient _client;
        private readonly IDateTimeProvider _dateTimeProvider;

        public WeatherDataService(ILogger<WeatherDataService> logger, IWeatherDataRepository weatherRepository, IWeatherDataClient client, IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _weatherRepository = weatherRepository;
            _client = client;
            _dateTimeProvider = dateTimeProvider;
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
                LastUpdated: new DateTimeOffset(weatherEntities.Max(x => x.FetchedTimeStamp), TimeSpan.Zero),
                Country: weatherEntities.First().Country,
                City: weatherEntities.First().Name,
                WeatherMeasurements: weatherEntities.Select(x =>
                    new WeatherMeasurement
                    (
                        TimeStamp: new DateTimeOffset(x.FetchedTimeStamp, TimeSpan.Zero),
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
                FetchedTimeStamp: DateTime.SpecifyKind(_dateTimeProvider.UtcNow, DateTimeKind.Utc),
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
    }
}
