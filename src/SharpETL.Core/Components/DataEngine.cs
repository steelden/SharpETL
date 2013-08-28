using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using SharpETL.Planning;

namespace SharpETL.Components
{
    public sealed class DataEngine : IEngine
    {
        private IEngineConfiguration _config;
        private IPlan _plan;
        private IContext _globalContext;
        private IDictionary<Type, object> _services;

        public IEngineConfiguration Configuration { get { ThrowIfConfigIsNull(); return _config; } }
        public IContext GlobalContext { get { return _globalContext; } }

        public DataEngine()
        {
            _services = new Dictionary<Type, object>();
        }

        private void ThrowIfConfigIsNull()
        {
            if (_config == null) throw new InvalidOperationException("_config is null (use Setup())");
        }

        public IEngine Setup(IMutableEngineConfigurator provider)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            return Setup(provider.Configure);
        }

        public IEngine Setup(Action<IMutableEngineConfiguration> configurator)
        {
            if (configurator == null) throw new ArgumentNullException("configurator");
            return Setup(() => new MutableEngineConfiguration(this), configurator);
        }

        public IEngine Setup(Func<IMutableEngineConfiguration> creator, Action<IMutableEngineConfiguration> configurator)
        {
            if (creator == null) throw new ArgumentNullException("creator");
            if (configurator == null) throw new ArgumentNullException("configurator");
            IMutableEngineConfiguration config = creator();
            if (config == null) throw new InvalidOperationException("config from creator() is null");
            _config = config;
            configurator(config);
            _globalContext = config.ContextProvider();
            _plan = config.PlanProvider();
            foreach (var action in _config.Actions) {
                action.SetEngine(this);
            }
            return this;
        }

        public void Run()
        {
            _plan.Execute();
        }

        public T GetService<T>() where T : IService
        {
            return Configuration.ServiceResolver.GetService<T>();
        }

        public T QueryService<T>() where T : IService
        {
            return Configuration.ServiceResolver.QueryService<T>();
        }
    }
}
