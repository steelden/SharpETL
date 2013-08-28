using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using SharpETL.Utility;
using SharpETL.Components;
using SharpETL.Planning;
using System.Configuration;
using System.IO;
using SharpETL.Configuration.Configurators;

namespace SharpETL.Configuration
{
    public sealed class MutableEngineConfiguration : EngineConfiguration, IMutableEngineConfiguration
    {
        private MutableEngineConfiguration()
        {
        }

        public MutableEngineConfiguration(IEngine engine)
            : base(engine)
        {
        }

        public void SetServiceResolver(IServiceResolver serviceResolver)
        {
            if (serviceResolver == null) throw new ArgumentNullException("serviceResolver");
            _serviceResolver = serviceResolver;
        }

        public void SetContextProvider(Func<IContext> contextProvider)
        {
            if (contextProvider == null) throw new ArgumentNullException("contextProvider");
            _contextProvider = contextProvider;
        }

        public void SetPlanProvider(Func<IPlan> planProvider)
        {
            if (planProvider == null) throw new ArgumentNullException("planProvider");
            _planProvider = planProvider;
        }

        public void AddAction(IAction action)
        {
            if (action == null) throw new ArgumentNullException("action");
            _actions.Add(action);
        }

        public void AddLink(ILink link)
        {
            if (link == null) throw new ArgumentNullException("link");
            if (link.From == null) throw new ArgumentNullException("link.From");
            if (link.To == null) throw new ArgumentNullException("link.To");
            if (!_actions.Contains(link.From)) {
                AddAction(link.From);
            }
            if (!_actions.Contains(link.To)) {
                AddAction(link.To);
            }
            _links.Add(link);
        }

        public void AddLink(IAction source, IAction destination)
        {
            AddLink(new DataLink(source, destination));
        }

        public void LoadConfiguration(string path)
        {
            XmlEngineConfigurator configurator = new XmlEngineConfigurator(path);
            configurator.Configure(this);
        }

        public void LoadPlugin(string path)
        {
            System.Reflection.Assembly.LoadFrom(path);
        }
    }
}
