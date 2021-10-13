using System.Threading.Tasks;
using MetricsApp.Models;

namespace MetricsApp.Services
{
    public interface IWeatherService
    {
        Task<WeatherForecast> GetWeatherForecast(string cityName, bool isAirQualityNeeded);
    }
}
