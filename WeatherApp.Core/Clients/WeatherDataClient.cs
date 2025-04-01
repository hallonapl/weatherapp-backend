using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Core.Configuration;
using WeatherApp.Core.Exceptions;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services;

namespace WeatherApp.Core.Clients
{
    public interface IWeatherDataClient
    {
        public Task<WeatherPayload> GetWeatherReportForCityAsync(string cityName, CancellationToken cancellationToken);
    }

    public class WeatherDataClient : IWeatherDataClient
    {
        private readonly ILogger<WeatherDataClient> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<WeatherDataClientSettings> _options;

        public WeatherDataClient(ILogger<WeatherDataClient> logger, IHttpClientFactory httpClientFactory, IOptions<WeatherDataClientSettings> options)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _options = options;
        }

        public async Task<WeatherPayload> GetWeatherReportForCityAsync(string cityName, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient(nameof(WeatherDataClient));
            var response = await client.GetAsync($"?q={cityName}&appid={_options.Value.ApiKey}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to get weather report");
                throw new WeatherDataClientFetchException("Failed to get weather report");
            }
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<WeatherPayload>(content, SerializerOptions.PayloadSerializerOptions);
            if (result is null)
            {
                throw new WeatherDataClientFetchException("Failed to deserialize weather report");
            }
            return result;
        }
    }
}