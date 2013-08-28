using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using System.Reactive.Linq;

namespace SharpETL.Actions.Binding
{
    public sealed class DistinctStrategy : BindingStrategy
    {
        private IEqualityComparer<IElement> _comparer;

        public DistinctStrategy(IEqualityComparer<IElement> comparer)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");
            _comparer = comparer;
        }

        protected override IObservable<IElement> OnBind(IObservable<IElement> input)
        {
            return base.OnBind(input.Distinct(_comparer));
        }
    }
}
