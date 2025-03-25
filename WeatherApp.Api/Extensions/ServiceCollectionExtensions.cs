using WeatherApp.Api.Services;

namespace WeatherApp.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IWeatherDataService, WeatherDataService>();
            return services;
        }
    }
}
