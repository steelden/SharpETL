using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;

namespace SharpETL.Components
{
    public interface IAction
    {
        string Id { get; }

        IEngine Engine { get; }

        IContext GlobalContext { get; }
        IContext ActionContext { get; }

        IObservable<IElement> Output { get; }

        void SetEngine(IEngine engine);
        void SetActionContext(IContext actionContext);
        void SetInput(IObservable<IElement> input);
    }
}
