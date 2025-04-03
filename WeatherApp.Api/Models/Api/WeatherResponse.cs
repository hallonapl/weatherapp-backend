namespace WeatherApp.Api.Models.Api
{
    public record WeatherResponse
        (
            DateTimeOffset LastUpdated,
            string Country,
            string City,
            IEnumerable<WeatherMeasurement> WeatherMeasurements
        );

    public record WeatherMeasurement
        (
            DateTimeOffset TimeStamp,
            double Temperature,
            double TemperatureFeelsLike,
            double TemperatureMin,
            double TemperatureMax,
            int Pressure,
            int Humidity,
            int SeaLevel,
            int GroundLevel
        );


}
