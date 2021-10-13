namespace MetricsApp.Models
{
    public class WeatherForecast
    {
        public WeatherLocation location { get; set; }
        public WeatherCurrent current { get; set; }
        public WeatherAirQuality air_quality { get; set; }

        public decimal wind_mph { get; set; }
        public decimal wind_kph { get; set; }
        public int wind_degree { get; set; }
        public string wind_dir { get; set; }
        public decimal pressure_mb { get; set; }
        public decimal pressure_in { get; set; }
        public decimal precip_mm { get; set; }
        public decimal precip_in { get; set; }
        public int humidity { get; set; }
        public int cloud { get; set; }
        public decimal feelslike_c { get; set; }
        public decimal vis_km { get; set; }
        public decimal vis_miles { get; set; }
        public decimal uv { get; set; }
        public decimal gust_mph { get; set; }
        public decimal gust_kph { get; set; }
    }
}
