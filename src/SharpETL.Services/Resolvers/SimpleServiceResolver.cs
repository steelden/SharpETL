using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using SharpETL.Components;

namespace SharpETL.Services.Resolvers
{
    public class SimpleServiceResolver : IServiceResolver
    {
        private IDictionary<Type, Func<IService>> _services;

        public SimpleServiceResolver()
        {
            _services = new Dictionary<Type, Func<IService>>();
        }

        public T GetService<T>() where T : IService
        {
            return (T)(_services[typeof(T)]());
        }

        public T QueryService<T>() where T : IService
        {
            return _services.ContainsKey(typeof(T)) ? GetService<T>() : default(T);
        }

        public void RegisterService(Type interfaceType, Func<IService> serviceProvider)
        {
            _services[interfaceType] = serviceProvider;
        }
    }
}
