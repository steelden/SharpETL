using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Utility;
using SharpETL.Actions.Binding;

namespace SharpETL.Actions
{
    public abstract class ActionBase : IAction
    {
        private string _id;
        private IEngine _engine;
        private IContext _actionContext;
        private IObservable<IElement> _output;
        private IBindingStrategy _strategy;

        private void ThrowIfEngineIsNull()
        {
            if (_engine == null) throw new InvalidOperationException("_engine is null (use SetEngine)");
        }

        public ActionBase(string id)
        {
            _id = id;
        }

        public void SetActionContext(IContext actionContext)
        {
            if (actionContext == null) throw new ArgumentNullException("actionContext");
            _actionContext = actionContext;
        }

        public void SetInput(IObservable<IElement> input)
        {
            if (input == null) throw new ArgumentNullException("input");
            IConfigurableBindingStrategy strategy = OnCreateStrategy();
            OnConfigureStrategy(strategy);
            _strategy = strategy;
            _output = OnCreateOutput(input);
        }

        public void SetEngine(IEngine engine)
        {
            if (engine == null) throw new ArgumentNullException("engine");
            _engine = engine;
            OnSetEngine();
        }

        protected virtual IObservable<IElement> OnCreateOutput(IObservable<IElement> input)
        {
            if (_strategy != null) {
                return _strategy.Bind(input);
            }
            return input;
        }

        protected abstract IConfigurableBindingStrategy OnCreateStrategy();
        protected abstract void OnConfigureStrategy(IConfigurableBindingStrategy strategy);

        protected virtual void OnSetEngine()
        {
            SetActionContext(Engine.Configuration.GetContextForId(Id));
        }

        public string Id { get { return _id; } }
        public IEngine Engine { get { ThrowIfEngineIsNull(); return _engine; } }

        public IContext GlobalContext { get { return Engine.GlobalContext; } }
        public IContext ActionContext { get { return _actionContext; } }

        public IObservable<IElement> Output { get { return _output; } }
    }
}
