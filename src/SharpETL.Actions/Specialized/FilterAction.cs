using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Actions.Binding;

namespace SharpETL.Actions.Specialized
{
    public sealed class FilterAction : ActionBase
    {
        private Func<IElement, bool> _predicate;

        public FilterAction(string id, Func<IElement, bool> predicate)
            : base(id)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            _predicate = predicate;
        }

        protected override IConfigurableBindingStrategy OnCreateStrategy()
        {
            return new WhereStrategy(_predicate);
        }

        protected override void OnConfigureStrategy(IConfigurableBindingStrategy strategy)
        {
        }
    }
}
