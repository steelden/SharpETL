using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Planning;
using SharpETL.Utility;

namespace SharpETL.Configuration
{
    public interface IEngineConfiguration
    {
        IEngine Engine { get; }
        IServiceResolver ServiceResolver { get; }
        Func<IContext> ContextProvider { get; }
        Func<IPlan> PlanProvider { get; }

        IEnumerable<IAction> Actions { get; }
        IEnumerable<ILink> Links { get; }

        IContext GetContextForId(string id);
    }
}
