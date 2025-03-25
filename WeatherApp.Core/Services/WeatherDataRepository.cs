using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Services
{
    public interface IWeatherDataRepository
    {
        Task<IEnumerable<WeatherDatum>> GetWeatherData();
        Task SaveWeatherData(WeatherDatum weatherData);
    }

    public class WeatherDataRepository : IWeatherDataRepository
    {
        public WeatherDataRepository() { }

        private List<WeatherDatum> DummyData => new List<WeatherDatum>()
            {
                new WeatherDatum(1, "New York", "Sunny", 25.3, 5.2, 60),
                new WeatherDatum(2, "Los Angeles", "Cloudy", 22.1, 3.5, 70),
                new WeatherDatum(3, "Chicago", "Rainy", 18.4, 6.0, 80)
            };

        public Task<IEnumerable<WeatherDatum>> GetWeatherData()
        {
            return Task.FromResult<IEnumerable<WeatherDatum>>(DummyData);
        }

        public Task SaveWeatherData(WeatherDatum weatherData)
        {
            throw new NotImplementedException();
        }
    }

}
