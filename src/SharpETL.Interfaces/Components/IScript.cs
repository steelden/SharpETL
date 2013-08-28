using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Components
{
    public interface IScript
    {
        string Id { get; }
        IEnumerable<IElement> OnElement(IAction action, IElement element);
        IEnumerable<IElement> OnCompleted(IAction action);
        IEnumerable<IElement> OnError(IAction action, Exception exception);
        void OnFinally(IAction action);
        Exception GetDetailedException(Exception ex);
    }
}
