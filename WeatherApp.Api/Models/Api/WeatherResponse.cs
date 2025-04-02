namespace WeatherApp.Api.Models.Api
{
    public record WeatherResponse
        (
            DateTime LastUpdated,
            string Country,
            string City,
            IEnumerable<WeatherMeasurement> WeatherMeasurements
        );

    public record WeatherMeasurement
        (
            DateTime TimeStamp,
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
