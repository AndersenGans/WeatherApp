using System;
using WeatherApp_OKopot.DAL.EF;
using WeatherApp_OKopot.DAL.Entities;
using WeatherApp_OKopot.DAL.Interfaces;

namespace WeatherApp_OKopot.DAL.Repositories
{
    public class EFUnitOfWork:IUnitOfWork
    {
        private WeatherDbContext dbContext;
        private WeatherRepository weatherRepository;
        private CityRepository cityRepository;
        private HistoryRepository historyRepository;
        private bool disposed;

        public EFUnitOfWork(string connectionString)
        {
            dbContext = new WeatherDbContext(connectionString);
        }

        public IRepository<City> Cities
        {
            get
            {
                if (cityRepository == null)
                    cityRepository = new CityRepository(dbContext);
                return cityRepository;
            }
        }

        public IRepository<Weather> Weathers
        {
            get
            {
                if(weatherRepository == null)
                    weatherRepository = new WeatherRepository(dbContext);
                return weatherRepository;
            }
        }

        public IRepository<History> Histories
        {
            get
            {
                if(historyRepository == null)
                    historyRepository = new HistoryRepository(dbContext);
                return historyRepository;
            }
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
