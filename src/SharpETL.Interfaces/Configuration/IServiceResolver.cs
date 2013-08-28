using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Configuration
{
    public interface IServiceResolver
    {
        T GetService<T>() where T : IService;
        T QueryService<T>() where T : IService;
        void RegisterService(Type interfaceType, Func<IService> serviceProvider);
    }
}
