using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;
using SharpETL.Components;
using SharpETL.Extensions;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;
using System.Xml;
using SharpETL.Configuration.Configurators.Xml;

namespace SharpETL.Configuration.Configurators
{
    public class XmlConfigurationException : Exception
    {
        public XElement Element { get; private set; }

        public XmlConfigurationException(XElement element, string message, params object[] parameters)
            : base(GetMessage(element, message, parameters))
        {
            Element = element;
        }

        private static string GetMessage(XElement element, string message, params object[] parameters)
        {
            string prepared = String.Format(message, parameters);
            return String.Format("Error occured while processing '{0}' element: {1}.", element.Name.NamespaceName, prepared);
        }
    }

    public class XmlEngineConfigurator : IMutableEngineConfigurator
    {
        public const string XML_ROOT_ELEMENT_NAME = "Engine";

        private XDocument _document;
        private IXmlElementHandlersContainer _handlers;

        public XmlEngineConfigurator(string path)
        {
            using (Stream stream = File.OpenRead(path)) {
                Initialize(stream);
            }
        }

        public XmlEngineConfigurator(Stream xmlStream)
        {
            Initialize(xmlStream);
        }

        private void Initialize(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            _document = XDocument.Load(stream);
            _handlers = new XmlElementHandlersContainer() {
                new ActionsElementHandler(),
                new LinksElementHandler(),
                new ScriptsElementHandler(),
                new ServicesElementHandler(),
                new SourcesElementHandler()
            };
        }

        public void Configure(IMutableEngineConfiguration config)
        {
            ProcessXml(_document.Root);
            _handlers.GetResults(config);
        }

        private void ProcessXml(XElement root)
        {
            if (!root.Name.LocalName.Equals(XML_ROOT_ELEMENT_NAME, StringComparison.OrdinalIgnoreCase)) {
                throw new XmlConfigurationException(root, "Unexpected root element name '{0}' (expecting '{1}').",
                    root.Name.LocalName, XML_ROOT_ELEMENT_NAME);
            }
            foreach (XElement element in root.Elements()) {
                if (!_handlers.HandleElement(element)) {
                    throw new XmlConfigurationException(element, "Unknown element");
                }
            }
        }
    }
}
