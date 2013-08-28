using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SharpETL.Configuration
{
    public interface IConfigurationData<out T> where T : class
    {
        string Id { get; }
        T CreateObject();
        XElement GetXml();
        bool ValidateData();
        string GetName();
        IDictionary<string, object> GetData();
    }
}
