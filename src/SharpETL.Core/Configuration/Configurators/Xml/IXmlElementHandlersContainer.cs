using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;
using System.Xml.Linq;

namespace SharpETL.Configuration.Configurators.Xml
{
    public interface IXmlElementHandlersContainer : IEnumerable<IXmlElementHandler>
    {
        ConfigurationDataCollector Collector { get; }
        object ItemLookup(Type expectedType, string id);
        void Add(IXmlElementHandler handler);
        void Add<T>() where T : IXmlElementHandler, new();
        bool HandleElement(XElement element);
        void GetResults(IMutableEngineConfiguration config);
    }
}
