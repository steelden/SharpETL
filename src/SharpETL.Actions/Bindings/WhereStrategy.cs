using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using SharpETL.Components;

namespace SharpETL.Actions.Binding
{
    public sealed class WhereStrategy : BindingStrategy
    {
        private Func<IElement, bool> _predicate;
        public WhereStrategy(Func<IElement, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            _predicate = predicate;
        }

        protected override IObservable<IElement> OnBind(IObservable<IElement> input)
        {
            return base.OnBind(input.Where(_predicate));
        }
    }
}
