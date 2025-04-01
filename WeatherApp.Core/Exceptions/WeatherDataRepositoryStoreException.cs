namespace WeatherApp.Core.Exceptions
{
    public class WeatherDataRepositoryStoreException : Exception
    {
        public WeatherDataRepositoryStoreException()
        {
        }

        public WeatherDataRepositoryStoreException(string? message) : base(message)
        {
        }

        public WeatherDataRepositoryStoreException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
