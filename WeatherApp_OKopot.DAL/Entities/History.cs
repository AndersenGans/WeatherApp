using System;

namespace WeatherApp_OKopot.DAL.Entities
{
    public class History
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}
