using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Actions.Binding;
using SharpETL.Components;

namespace SharpETL.Actions
{
    [Flags]
    public enum ActionEvents
    {
        None = 0,
        Element = 1,
        Completed = 2,
        Error = 4,
        Finally = 8,
        All = Element | Completed | Error | Finally
    }

    public abstract class BindedActionBase : ActionBase
    {
        public BindedActionBase(string id)
            : base(id)
        {
        }

        protected abstract ActionEvents OnGetExpectedEvents();

        protected virtual bool OnElement(IElement element, IObserver<IElement> sink) { return true; }
        protected virtual bool OnCompleted(IObserver<IElement> sink) { return true; }
        protected virtual bool OnError(Exception exception, IObserver<IElement> sink) { return true; }
        protected virtual void OnFinally() { }

        protected override IConfigurableBindingStrategy OnCreateStrategy()
        {
            return new BindingStrategy();
        }

        protected override void OnConfigureStrategy(IConfigurableBindingStrategy strategy)
        {
            ActionEvents events = OnGetExpectedEvents();
            if ((events & ActionEvents.Element) > 0) {
                strategy.OnElement(OnElement);
            }
            if ((events & ActionEvents.Completed) > 0) {
                strategy.OnCompleted(OnCompleted);
            }
            if ((events & ActionEvents.Error) > 0) {
                strategy.OnError(OnError);
            }
            if ((events & ActionEvents.Finally) > 0) {
                strategy.OnFinally(OnFinally);
            }
        }
    }
}
