namespace MetricsApp.Models
{
    public class WeatherCurrent
    {
        public int last_updated_epoch { get; set; }
        public string last_updated { get; set; }
        public decimal temp_c { get; set; }
        public decimal temp_f { get; set; }
        public int is_day { get; set; }
        public WeatherCondition condition { get; set; }
    }
}
