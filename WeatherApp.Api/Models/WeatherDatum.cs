namespace WeatherApp.Api.Models
{
    public record WeatherDatum(
        string City,
        string Description,
        double Temperature,
        double WindSpeed,
        double Humidity);
}
