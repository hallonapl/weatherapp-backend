using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Core.Exceptions
{
    public class WeatherDataClientFetchException : Exception
    {
        public WeatherDataClientFetchException()
        {
        }

        public WeatherDataClientFetchException(string? message) : base(message)
        {
        }

        public WeatherDataClientFetchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
