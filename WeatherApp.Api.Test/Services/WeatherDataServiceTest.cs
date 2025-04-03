#nullable disable
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using WeatherApp.Api.Models.Api;
using WeatherApp.Api.Services;
using WeatherApp.Core.Clients;
using WeatherApp.Core.Exceptions;
using WeatherApp.Core.Models;
using WeatherApp.Core.Models.Entities;
using WeatherApp.Core.Services;

namespace WeatherApp.Api.Test.Services
{
    [TestClass]
    public class WeatherDataServiceTest
    {
        private Mock<IWeatherDataRepository> _repository;
        private Fixture _fixture;
        private Mock<ILogger<WeatherDataService>> _logger;
        private Mock<IWeatherDataClient> _client;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private WeatherDataService sut;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _logger = new Mock<ILogger<WeatherDataService>>();
            _repository = new Mock<IWeatherDataRepository>();
            _client = new Mock<IWeatherDataClient>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            sut = new WeatherDataService(_logger.Object, _repository.Object, _client.Object, _dateTimeProvider.Object);
        }

        [TestMethod]
        public async Task StoreWeatherDataAsync_HappyPath_ShouldAddWeatherReport()
        {
            // Arrange
            var weatherPayload = _fixture.Create<WeatherPayload>();
            var timeStamp = _fixture.Create<DateTime>();
            var expected = new WeatherEntity(
                Id: Guid.Empty,
                FetchedTimeStamp: timeStamp,
                Name: weatherPayload.Name,
                Country: weatherPayload.Sys.Country,
                Temperature: weatherPayload.Main.Temp,
                TemperatureFeelsLike: weatherPayload.Main.Feels_Like,
                TemperatureMin: weatherPayload.Main.Temp_Min,
                TemperatureMax: weatherPayload.Main.Temp_Max,
                Pressure: weatherPayload.Main.Pressure,
                Humidity: weatherPayload.Main.Humidity,
                SeaLevel: weatherPayload.Main.Sea_Level,
                GroundLevel: weatherPayload.Main.Grnd_Level
            );
            var cancellationToken = CancellationToken.None;

            _dateTimeProvider.Setup(x => x.UtcNow)
                .Returns(timeStamp);
            _repository.Setup(x => x.StoreWeatherDataAsync(It.IsAny<WeatherEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await sut.StoreWeatherDataAsync(weatherPayload, cancellationToken);

            // Assert
            _repository.Verify(repo => repo.StoreWeatherDataAsync(expected, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task StoreWeatherDataAsync_WhenRepositoryThrows_ShouldThrow()
        {
            // Arrange
            var weatherPayload = _fixture.Create<WeatherPayload>();
            var cancellationToken = CancellationToken.None;

            _repository.Setup(x => x.StoreWeatherDataAsync(It.IsAny<WeatherEntity>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new WeatherDataRepositoryStoreException());

            // Act
            var action = () => sut.StoreWeatherDataAsync(weatherPayload, cancellationToken);

            // Assert
            await action.ShouldThrowAsync<WeatherDataRepositoryStoreException>();
        }

        [TestMethod]
        public async Task GetWeatherDataByCityAsync_HappyPath_ShouldReturnWeatherReport()
        {
            // Arrange
            var countryName = _fixture.Create<string>();
            var timeStamp = _fixture.Create<DateTime>();
            var weatherEntity = _fixture.Create<WeatherEntity>();
            var expected = new WeatherResponse(
                LastUpdated: new DateTimeOffset(weatherEntity.FetchedTimeStamp, TimeSpan.Zero),
                Country: weatherEntity.Country,
                City: weatherEntity.Name,
                WeatherMeasurements: new List<WeatherMeasurement>
                {
                    new WeatherMeasurement(
                        TimeStamp: new DateTimeOffset(weatherEntity.FetchedTimeStamp, TimeSpan.Zero),
                        Temperature: weatherEntity.Temperature,
                        TemperatureFeelsLike: weatherEntity.TemperatureFeelsLike,
                        TemperatureMin: weatherEntity.TemperatureMin,
                        TemperatureMax: weatherEntity.TemperatureMax,
                        Pressure: weatherEntity.Pressure,
                        Humidity: weatherEntity.Humidity,
                        SeaLevel: weatherEntity.SeaLevel,
                        GroundLevel: weatherEntity.GroundLevel
                    )
                });
            var cancellationToken = CancellationToken.None;

            _repository.Setup(x => x.GetWeatherDataByCityAsync(countryName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<WeatherEntity> { weatherEntity });

            // Act
            var actual = await sut.GetWeatherByCityAsync(countryName, cancellationToken);

            // Assert
            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
