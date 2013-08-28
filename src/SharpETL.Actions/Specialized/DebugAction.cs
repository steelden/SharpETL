using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using System.Reactive;
using SharpETL.Actions.Binding;
using SharpETL.Services;
using SharpETL.Services.Loggers;

namespace SharpETL.Actions.Specialized
{
    public class DebugAction : ActionBase
    {
        private ILoggerService _logger;

        public DebugAction(string id)
            : base(id)
        {
        }

        protected override void OnSetEngine()
        {
            base.OnSetEngine();
            _logger = Engine.GetService<ILoggerService>();
        }

        protected override IConfigurableBindingStrategy OnCreateStrategy()
        {
            return new TimestampedDoStrategy();
        }

        protected override void OnConfigureStrategy(IConfigurableBindingStrategy strategy)
        {
            strategy
                //.OnElement(te => OnElement(te))
                .OnElement((e, _) => OnElement(e))
                .OnCompleted(_ => OnCompleted())
                .OnError((ex, _) => OnError(ex));
        }

        protected virtual void OnElement(Timestamped<IElement> element)
        {
            _logger.Debug("{0}: OnElement ({1}): {2}", Id, element.Timestamp, element.Value);
        }

        protected virtual bool OnElement(IElement element)
        {
            _logger.Debug("{0}: OnElement: {1}", Id, element);
            return true;
        }

        protected virtual bool OnCompleted()
        {
            _logger.Debug("{0}: OnCompleted", Id);
            return true;
        }

        protected virtual bool OnError(Exception ex)
        {
            _logger.Debug("{0}: OnError({1}): {2}", Id, ex.GetType().Name, ex.Message);
            return true;
        }
    }
}
