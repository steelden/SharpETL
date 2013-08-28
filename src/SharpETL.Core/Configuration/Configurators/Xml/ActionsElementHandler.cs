using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Utility;

namespace SharpETL.Configuration.Configurators.Xml
{
    public class ActionsElementHandler : XmlElementHandlerBase<IAction>
    {
        public override string ElementName { get { return "Actions"; } }

        protected override string GetItemId(IAction item)
        {
            return item.Id;
        }

        protected override void ProcessResults(IMutableEngineConfiguration config, IEnumerable<IAction> results)
        {
            foreach (var action in results) {
                config.AddAction(action);
            }
        }
    }
}
