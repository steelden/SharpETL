using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SharpETL.Utility;

namespace SharpETL.Configuration.Configurators.Xml
{
    public abstract class XmlElementHandlerBase<T> : IXmlElementHandler where T : class
    {
        private Lazy<ConfigurationDataCollector> _collector;
        private IDictionary<string, T> _items;
        private Func<Type, string, object> _globalLookup;

        public XmlElementHandlerBase()
        {
            _collector = new Lazy<ConfigurationDataCollector>(() => new ConfigurationDataCollector(ItemConfigurationDataType));
            _globalLookup = ItemLookup;
            _items = new Dictionary<string, T>();
        }

        public abstract string ElementName { get; }
        protected abstract string GetItemId(T item);
        protected abstract void ProcessResults(IMutableEngineConfiguration config, IEnumerable<T> results);

        public Type ItemType { get { return typeof(T); } }
        public Type ItemConfigurationDataType { get { return typeof(IConfigurationData<T>); } }

        public void GetResults(IMutableEngineConfiguration config)
        {
            ProcessResults(config, _items.Values);
        }

        public object ItemLookup(Type expectedType, string id)
        {
            T result;
            if (expectedType.IsAssignableFrom(ItemType) && _items.TryGetValue(id, out result)) {
                return result;
            }
            return null;
        }

        public void SetContainer(IXmlElementHandlersContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _collector = new Lazy<ConfigurationDataCollector>(() => container.Collector);
            _globalLookup = container.ItemLookup;
        }

        public void HandleElement(XElement ancestor)
        {
            if (ancestor == null) throw new ArgumentNullException("ancestor");
            if (!ElementName.Equals(ancestor.Name.LocalName, StringComparison.OrdinalIgnoreCase)) {
                throw new XmlConfigurationException(ancestor, "Unexpected element '{0}' (expecting '{1}').",
                    ancestor.Name.LocalName, ElementName);
            }

            IList<T> result = new List<T>();
            foreach (XElement element in ancestor.Elements()) {
                string typeName = element.Name.LocalName;
                if (!_collector.Value.Contains(typeName)) throw new XmlConfigurationException(element, "Unknown element");

                ConfigurationTypeInfo typeInfo = _collector.Value.GetTypeInfo(typeName);
                IConfigurationData<T> data = typeInfo.CreateInstance<IConfigurationData<T>>();

                var attributes = element.Attributes().Select(x => new KeyValuePair<string, string>(x.Name.LocalName, x.Value));
                var subelements = element.Elements().Select(x => new KeyValuePair<string, string>(x.Name.LocalName, x.Value));

                foreach (var kv in attributes.Concat(subelements)) {
                    SetPropertyValue(element, typeInfo, data, kv.Key, kv.Value);
                }

                T item = data.CreateObject();
                string id = GetItemId(item);
                if (_items.ContainsKey(id)) throw new XmlConfigurationException(element, "Duplicate item (id '{0}') found.", id);
                _items.Add(id, item);
            }
        }

        private void SetPropertyValue(XElement element, ConfigurationTypeInfo typeInfo, IConfigurationData<T> data, string propertyName, string propertyValue)
        {
            ConfigurationPropertyInfo propertyInfo = typeInfo.GetPropertyInfo(propertyName);
            if (propertyInfo == null) throw new XmlConfigurationException(element, "Unknown property '{0}'", propertyName);

            object value = ConvertFromString(element, propertyInfo, propertyValue);
            propertyInfo.Property.SetValue(data, value, null);
        }

        private object ConvertFromString(XElement element, ConfigurationPropertyInfo propertyInfo, string value)
        {
            // if target type is string no need to convert
            if (typeof(string).IsAssignableFrom(propertyInfo.PropertyType)) {
                return value;
            }
            // if its convertible from string just convert it and return
            if (propertyInfo.Converter != null && propertyInfo.Converter.CanConvertFrom(typeof(string))) {
                return propertyInfo.Converter.ConvertFromString(value);
            }
            // try id lookup on other handlers
            object result = DoItemLookup(propertyInfo.PropertyType, value);
            if (result != null) {
                if (!propertyInfo.PropertyType.IsAssignableFrom(result.GetType())) {
                    throw new XmlConfigurationException(element, "Id '{0}' lookup found an incompatible result '{1}' (need '{2}').",
                        value, result.GetType().Name, propertyInfo.PropertyType.Name);
                }
                return result;
            }
            throw new XmlConfigurationException(element, "Unable to convert value of property '{0}' to specified type '{1}'",
                propertyInfo.PropertyName, propertyInfo.PropertyType.Name);
        }

        private object DoItemLookup(Type expectedType, string id)
        {
            return (_globalLookup != null ? _globalLookup(expectedType, id) : ItemLookup(expectedType, id));
        }
    }
}
