using Microsoft.Data.SqlClient;

namespace WeatherApp.Core.DataAccess
{
    public interface IDbConnectionFactory
    {
        SqlConnection GetConnection();
    }
}