using System;

namespace WeatherApp_OKopot.Models
{
    public class HistoryModel
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int CityId { get; set; }

        public virtual CityModel City { get; set; }
    }
}