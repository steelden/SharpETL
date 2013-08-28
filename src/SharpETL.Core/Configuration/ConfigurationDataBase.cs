using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace SharpETL.Configuration
{
    [Serializable]
    public sealed class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException(string message) : base(message) { }
        public InvalidConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }

    [Serializable]
    public abstract class ConfigurationDataBase<T> : IConfigurationData<T> where T : class
    {
        private string _id;
        private static Regex _regex = new Regex(@"^(.+?)(?:ConfigurationData)?$", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string _name;

        public string Id { get { return _id; } set { _id = value; } }
        public abstract T CreateObject();
        public abstract IDictionary<string, object> GetData();

        public ConfigurationDataBase()
        {
            string typeName = this.GetType().Name;
            Match match = _regex.Match(typeName);
            _name = (match.Success && match.Groups.Count > 1 ? match.Groups[1].Value : typeName);
        }

        public virtual string GetName() { return _name; }
        public virtual bool ValidateData() { return DoValidate(GetData(), false); }

        public virtual XElement GetXml()
        {
            var result = new XElement(GetName());
            foreach (var item in GetData()) {
                result.Add(new XAttribute(item.Key, item.Value));
            }
            return result;
        }

        protected void ValidateThrow() { DoValidate(GetData(), true); }

        protected bool DoValidate(IEnumerable<KeyValuePair<string, object>> data, bool throwOnError)
        {
            foreach (var item in data) {
                if (CheckIfNull(item.Value, item.Key, throwOnError)) {
                    return false;
                }
            }
            return true;
        }

        protected bool CheckIfNull(object obj, string paramName, bool throwOnError)
        {
            if (Object.ReferenceEquals(obj, null)) {
                if (throwOnError) {
                    string text = String.Format("Operation failed. Parameter '{0}' is empty.", paramName);
                    throw new InvalidConfigurationException(text);
                }
                return true;
            }
            return false;
        }


    }
}
