using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Utility;

namespace SharpETL.Configuration.Configurators.Xml
{
    public class LinksElementHandler : XmlElementHandlerBase<ILink>
    {
        public override string ElementName { get { return "Links"; } }

        protected override string GetItemId(ILink item)
        {
            return String.Format("link_{0}_to_{1}", item.From.Id, item.To.Id);
        }

        protected override void ProcessResults(IMutableEngineConfiguration config, IEnumerable<ILink> results)
        {
            foreach (var link in results) {
                config.AddLink(link);
            }
        }
    }
}
