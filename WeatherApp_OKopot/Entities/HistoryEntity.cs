using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp_OKopot.Entities
{
    public class HistoryEntity
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public int CityEntityId { get; set; }
        public virtual CityEntity CityEntity { get; set; }
    }
}