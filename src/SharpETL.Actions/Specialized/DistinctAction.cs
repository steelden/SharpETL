using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Actions.Binding;
using SharpETL.Components;

namespace SharpETL.Actions.Specialized
{
    public sealed class DistinctAction : ActionBase
    {
        public DistinctAction(string id)
            : base(id)
        {
        }

        protected override IConfigurableBindingStrategy OnCreateStrategy()
        {
            return new DistinctStrategy(new ElementComparer());
        }

        protected override void OnConfigureStrategy(IConfigurableBindingStrategy strategy)
        {
        }

        private class ElementComparer : IEqualityComparer<IElement>
        {
            public bool Equals(IElement x, IElement y)
            {
                if (!x.Name.Equals(y.Name)) return false;
                if (!x.Id.Equals(y.Id)) return false;
                return true;
            }

            public int GetHashCode(IElement obj)
            {
                unchecked {
                    int hash = 17;
                    hash = 31 * hash + obj.Name.GetHashCode();
                    hash = 31 * hash + obj.Id.GetHashCode();
                    return hash;
                }
            }
        }
    }
}
