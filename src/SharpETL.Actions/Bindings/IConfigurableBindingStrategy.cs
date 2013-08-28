using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Actions.Binding
{
    public interface IConfigurableBindingStrategy : IBindingStrategy
    {
        IConfigurableBindingStrategy OnBind(Func<IObservable<IElement>, IObservable<IElement>> onBind);
        IConfigurableBindingStrategy OnElement(Func<IElement, IObserver<IElement>, bool> onElement);
        IConfigurableBindingStrategy OnCompleted(Func<IObserver<IElement>, bool> onCompleted);
        IConfigurableBindingStrategy OnError(Func<Exception, IObserver<IElement>, bool> onError);
        IConfigurableBindingStrategy OnFinally(Action onFinally);
    }
}
