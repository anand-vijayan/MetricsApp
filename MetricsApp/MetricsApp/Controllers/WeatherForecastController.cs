using System.Threading.Tasks;
using MetricsApp.Exceptions;
using MetricsApp.Models;
using MetricsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;

            _logger.LogDebug(1001, "NLogger injected to WeatherForecastController");
        }

        [HttpGet]
        public async Task<WeatherForecast> Get(string cityName, bool isAirQualityNeeded)
        {
            //Creating an "Not-Found" exception to check exception handling in middleware
            if (cityName == "Bengalore")
            {
                throw new DomainNotFoundException($"Weather data for {cityName} not found");
            }

            //Logging message with "Event Id" which will be a constant for this method and message
            //Since this is only for tracing it will be logged in developer machine. It is set in appSettings.json
            _logger.LogTrace(1002, "Requesting weather of {cityName} with Air Quality info needed as {isAirQualityNeeded}", cityName, isAirQualityNeeded);
            return await _weatherService.GetWeatherForecast(cityName, isAirQualityNeeded);
        }
    }
}
