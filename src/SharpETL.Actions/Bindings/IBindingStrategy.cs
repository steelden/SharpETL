using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Actions.Binding
{
    public interface IBindingStrategy
    {
        IObservable<IElement> DataStream { get; }
        IObserver<IElement> DataSink { get; }
        IObservable<IElement> Bind(IObservable<IElement> input);
    }
}
