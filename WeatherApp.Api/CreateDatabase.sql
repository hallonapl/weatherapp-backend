CREATE DATABASE Weather;

GO

USE Weather;

GO

CREATE TABLE WeatherReport (
    Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    FetchedTimeStamp DATETIME NOT NULL,
    Country NVARCHAR(100) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Temperature FLOAT NOT NULL,
    TemperatureFeelsLike FLOAT NOT NULL,
    TemperatureMin FLOAT NOT NULL,
    TemperatureMax FLOAT NOT NULL,
    Pressure INT NOT NULL,
    Humidity INT NOT NULL,
    SeaLevel INT NOT NULL,
    GroundLevel INT NOT NULL
);
