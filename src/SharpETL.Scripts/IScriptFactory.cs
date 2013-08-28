using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Configuration
{
    public interface IScriptFactory
    {
        void InitializePython(IEnumerable<Type> additionalAssemblies = null, IEnumerable<string> includePaths = null);
        IScript CreatePythonScript(string id, string text);
        IScript LoadPythonScript(string id, string path);
        IScript CreateSharpScript(string id, string text);
        IScript CreateSharpScript(string id,
            Func<IAction, IElement, IEnumerable<IElement>> onElement = null,
            Func<IAction, IEnumerable<IElement>> onCompleted = null,
            Func<IAction, Exception, IEnumerable<IElement>> onError = null,
            Action<IAction> onFinally = null);
        IScript LoadSharpScript(string id, string path);
    }
}
