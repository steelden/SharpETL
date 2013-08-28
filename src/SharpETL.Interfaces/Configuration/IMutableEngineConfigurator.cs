using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Configuration
{
    public interface IMutableEngineConfigurator
    {
        void Configure(IMutableEngineConfiguration config);
    }
}
