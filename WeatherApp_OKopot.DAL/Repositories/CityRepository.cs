using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WeatherApp_OKopot.DAL.EF;
using WeatherApp_OKopot.DAL.Entities;
using WeatherApp_OKopot.DAL.Interfaces;

namespace WeatherApp_OKopot.DAL.Repositories
{
    class CityRepository:IRepository<City>
    {
        private WeatherDbContext dbContext;

        public CityRepository(WeatherDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<City> GetAll()
        {
            return dbContext.Cities; 
        }

        public City Get(int id)
        {
            return dbContext.Cities.Find(id);
        }

        public void Create(City city)
        {
            dbContext.Cities.Add(city);
        }

        public void Update(City city)
        {
            dbContext.Entry(city).State = EntityState.Modified;
        }

        public IEnumerable<City> Find(Func<City, Boolean> predicate)
        {
            return dbContext.Cities.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            City city = dbContext.Cities.Find(id);
            if (city != null)
                dbContext.Cities.Remove(city);
        }

        public void Delete(IEnumerable<City> cities)
        {
            if (cities.Any())
                dbContext.Cities.RemoveRange(cities);
        }
    }
}
