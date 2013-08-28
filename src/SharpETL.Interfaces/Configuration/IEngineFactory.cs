using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Configuration
{
    public interface IEngineFactory
    {
        IEngine Create(IMutableEngineConfigurator configurator);
        IEngine Create(Action<IMutableEngineConfiguration> configurator);
        IEngine LoadXml(string path);
    }
}
