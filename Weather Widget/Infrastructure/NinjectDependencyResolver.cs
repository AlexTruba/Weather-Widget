using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weather_Widget.Models;

namespace Weather_Widget.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {
            kernel.Bind<IWeather>().To<WeatherAPI>();
            kernel.Bind<IRepository<Log>>().To<LogRepository>();
        }

        public object GetService(Type serviceType) => kernel.TryGet(serviceType);

        public IEnumerable<object> GetServices(Type serviceType) => kernel.GetAll(serviceType);

    }
}