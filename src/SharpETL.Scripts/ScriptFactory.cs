using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using SharpETL.Components;
using System.IO;
using SharpETL.Utility;
using Microsoft.Scripting.Hosting;
using SharpETL.Services;

namespace SharpETL.Scripts
{
    public sealed class ScriptFactory : IScriptFactory
    {
        public ScriptFactory()
        {
        }

        private ScriptEngine _pengine;

        public void InitializePython(IEnumerable<Type> additionalAssemblies = null, IEnumerable<string> includePaths = null)
        {
            if (_pengine != null) return;
            _pengine = PythonScript.CreatePythonEngine(additionalAssemblies, includePaths);
        }

        public IScript CreatePythonScript(string id, string text)
        {
            InitializePython();
            return new PythonScript(id, text, _pengine);
        }

        public IScript LoadPythonScript(string id, string path)
        {
            string text = File.ReadAllText(path);
            return CreatePythonScript(id, text);
        }

        public IScript CreateSharpScript(string id, string text)
        {
            return new SharpScript(id, text);
        }

        public IScript LoadSharpScript(string id, string path)
        {
            string text = File.ReadAllText(path);
            return CreateSharpScript(id, text);
        }

        public IScript CreateSharpScript(string id,
            Func<IAction, IElement, IEnumerable<IElement>> onElement = null,
            Func<IAction, IEnumerable<IElement>> onCompleted = null,
            Func<IAction, Exception, IEnumerable<IElement>> onError = null,
            Action<IAction> onFinally = null)
        {
            return new SharpScript(id, onElement, onCompleted, onError, onFinally);
        }
    }
}
