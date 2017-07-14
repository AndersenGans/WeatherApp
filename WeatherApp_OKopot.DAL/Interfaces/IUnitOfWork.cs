using System;
using WeatherApp_OKopot.DAL.Entities;

namespace WeatherApp_OKopot.DAL.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository<City> Cities { get; }
        IRepository<History> Histories { get; }
        IRepository<Weather> Weathers { get; }
        void Save();
    }
}