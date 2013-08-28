using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Configuration
{
    public interface IServiceInfo
    {
        Type InterfaceType { get; }
        Type ClassType { get; }
        IService Create();
    }

    public interface IServiceInfo<out TInterface, out TClass> : IServiceInfo
        where TInterface : IService
        where TClass : TInterface, new()
    {
        //TClass Create();
    }
}
