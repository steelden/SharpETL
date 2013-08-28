using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using SharpETL.Components;
using Ninject;

namespace SharpETL.Ninject
{
    public class NinjectServiceResolver : IServiceResolver
    {
        private IKernel _kernel;
        private ISet<Type> _registered;

        public NinjectServiceResolver(IKernel kernel)
        {
            if (kernel == null) throw new ArgumentNullException("kernel");
            _kernel = kernel;
            _registered = new HashSet<Type>();
        }

        public T GetService<T>() where T : IService
        {
            Type t = typeof(T);
            if (!_registered.Contains(t)) throw new InvalidOperationException(String.Format("Type '{0}' is not registered.", t.Name));
            return _kernel.Get<T>();
        }

        public T QueryService<T>() where T : IService
        {
            return (_registered.Contains(typeof(T)) ? _kernel.TryGet<T>() : default(T));
        }

        public void RegisterService(Type interfaceType, Func<IService> serviceProvider)
        {
            if (!typeof(IService).IsAssignableFrom(interfaceType)) {
                string text = String.Format("Type '{0}' must be IService derivative.", interfaceType.Name);
                throw new ArgumentException(text);
            }
            _registered.Add(interfaceType);
            _kernel.Rebind(interfaceType).ToMethod(ctx => serviceProvider());
        }
    }
}
