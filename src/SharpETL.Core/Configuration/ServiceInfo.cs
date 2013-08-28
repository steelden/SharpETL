using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Configuration
{
    [Serializable]
    public sealed class ServiceInfo : IServiceInfo
    {
        private Type _interfaceType;
        private Type _classType;

        public ServiceInfo(Type interfaceType, Type classType) 
        {
            _interfaceType = interfaceType;
            _classType = classType;
        }

        public Type InterfaceType { get { return _interfaceType; } }
        public Type ClassType { get { return _classType; } }
        public IService Create() { return (IService)Activator.CreateInstance(_classType); }
    }

    [Serializable]
    public sealed class ServiceInfo<TInterface, TClass> : IServiceInfo<TInterface, TClass>
        where TInterface : IService
        where TClass : TInterface, new()
    {
        private Type _interfaceType;
        private Type _classType;

        public ServiceInfo()
        {
            _interfaceType = typeof(TInterface);
            _classType = typeof(TClass);
        }

        public Type InterfaceType { get { return _interfaceType; } }
        public Type ClassType { get { return _classType; } }
        public IService Create() { return new TClass(); }
    }
}
