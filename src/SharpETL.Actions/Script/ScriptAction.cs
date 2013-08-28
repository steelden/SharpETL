using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Actions.Binding;

namespace SharpETL.Actions.Script
{
    public class ScriptException : Exception
    {
        public readonly IScriptAction Action;
        public ScriptException(IScriptAction action, Exception ex)
            : base(GetMessage(action, ex), ex)
        {
            Action = action;
        }

        public static string GetMessage(IScriptAction action, Exception ex)
        {
            string actionId = (action != null ? action.Id : "unknown");
            string scriptId = (action != null && action.Script != null ? action.Script.Id : "unknown");
            return String.Format("Exception occured in script '{0}' action '{1}'. See inner exception for details.", scriptId, actionId);
        }
    }

    public sealed class ScriptAction : ScriptActionBase
    {
        public ScriptAction(string id, ISource source, IScript script)
            : base(id, source, script)
        {
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return ActionEvents.All;
        }

        protected override bool OnElement(IElement element, IObserver<IElement> sink)
        {
            var elements = Script.OnElement(this, element);
            EnumerateAndPassErrors(elements, sink);
            return false;
        }

        protected override bool OnCompleted(IObserver<IElement> sink)
        {
            var elements = Script.OnCompleted(this);
            EnumerateAndPassErrors(elements, sink);
            return true;
        }

        protected override bool OnError(Exception exception, IObserver<IElement> sink)
        {
            var elements = Script.OnError(this, exception);
            EnumerateAndPassErrors(elements, sink);
            return true;
        }

        protected override void OnFinally()
        {
            Script.OnFinally(this);
        }

        private void EnumerateAndPassErrors(IEnumerable<IElement> elements, IObserver<IElement> sink)
        {
            var enumerator = elements.GetEnumerator();
            while (true) {
                bool canContinue;
                try {
                    canContinue = enumerator.MoveNext();
                }
                catch (Exception ex) {
                    Exception detailed = Script.GetDetailedException(ex);
                    sink.OnError(new ScriptException(this, detailed));
                    return;
                }
                if (!canContinue) break;
                sink.OnNext(enumerator.Current);
            }
        }
    }
}
