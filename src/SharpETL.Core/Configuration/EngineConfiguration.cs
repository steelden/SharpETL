using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using SharpETL.Utility;
using SharpETL.Components;
using SharpETL.Planning;

namespace SharpETL.Configuration
{
    [Serializable]
    public class EngineConfiguration : IEngineConfiguration
    {
        protected IEngine _engine;
        protected IServiceResolver _serviceResolver;
        protected Func<IContext> _contextProvider;
        protected Func<IPlan> _planProvider;

        protected IList<IAction> _actions;
        protected IList<ILink> _links;
        protected IDictionary<string, IContext> _contexts;

        protected EngineConfiguration()
        {
            _contextProvider = null;
            _planProvider = null;
            _actions = new List<IAction>();
            _links = new List<ILink>();
            _contexts = new Dictionary<string, IContext>();
        }

        public EngineConfiguration(IEngine engine)
            : this()
        {
            if (engine == null) throw new ArgumentNullException("engine");
            _engine = engine;
        }

        public IEngine Engine { get { return _engine; } }
        public IEnumerable<IAction> Actions { get { return _actions; } }
        public IEnumerable<ILink> Links { get { return _links; } }

        public IServiceResolver ServiceResolver
        {
            get { ThrowIfNull(_serviceResolver, "ServiceResolver"); return _serviceResolver; }
        }

        public Func<IContext> ContextProvider
        {
            get { ThrowIfNull(_contextProvider, "ContextProvider"); return _contextProvider; }
        }

        public Func<IPlan> PlanProvider
        {
            get { ThrowIfNull(_planProvider, "PlanProvider"); return _planProvider; }
        }

        private void ThrowIfNull(object obj, string name)
        {
            if (obj == null) throw new InvalidOperationException(String.Format("{0} is not configured.", name));
        }

        public IContext GetContextForId(string id)
        {
            if (String.IsNullOrEmpty(id)) throw new ArgumentNullException("id");
            IContext context;
            if (!_contexts.TryGetValue(id, out context)) {
                context = ContextProvider();
                _contexts[id] = context;
            }
            return context;
        }
    }
}
