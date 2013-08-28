using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Utility;
using SharpETL.Configuration;
using SharpETL.Components;

namespace SharpETL.Test
{
    [TestClass]
    public class ConfigurationDataPreprocessorTest
    {
        private ConfigurationDataCollector _processor;

        public ConfigurationDataPreprocessorTest()
        {
            _processor = new ConfigurationDataCollector(typeof(IConfigurationData<IAction>));
        }

        [TestMethod]
        public void ConfigurationDataPreprocessor_correctly_stores_type_info()
        {
            // checks for TestConfigurationData type, defined in ConfigurationDataTest.cs
            Assert.IsTrue(_processor.Contains("Test"));
            ConfigurationTypeInfo cti = _processor.GetTypeInfo("Test");
            Assert.IsNotNull(cti);
            Assert.AreEqual(typeof(TestConfigurationData), cti.ObjectType);
        }

        [TestMethod]
        public void ConfigurationDataProcessor_correctly_finds_property_info()
        {
            ConfigurationPropertyInfo cpi = _processor.GetPropertyInfo("Test", "Id");
            Assert.IsNotNull(cpi);
            Assert.AreEqual("Id", cpi.PropertyName);
            Assert.AreEqual(typeof(string), cpi.PropertyType);
        }

        [TestMethod]
        public void ConfigurationDataProcessor_correctly_creates_instance()
        {
            ConfigurationTypeInfo cti = _processor.GetTypeInfo("Test");
            Assert.IsNotNull(cti);
            object data = cti.CreateInstance<object>();
            Assert.IsNotNull(data);
            Assert.IsInstanceOfType(data, typeof(TestConfigurationData));
        }
    }
}
