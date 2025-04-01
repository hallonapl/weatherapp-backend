using Dapper;
using WeatherApp.Core.DataAccess;
using WeatherApp.Core.Exceptions;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Services
{
    public interface IWeatherDataRepository
    {
        Task<IEnumerable<WeatherReport>> GetWeatherDataAsync(CancellationToken cancellationToken);
        Task StoreWeatherDataAsync(WeatherReport weatherData, CancellationToken cancellationToken);
    }

    public class WeatherDataRepository : IWeatherDataRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public WeatherDataRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<WeatherReport>> GetWeatherDataAsync(CancellationToken cancellationToken)
        {
            using (var connection = _dbConnectionFactory.GetConnection())
            {
                await connection.OpenAsync(cancellationToken);
                using (var transaction = connection.BeginTransaction())
                {
                    var result = await connection.QueryAsync<WeatherReport>(
                        SqlQuery.GetWeatherReports,
                        transaction: transaction
                    );
                    transaction.Commit();
                    return result;
                }
            }
        }

        public async Task StoreWeatherDataAsync(WeatherReport weatherData, CancellationToken cancellationToken)
        {
            using (var connection = _dbConnectionFactory.GetConnection())
            {
                await connection.OpenAsync(cancellationToken);
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync(
                            SqlQuery.InsertWeatherReport,
                            new
                            {
                                weatherData.FetchedTimeStamp,
                                weatherData.Country,
                                weatherData.Name,
                                weatherData.Temperature,
                                weatherData.TemperatureFeelsLike,
                                weatherData.TemperatureMin,
                                weatherData.TemperatureMax,
                                weatherData.Pressure,
                                weatherData.Humidity,
                                weatherData.SeaLevel,
                                weatherData.GroundLevel
                            },
                            transaction: transaction,
                            commandTimeout: null,
                            commandType: null
                        );
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new WeatherDataRepositoryStoreException("Failed to store weather report", ex);
                    }
                }
            }
        }
    }

    static class SqlQuery
    {
        public static string GetWeatherReports = @"
            SELECT
                Id,
                FetchedTimeStamp,
                Country,
                Name,
                Temperature,
                TemperatureFeelsLike,
                TemperatureMin,
                TemperatureMax,
                Pressure,
                Humidity,
                SeaLevel,
                GroundLevel
            FROM
                WeatherReport
        ";

        public static string InsertWeatherReport = @"
            INSERT INTO WeatherReport (
                FetchedTimeStamp,
                Country,
                Name,
                Temperature,
                TemperatureFeelsLike,
                TemperatureMin,
                TemperatureMax,
                Pressure,
                Humidity,
                SeaLevel,
                GroundLevel
            ) VALUES (
                @FetchedTimeStamp,
                @Country,
                @Name,
                @Temperature,
                @TemperatureFeelsLike,
                @TemperatureMin,
                @TemperatureMax,
                @Pressure,
                @Humidity,
                @SeaLevel,
                @GroundLevel
            )
        ";
    }

}
