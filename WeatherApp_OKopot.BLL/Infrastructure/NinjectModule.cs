using System;
using System.Collections.Generic;
using Ninject;
using WeatherApp_OKopot.BLL.Interfaces;
using WeatherApp_OKopot.BLL.Services;
using WeatherApp_OKopot.DAL.Repositories;
using WeatherApp_OKopot.DAL.Interfaces;

namespace WeatherApp_OKopot.BLL.Infrastructure
{
    public class NinjectModule:Ninject.Modules.NinjectModule
    {
        private string connectionString;

        public NinjectModule(string connection)
        {
            connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}