using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using WeatherApp.Core.Interfaces;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services;
using Xunit;

namespace WeatherApp.Test.WeatherApp.Core.Tests.Services
{
    public class WeatherDataRepositoryTest
    {
        private readonly Mock<IWeatherDataRepository> _mockRepository;
        private readonly WeatherDataRepository _repository;

        public WeatherDataRepositoryTest()
        {
            _mockRepository = new Mock<IWeatherDataRepository>();
            _repository = new WeatherDataRepository(_mockRepository.Object);
        }

        [Fact]
        public async Task AddWeatherReportAsync_ShouldAddWeatherReport()
        {
            // Arrange
            var weatherReport = new WeatherReport(
                Guid.NewGuid(),
                DateTime.UtcNow,
                "USA",
                "New York",
                25.0,
                23.0,
                20.0,
                30.0,
                1013,
                60,
                1015,
                1010
            );

            _mockRepository.Setup(repo => repo.AddWeatherReportAsync(weatherReport))
                .Returns(Task.CompletedTask);

            // Act
            await _repository.AddWeatherReportAsync(weatherReport);

            // Assert
            _mockRepository.Verify(repo => repo.AddWeatherReportAsync(weatherReport), Times.Once);
        }

        [Fact]
        public async Task GetWeatherReportByIdAsync_ShouldReturnWeatherReport()
        {
            // Arrange
            var weatherReportId = Guid.NewGuid();
            var weatherReport = new WeatherReport(
                weatherReportId,
                DateTime.UtcNow,
                "USA",
                "New York",
                25.0,
                23.0,
                20.0,
                30.0,
                1013,
                60,
                1015,
                1010
            );

            _mockRepository.Setup(repo => repo.GetWeatherReportByIdAsync(weatherReportId))
                .ReturnsAsync(weatherReport);

            // Act
            var result = await _repository.GetWeatherReportByIdAsync(weatherReportId);

            // Assert
            Assert.Equal(weatherReport, result);
        }

        [Fact]
        public async Task GetAllWeatherReportsAsync_ShouldReturnAllWeatherReports()
        {
            // Arrange
            var weatherReports = new List<WeatherReport>
            {
                new WeatherReport(Guid.NewGuid(), DateTime.UtcNow, "USA", "New York", 25.0, 23.0, 20.0, 30.0, 1013, 60, 1015, 1010),
                new WeatherReport(Guid.NewGuid(), DateTime.UtcNow, "Canada", "Toronto", 15.0, 13.0, 10.0, 20.0, 1010, 70, 1012, 1008)
            };

            _mockRepository.Setup(repo => repo.GetAllWeatherReportsAsync())
                .ReturnsAsync(weatherReports);

            // Act
            var result = await _repository.GetAllWeatherReportsAsync();

            // Assert
            Assert.Equal(weatherReports, result);
        }
    }
}
