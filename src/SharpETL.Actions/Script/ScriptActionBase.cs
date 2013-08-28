using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Utility;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Reactive;
using SharpETL.Services;

namespace SharpETL.Actions.Script
{
    public abstract class ScriptActionBase : SourceActionBase, IScriptAction
    {
        private IScript _script;
        private IContext _scriptContext;
        private ILoggerService _logger;

        public IScript Script { get { return _script; } }
        public IContext ScriptContext { get { return _scriptContext; } }
        public ILoggerService Log { get { return _logger; } }

        public ScriptActionBase(string id, ISource source, IScript script)
            : base(id, source)
        {
            SetScript(script);
        }

        public void SetScript(IScript script)
        {
            if (script == null) throw new ArgumentNullException("script");
            _script = script;
        }

        public void SetScript(IScript script, IContext scriptContext)
        {
            SetScript(script);
            SetScriptContext(scriptContext);
        }

        public void SetScriptContext(IContext scriptContext)
        {
            if (scriptContext == null) throw new ArgumentNullException("scriptContext");
            _scriptContext = scriptContext;
        }

        protected override void OnSetEngine()
        {
            base.OnSetEngine();
            SetScriptContext(Engine.Configuration.GetContextForId(Script.Id));
            _logger = Engine.GetService<ILoggerService>();
        }
    }
}
