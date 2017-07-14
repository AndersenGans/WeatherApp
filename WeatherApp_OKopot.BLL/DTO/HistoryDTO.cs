using System;

namespace WeatherApp_OKopot.BLL.DTO
{
    public class HistoryDTO
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public int CityId { get; set; }

        public virtual CityDTO City { get; set; }
    }
}