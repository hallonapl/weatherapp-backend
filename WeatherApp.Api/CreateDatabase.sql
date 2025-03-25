-- Create the WeatherData database
CREATE DATABASE WeatherData;
GO

-- Use the WeatherData database
USE WeatherData;
GO

-- Create the WeatherData table
CREATE TABLE WeatherData (
    Id INT PRIMARY KEY IDENTITY(1,1),
    City NVARCHAR(100) NOT NULL,
    Temperature DECIMAL(5,2) NOT NULL,
    Humidity INT NOT NULL,
    WeatherDescription NVARCHAR(255),
    DateRecorded DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Create the Logs table
CREATE TABLE Logs (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    LogMessage NVARCHAR(1000) NOT NULL,
    LogDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO
