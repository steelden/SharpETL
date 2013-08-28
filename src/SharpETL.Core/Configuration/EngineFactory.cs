using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Configuration.Configurators;

namespace SharpETL.Configuration
{
    public sealed class EngineFactory : IEngineFactory
    {
        public IEngine Create(Action<IMutableEngineConfiguration> configurator)
        {
            return Create(new ManualEngineConfigurator(configurator));
        }

        public IEngine LoadXml(string path)
        {
            return Create(new XmlEngineConfigurator(path));
        }

        public IEngine Create(IMutableEngineConfigurator configurator)
        {
            return new DataEngine().Setup(configurator);
        }
    }
}
