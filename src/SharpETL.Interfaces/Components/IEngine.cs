using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;

namespace SharpETL.Components
{
    public interface IEngine
    {
        IContext GlobalContext { get; }
        IEngineConfiguration Configuration { get; }

        IEngine Setup(IMutableEngineConfigurator configurator);
        IEngine Setup(Action<IMutableEngineConfiguration> configurator);
        IEngine Setup(Func<IMutableEngineConfiguration> creator, Action<IMutableEngineConfiguration> configurator);
        void Run();

        T GetService<T>() where T : IService;
        T QueryService<T>() where T : IService;
    }
}
