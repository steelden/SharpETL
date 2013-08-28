using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;
using SharpETL.Components;
using SharpETL.Services;

namespace SharpETL.Actions.Script
{
    public interface IScriptAction : ISourceAction
    {
        ILoggerService Log { get; }
        IScript Script { get; }
        IContext ScriptContext { get; }
        void SetScript(IScript script);
        void SetScript(IScript script, IContext scriptContext);
        void SetScriptContext(IContext scriptContext);
    }
}
