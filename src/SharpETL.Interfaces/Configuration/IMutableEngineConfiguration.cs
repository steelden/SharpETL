using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;
using SharpETL.Planning;
using SharpETL.Components;
using System.IO;

namespace SharpETL.Configuration
{
    public interface IMutableEngineConfiguration : IEngineConfiguration
    {
        void SetServiceResolver(IServiceResolver serviceResolver);
        void SetPlanProvider(Func<IPlan> planProvider);
        void SetContextProvider(Func<IContext> contextProvider);
        void AddAction(IAction action);
        void AddLink(ILink link);
        void AddLink(IAction source, IAction destination);
        void LoadConfiguration(string path);
        void LoadPlugin(string path);
    }
}
