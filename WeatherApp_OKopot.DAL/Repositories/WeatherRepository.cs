using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp_OKopot.DAL.EF;
using WeatherApp_OKopot.DAL.Entities;
using WeatherApp_OKopot.DAL.Interfaces;

namespace WeatherApp_OKopot.DAL.Repositories
{
    class WeatherRepository:IRepository<Weather>
    {
        private WeatherDbContext dbContext;

        public WeatherRepository(WeatherDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Weather> GetAll()
        {
            return dbContext.Weathers;
        }

        public Weather Get(int id)
        {
            return dbContext.Weathers.Find(id);
        }

        public void Create(Weather weather)
        {
            dbContext.Weathers.Add(weather);
        }

        public void Update(Weather weather)
        {
            dbContext.Entry(weather).State = EntityState.Modified;
        }

        public IEnumerable<Weather> Find(Func<Weather, Boolean> predicate)
        {
            return dbContext.Weathers.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Weather weather = dbContext.Weathers.Find(id);
            if (weather != null)
                dbContext.Weathers.Remove(weather);
        }

        public void Delete(IEnumerable<Weather> weathers)
        {
            if (weathers.Any())
                dbContext.Weathers.RemoveRange(weathers);
        }
    }
}
