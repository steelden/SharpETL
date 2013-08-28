using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.Dynamic;
using System.Linq.Expressions;

namespace SharpETL.Scripts
{
    public sealed class SharpScriptException : Exception
    {
        public SharpScriptException(CompilerResults results)
            : base(GetExceptionMessage(results))
        {
        }
        public SharpScriptException(Exception inner)
            : base("SharpScriptException", inner)
        {
        }
        public static string GetExceptionMessage(CompilerResults results)
        {
            var errors = results.Errors.Cast<CompilerError>().Select(x => x.ToString());
            return String.Join(Environment.NewLine, errors);
        }
    }

    public sealed class SharpScript : ScriptBase
    {
        private const string CODE_TEMPLATE = @"using System; using System.Collections.Generic; using System.Linq; using System.Text; using SharpETL.Components; namespace UserScript {{ public class UserClass {{ {0} }} }}";

        private Func<IAction, IElement, IEnumerable<IElement>> _onElement;
        private Func<IAction, IEnumerable<IElement>> _onCompleted;
        private Func<IAction, Exception, IEnumerable<IElement>> _onError;
        private Action<IAction> _onFinally;

        private readonly Func<IAction, IElement, IEnumerable<IElement>> _onElementDefault = (a, e) => null;
        private readonly Func<IAction, IEnumerable<IElement>> _onCompletedDefault = a => null;
        private readonly Func<IAction, Exception, IEnumerable<IElement>> _onErrorDefault = (a, ex) => null;
        private readonly Action<IAction> _onFinallyDefault = a => { };

        public SharpScript(string id, string text)
            : base(id, String.Format(CODE_TEMPLATE, text))
        {
            var provider = new CSharpCodeProvider();
            var compilerParams = new CompilerParameters() { GenerateInMemory = true };
            AddReferences(compilerParams);
            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, Text);
            if (results.Errors.HasErrors) {
                throw new SharpScriptException(results);
            }
            var assembly = results.CompiledAssembly;
            var userType = assembly.GetType("UserScript.UserClass");
            var instance = Activator.CreateInstance(userType);
            _onElement = (Func<IAction, IElement, IEnumerable<IElement>>)CreateDelegate(instance, userType.GetMethod("OnElement")) ?? _onElementDefault;
            _onCompleted = (Func<IAction, IEnumerable<IElement>>)CreateDelegate(instance, userType.GetMethod("OnCompleted")) ?? _onCompletedDefault;
            _onError = (Func<IAction, Exception, IEnumerable<IElement>>)CreateDelegate(instance, userType.GetMethod("OnError")) ?? _onErrorDefault;
            _onFinally = (Action<IAction>)CreateDelegate(instance, userType.GetMethod("OnFinally")) ?? _onFinallyDefault;
        }

        public SharpScript(string id,
            Func<IAction, IElement, IEnumerable<IElement>> onElement,
            Func<IAction, IEnumerable<IElement>> onCompleted,
            Func<IAction, Exception, IEnumerable<IElement>> onError,
            Action<IAction> onFinally)
            : base(id, "")
        {
            _onElement = onElement ?? _onElementDefault;
            _onCompleted = onCompleted ?? _onCompletedDefault;
            _onError = onError ?? _onErrorDefault;
            _onFinally = onFinally ?? _onFinallyDefault;
        }

        private void AddReferences(CompilerParameters p)
        {
            p.ReferencedAssemblies.Add("System.dll");
            p.ReferencedAssemblies.Add("System.Core.dll");
            string name = Assembly.GetExecutingAssembly().Location;
            p.ReferencedAssemblies.Add(name);
            p.ReferencedAssemblies.Add(name.Replace(".Scripts.", ".Interfaces."));
            p.ReferencedAssemblies.Add(name.Replace(".Scripts.", ".Core."));
        }

        private Delegate CreateDelegate(object instance, MethodInfo info)
        {
            if (info == null) return null;
            var parameters = info.GetParameters().Select(x => x.ParameterType).Concat(new[] { info.ReturnType });
            return Delegate.CreateDelegate(Expression.GetDelegateType(parameters.ToArray()), instance, info);
        }

        public override IEnumerable<IElement> OnElement(IAction action, IElement element)
        {
            IEnumerable<IElement> result = null;
            if (_onElement != null) {
                result = _onElement(action, element);
            }
            return result ?? Enumerable.Empty<IElement>();
        }

        public override IEnumerable<IElement> OnCompleted(IAction action)
        {
            IEnumerable<IElement> result = null;
            if (_onCompleted != null) {
                result = _onCompleted(action);
            }
            return result ?? Enumerable.Empty<IElement>();
        }

        public override IEnumerable<IElement> OnError(IAction action, Exception exception)
        {
            IEnumerable<IElement> result = null;
            if (_onError != null) {
                result = _onError(action, exception);
            }
            return result ?? Enumerable.Empty<IElement>();
        }

        public override void OnFinally(IAction action)
        {
            if (_onFinally != null) {
                _onFinally(action);
            }
        }

        public override Exception GetDetailedException(Exception ex)
        {
            return new SharpScriptException(ex);
        }
    }
}
