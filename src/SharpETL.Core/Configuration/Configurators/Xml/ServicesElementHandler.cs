using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;
using SharpETL.Extensions;

namespace SharpETL.Configuration.Configurators.Xml
{
    public class ServicesElementHandler : XmlElementHandlerBase<IServiceInfo>
    {
        public override string ElementName { get { return "Services"; } }

        protected override string GetItemId(IServiceInfo item)
        {
            return item.InterfaceType.Name;
        }

        protected override void ProcessResults(IMutableEngineConfiguration config, IEnumerable<IServiceInfo> results)
        {
            foreach (var serviceInfo in results) {
                config.ServiceResolver.RegisterService(serviceInfo);
            }
        }
    }
}
