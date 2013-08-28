using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Utility;

namespace SharpETL.Configuration.Configurators.Xml
{
    public class ScriptsElementHandler : XmlElementHandlerBase<IScript>
    {
        public override string ElementName { get { return "Scripts"; } }

        protected override string GetItemId(IScript item)
        {
            return item.Id;
        }

        protected override void ProcessResults(IMutableEngineConfiguration config, IEnumerable<IScript> results)
        {
            // do nothing
        }
    }
}
