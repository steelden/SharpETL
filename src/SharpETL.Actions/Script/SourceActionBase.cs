using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Utility;

namespace SharpETL.Actions.Script
{
    public abstract class SourceActionBase : BindedActionBase, ISourceAction
    {
        private ISource _source;
        private IContext _sourceContext;

        public ISource Source { get { return _source; } }
        public IContext SourceContext { get { return _sourceContext; } }

        public SourceActionBase(string id, ISource source)
            : base(id)
        {
            SetSource(source);
        }

        public void SetSource(ISource source)
        {
            if (source == null) throw new ArgumentNullException("source");
            _source = source;
        }

        public void SetSource(ISource source, IContext sourceContext)
        {
            SetSource(source);
            SetSourceContext(sourceContext);
        }

        public void SetSourceContext(IContext sourceContext)
        {
            if (sourceContext == null) throw new ArgumentNullException("sourceContext");
            _sourceContext = sourceContext;
        }

        protected override void OnSetEngine()
        {
            base.OnSetEngine();
            SetSourceContext(Engine.Configuration.GetContextForId(Source.Id));
        }
    }
}
