using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive;
using SharpETL.Components;
using System.Reactive.Linq;

namespace SharpETL.Actions.Binding
{
    public sealed class TimestampedDoStrategy : BindingStrategy
    {
        public TimestampedDoStrategy()
        {
        }

        protected override IObservable<IElement> OnBind(IObservable<IElement> input)
        {
            return base.OnBind(input.Timestamp().Do(te => OnElement(te)).Select(x => x.Value));
        }

        private Action<Timestamped<IElement>> _onTimestampedElement;

        public TimestampedDoStrategy OnElement(Action<Timestamped<IElement>> onTimestampedElement)
        {
            if (onTimestampedElement == null) throw new ArgumentNullException("onTimestampedElement");
            _onTimestampedElement = onTimestampedElement;
            return this;
        }

        private void OnElement(Timestamped<IElement> element)
        {
            if (_onTimestampedElement != null) {
                _onTimestampedElement(element);
            }
        }
    }
}
