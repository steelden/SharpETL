using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;
using SharpETL.Components;

namespace SharpETL.Actions.Script
{
    public interface ISourceAction : IAction
    {
        ISource Source { get; }
        IContext SourceContext { get; }
        void SetSource(ISource source);
        void SetSource(ISource source, IContext sourceContext);
        void SetSourceContext(IContext sourceContext);
    }
}
