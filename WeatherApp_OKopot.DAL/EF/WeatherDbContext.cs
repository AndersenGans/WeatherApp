using System.Data.Entity;
using WeatherApp_OKopot.DAL.Entities;

namespace WeatherApp_OKopot.DAL.EF
{
    class WeatherDbContext:DbContext
    {
        public WeatherDbContext(string connectionString) :base(connectionString)
        {
            Database.SetInitializer(new WeatherDbInitializer());
            Database.Initialize(true);
        }

        public DbSet<Weather> Weathers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<History> Histories { get; set; }
    }
}
