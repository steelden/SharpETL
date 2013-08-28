using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using System.Reactive.Linq;

namespace SharpETL.Actions.Binding
{
    public sealed class NullStrategy : BindingStrategy
    {
        protected override IObservable<IElement> OnBind(IObservable<IElement> input)
        {
            return base.OnBind(input.IgnoreElements());
        }
    }
}
