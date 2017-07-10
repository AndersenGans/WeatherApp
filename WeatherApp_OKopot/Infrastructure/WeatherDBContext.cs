using System.Data.Entity;
using WeatherApp_OKopot.Entities;

namespace WeatherApp_OKopot.Infrastructure
{
    public class WeatherDBContext:DbContext
    {
        public WeatherDBContext():base("WeatherDB")
        {
            Database.SetInitializer(new WeatherDbInitializer());
            Database.Initialize(true);
        }

        public DbSet<WeatherEntity> WeatherEntities { get; set; }
        public DbSet<CityEntity> CityEntities { get; set; }
        public DbSet<HistoryEntity> HistoryEntities { get; set; }
    }
}