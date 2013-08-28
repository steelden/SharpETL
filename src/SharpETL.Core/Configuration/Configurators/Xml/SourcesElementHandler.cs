using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Utility;

namespace SharpETL.Configuration.Configurators.Xml
{
    public class SourcesElementHandler : XmlElementHandlerBase<ISource>
    {
        public override string ElementName { get { return "Sources"; } }

        protected override string GetItemId(ISource item)
        {
            return item.Id;
        }

        protected override void ProcessResults(IMutableEngineConfiguration config, IEnumerable<ISource> results)
        {
            // do nothing
        }
    }
}
