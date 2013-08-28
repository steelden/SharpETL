using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using SharpETL.Components;
using SharpETL.Configuration;
using System.Text.RegularExpressions;

namespace SharpETL.Utility
{
    [Serializable]
    public sealed class ConfigurationTypeInfo
    {
        private IDictionary<string, ConfigurationPropertyInfo> _properties;

        public Type ObjectType { get; private set; }

        public ConfigurationTypeInfo(Type type)
        {
            ObjectType = type;
            _properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanRead && x.CanWrite)
                .Select(x => new ConfigurationPropertyInfo(x))
                .ToDictionary(x => x.PropertyName, StringComparer.OrdinalIgnoreCase);
        }

        public T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(ObjectType);
        }

        public ConfigurationPropertyInfo GetPropertyInfo(string propertyName)
        {
            ConfigurationPropertyInfo result;
            return (_properties.TryGetValue(propertyName, out result) ? result : null);
        }

        public bool Contains(string propertyName)
        {
            return _properties.ContainsKey(propertyName);
        }
    }

    [Serializable]
    public sealed class ConfigurationPropertyInfo
    {
        public string PropertyName { get; private set; }
        public Type PropertyType { get; private set; }
        public TypeConverter Converter { get; private set; }
        public PropertyInfo Property { get; private set; }

        public ConfigurationPropertyInfo(PropertyInfo property)
        {
            Property = property;
            PropertyName = Property.Name;
            PropertyType = Property.PropertyType;
            Converter = TypeDescriptor.GetConverter(PropertyType);
        }
    }

    [Serializable]
    public sealed class ConfigurationDataCollector
    {
        private Regex _nameRE = new Regex(@"^(.+?)(?:ConfigurationData)?$", RegexOptions.Compiled | RegexOptions.Singleline);
        private IDictionary<string, ConfigurationTypeInfo> _dict;

        public ConfigurationDataCollector(Type interfaceType) : this(ReflectionUtils.GetAssemblies(true), interfaceType) { }

        public ConfigurationDataCollector(IEnumerable<Type> interfaces) : this(ReflectionUtils.GetAssemblies(true), interfaces) { }

        public ConfigurationDataCollector(Assembly assembly, Type interfaceType) : this(new Assembly[] { assembly }, interfaceType) { }

        public ConfigurationDataCollector(Assembly assembly, IEnumerable<Type> interfaces) : this(new Assembly[] { assembly }, interfaces) { }

        public ConfigurationDataCollector(IEnumerable<Assembly> assemblies, Type interfaceType) : this(assemblies, new Type[] { interfaceType }) { }

        public ConfigurationDataCollector(IEnumerable<Assembly> assemblies, IEnumerable<Type> interfaces)
        {
            _dict = new Dictionary<string, ConfigurationTypeInfo>(StringComparer.OrdinalIgnoreCase);
            var types = ReflectionUtils.GetTypesImplementingInterfaces(assemblies, interfaces);
            foreach (Type type in types) {
                var cti = new ConfigurationTypeInfo(type);
                Match match = _nameRE.Match(type.Name);
                if (match.Success && match.Groups.Count > 1) {
                    _dict[match.Groups[1].Value] = cti;
                }
            }
        }

        public ConfigurationTypeInfo GetTypeInfo(string typeName)
        {
            ConfigurationTypeInfo cti;
            return (_dict.TryGetValue(typeName, out cti) ? cti : null);
        }

        public ConfigurationPropertyInfo GetPropertyInfo(string typeName, string propertyName)
        {
            ConfigurationTypeInfo cti;
            return (_dict.TryGetValue(typeName, out cti) ? cti.GetPropertyInfo(propertyName) : null);
        }

        public bool Contains(string typeName)
        {
            return _dict.ContainsKey(typeName);
        }
    }
}
