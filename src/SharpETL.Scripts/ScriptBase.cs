using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Scripts
{
    public abstract class ScriptBase : IScript
    {
        private string _id;
        private string _text;

        public string Id { get { return _id; } protected set { _id = value; } }
        public string Text { get { return _text; } protected set { _text = value; } }

        public ScriptBase(string id, string text)
        {
            _id = id;
            _text = text;
        }

        public abstract IEnumerable<IElement> OnElement(IAction action, IElement element);
        public abstract IEnumerable<IElement> OnCompleted(IAction action);
        public abstract IEnumerable<IElement> OnError(IAction action, Exception exception);
        public abstract void OnFinally(IAction action);
        public abstract Exception GetDetailedException(Exception ex);
    }
}
