using System.Text.Json;

namespace WeatherApp.Core.Configuration
{
    public static class SerializerOptions
    {
        public static JsonSerializerOptions PayloadSerializerOptions =>
            new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
    }
}