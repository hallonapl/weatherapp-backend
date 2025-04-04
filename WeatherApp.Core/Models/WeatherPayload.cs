﻿namespace WeatherApp.Core.Models
{
    public record Coord(double Lon, double Lat);

    public record Weather(int Id, string Main, string Description, string Icon);

    public record Main(
        double Temp,
        double Feels_Like,
        double Temp_Min,
        double Temp_Max,
        int Pressure,
        int Humidity,
        int Sea_Level,
        int Grnd_Level
    );

    public record Wind(double Speed, int Deg);

    public record Clouds(int All);

    public record Sys(
        int Type,
        int Id,
        string Country,
        long Sunrise,
        long Sunset
    );

    public record WeatherPayload(
        Coord Coord,
        List<Weather> Weather,
        string Base,
        Main Main,
        int Visibility,
        Wind Wind,
        Clouds Clouds,
        long Dt,
        Sys Sys,
        int Timezone,
        int Id,
        string Name,
        int Cod
    );
}
