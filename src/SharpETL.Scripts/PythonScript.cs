using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace SharpETL.Scripts
{
    public sealed class PythonScriptException : Exception
    {
        public PythonScriptException(ScriptEngine engine, Exception inner)
            : base(GetExceptionMessage(engine, inner), inner)
        {
        }
        public static string GetExceptionMessage(ScriptEngine engine, Exception ex)
        {
            ExceptionOperations eo = engine.GetService<ExceptionOperations>();
            return eo.FormatException(ex);
        }
    }

    public sealed class PythonScript : ScriptBase
    {
        private ScriptEngine _engine;
        private ScriptScope _scope;
        private ScriptSource _source;
        private CompiledCode _compiled;

        private Func<IAction, IElement, IEnumerable<IElement>> _onElement;
        private Func<IAction, IEnumerable<IElement>> _onCompleted;
        private Func<IAction, Exception, IEnumerable<IElement>> _onError;
        private Action<IAction> _onFinally;

        public PythonScript(string id, string text)
            : this(id, text, CreatePythonEngine())
        {
        }

        public PythonScript(string id, string text, ScriptEngine pythonEngine)
            : base(id, text)
        {
            if (pythonEngine == null) throw new ArgumentNullException("pythonEngine");
            _engine = pythonEngine;
            _scope = _engine.CreateScope();
            try {
                _source = _engine.CreateScriptSourceFromString(Text, SourceCodeKind.Statements);
                _compiled = _source.Compile();
                _compiled.Execute(_scope);
            }
            catch (SyntaxErrorException ex) {
                throw new PythonScriptException(_engine, ex);
            }
            _scope.TryGetVariable("OnElement", out _onElement);
            _scope.TryGetVariable("OnCompleted", out _onCompleted);
            _scope.TryGetVariable("OnError", out _onError);
            _scope.TryGetVariable("OnFinally", out _onFinally);
        }

        public override IEnumerable<IElement> OnElement(IAction action, IElement element)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (element == null) throw new ArgumentNullException("element");
            if (_onElement != null) {
                return _onElement(action, element);
            }
            return Enumerable.Empty<IElement>();
        }

        public override IEnumerable<IElement> OnCompleted(IAction action)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (_onCompleted != null) {
                return _onCompleted(action);
            }
            return Enumerable.Empty<IElement>();
        }

        public override IEnumerable<IElement> OnError(IAction action, Exception exception)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (exception == null) throw new ArgumentNullException("exception");
            if (_onError != null) {
                return _onError(action, exception);
            }
            return Enumerable.Empty<IElement>();
        }

        public override void OnFinally(IAction action)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (_onFinally != null) {
                _onFinally(action);
            }
        }

        public override Exception GetDetailedException(Exception ex)
        {
            return new PythonScriptException(_engine, ex);
        }

        public static ScriptEngine CreatePythonEngine(IEnumerable<Type> additionalAssemblies = null, IEnumerable<string> includePaths = null)
        {
            ScriptEngine pengine = IronPython.Hosting.Python.CreateEngine();

            Type[] defaultTypes = new[] {
                typeof(IElement),                           // SharpETL.Interfaces assembly
                typeof(Element),                            // SharpETL.Core assembly
                typeof(Enumerable),                         // System.Linq assembly
                typeof(SharpETL.Services.ILoggerService),   // SharpETL.Services assembly
                typeof(IronPython.Modules.CTypes)           // IronPython.Modules assembly
            };

            IEnumerable<Type> allTypes = defaultTypes;
            if (additionalAssemblies != null) {
                allTypes = defaultTypes.Concat(additionalAssemblies);
            }

            foreach (Type type in allTypes) {
                pengine.Runtime.LoadAssembly(type.Assembly);
            }

            if (includePaths != null) {
                var paths = pengine.GetSearchPaths();
                foreach (string path in includePaths) {
                    paths.Add(path);
                }
                pengine.SetSearchPaths(paths);
            }

            return pengine;
        }
    }
}
