using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Configuration.Configurators
{
    public class ManualEngineConfigurator : IMutableEngineConfigurator
    {
        private Action<IMutableEngineConfiguration> _configurator;

        public ManualEngineConfigurator(Action<IMutableEngineConfiguration> configurator)
        {
            if (configurator == null) throw new ArgumentNullException("config");
            _configurator = configurator;
        }

        public void Configure(IMutableEngineConfiguration config)
        {
            _configurator(config);
        }
    }
}
