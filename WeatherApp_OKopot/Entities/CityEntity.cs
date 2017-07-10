using System.Collections.Generic;

namespace WeatherApp_OKopot.Entities
{
    public class CityEntity
    {
        public int CityEntityId { get; set; }
        public string Name { get; set; }
        public string AlternativeName { get; set; }

        public virtual ICollection<WeatherEntity> WeatherEntities { get; set; }
        public virtual ICollection<HistoryEntity> HistoryEntities { get; set; }

        public CityEntity()
        {
            WeatherEntities = new HashSet<WeatherEntity>();
            HistoryEntities = new HashSet<HistoryEntity>();
        }
    }
}