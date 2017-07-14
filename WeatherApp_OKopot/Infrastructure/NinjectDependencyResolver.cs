using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using WeatherApp_OKopot.BLL.Interfaces;
using WeatherApp_OKopot.BLL.Services;

namespace WeatherApp_OKopot.Infrastructure
{
    public class NinjectDependencyResolver:IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IService>().To<Service>();
        }
    }
}