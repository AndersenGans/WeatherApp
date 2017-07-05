using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherApp_OKopot.Models
{
    public class WeatherModel
    {
        public string CityName { get; set; }
        public double DayAvgTemp { get; set; }
        public double DayMinTemp { get; set; }
        public double DayMaxTemp { get; set; }
        public double NightTemp { get; set; }
        public double EveTemp { get; set; }
        public double MonTemp { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public string MainWeather { get; set; }
        public string DescWeather { get; set; }
        public double WindSpeed { get; set; }
        public double Cloudiness { get; set; }
        public string IconId { get; set; }
        public DateTime Day { get; set; }
    }
}