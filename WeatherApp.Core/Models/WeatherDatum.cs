namespace WeatherApp.Core.Models
{
    public record WeatherDatum(
        int Id,
        string City,
        string Description,
        double Temperature,
        double WindSpeed,
        double Humidity);
}
