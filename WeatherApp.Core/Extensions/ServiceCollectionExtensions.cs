using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Core.Clients;
using WeatherApp.Core.DataAccess;
using WeatherApp.Core.Services;

namespace WeatherApp.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, string dbConnectionString)
        {
            services.AddScoped<IWeatherDataRepository, WeatherDataRepository>();
            services.AddScoped<IWeatherDataClient, WeatherDataClient>();
            services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>(services =>
            {
                return new SqlConnectionFactory(dbConnectionString);
            });
            return services;
        }
}
}
