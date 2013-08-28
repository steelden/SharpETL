using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;
using System.Collections;
using System.Xml.Linq;

namespace SharpETL.Configuration.Configurators.Xml
{
    public sealed class XmlElementHandlersContainer : IXmlElementHandlersContainer
    {
        private Lazy<ConfigurationDataCollector> _collector;
        private IDictionary<string, IXmlElementHandler> _handlers;

        public XmlElementHandlersContainer()
        {
            _collector = new Lazy<ConfigurationDataCollector>(() => new ConfigurationDataCollector(_handlers.Values.Select(x => x.ItemConfigurationDataType)));
            _handlers = new Dictionary<string, IXmlElementHandler>(StringComparer.OrdinalIgnoreCase);
        }

        public void Add<T>() where T : IXmlElementHandler, new()
        {
            Add(new T());
        }

        public void Add(IXmlElementHandler handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            handler.SetContainer(this);
            _handlers[handler.ElementName] = handler;
        }

        public ConfigurationDataCollector Collector { get { return _collector.Value; } }

        public object ItemLookup(Type expectedType, string id)
        {
            foreach (var handler in _handlers.Values) {
                object result = handler.ItemLookup(expectedType, id);
                if (result != null) return result;
            }
            return null;
        }

        public bool HandleElement(XElement element)
        {
            string name = element.Name.LocalName;
            if (_handlers.ContainsKey(name)) {
                _handlers[name].HandleElement(element);
                return true;
            }
            return false;
        }

        public void GetResults(IMutableEngineConfiguration config)
        {
            foreach (var handler in _handlers.Values) {
                handler.GetResults(config);
            }
        }

        public IEnumerator<IXmlElementHandler> GetEnumerator()
        {
            return _handlers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_handlers.Values).GetEnumerator();
        }
    }
}
