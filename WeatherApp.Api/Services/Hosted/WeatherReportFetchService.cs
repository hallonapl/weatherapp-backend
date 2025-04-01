
using WeatherApp.Core.Clients;
using WeatherApp.Core.Exceptions;
using WeatherApp.Core.Services;

namespace WeatherApp.Api.Services.Hosted
{
    public class WeatherReportFetchService : BackgroundService
    {
        private readonly ILogger<WeatherReportFetchService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public WeatherReportFetchService(ILogger<WeatherReportFetchService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Fetch(cancellationToken);
                _logger.LogInformation("Weather data fetched at: {time}", DateTimeOffset.UtcNow);
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }

        private async Task Fetch(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var weatherDataClient = scope.ServiceProvider.GetRequiredService<IWeatherDataClient>();
            var weatherDataService = scope.ServiceProvider.GetRequiredService<IWeatherDataService>();
            var cityName = "London";
            try
            {
                var response = await weatherDataClient.GetWeatherReportForCityAsync(cityName, cancellationToken);
                await weatherDataService.StoreWeatherDataAsync(response, cancellationToken);
            }
            catch (WeatherDataClientFetchException)
            {
                _logger.LogWarning("Failed to fetch weather data for {city}", cityName);
            }
        }
    }
}
