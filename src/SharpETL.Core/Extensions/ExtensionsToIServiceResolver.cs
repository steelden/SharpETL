using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Configuration;

namespace SharpETL.Extensions
{
    public static class ExtensionsToIServiceResolver
    {
        public static void RegisterService<T>(this IServiceResolver resolver, Func<IService> serviceProvider) where T : IService
        {
            resolver.RegisterService(typeof(T), serviceProvider);
        }

        public static void RegisterService(this IServiceResolver resolver, IServiceInfo serviceInfo)
        {
            resolver.RegisterService(serviceInfo.InterfaceType, () => serviceInfo.Create());
        }
    }
}
