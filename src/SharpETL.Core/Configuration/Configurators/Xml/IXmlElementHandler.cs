using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SharpETL.Configuration.Configurators.Xml
{
    public interface IXmlElementHandler
    {
        string ElementName { get; }
        Type ItemType { get; }
        Type ItemConfigurationDataType { get; }
        object ItemLookup(Type expectedType, string id);
        void SetContainer(IXmlElementHandlersContainer container);
        void HandleElement(XElement element);
        void GetResults(IMutableEngineConfiguration config);
    }
}
