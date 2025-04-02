namespace WeatherApp.Core.Models.Entities
{
    public record WeatherEntity(
        Guid Id,
        DateTime FetchedTimeStamp,
        string Country,
        string Name,
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