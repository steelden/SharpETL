using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Configuration;
using SharpETL.Components;

namespace SharpETL.Test
{
    internal class TestConfigurationData : ConfigurationDataBase<IAction>
    {
        public override IAction CreateObject()
        {
            return null;
        }

        public override IDictionary<string, object> GetData()
        {
            return new Dictionary<string, object>() { { "Id", Id } };
        }
    }

    [TestClass]
    public class ConfigurationDataTest
    {
        [TestMethod]
        public void ConfigurationData_correctly_calls_CreateObject()
        {
            string id = "123";
            IConfigurationData<IAction> data = new TestConfigurationData() { Id = id };
            Assert.AreEqual(id, data.Id);
            Assert.IsNull(data.CreateObject());
        }

        [TestMethod]
        public void ConfigurationData_sets_correct_name()
        {
            IConfigurationData<IAction> data = new TestConfigurationData() { Id = "123" };
            Assert.AreEqual("Test", data.GetName());
        }

        [TestMethod]
        public void ConfigurationData_returns_correct_data()
        {
            IConfigurationData<IAction> data = new TestConfigurationData() { Id = "123" };
            IDictionary<string, object> dictionaryData = data.GetData();
            Assert.IsNotNull(dictionaryData);
            Assert.IsTrue(dictionaryData.ContainsKey("Id"));
            Assert.AreEqual("123", dictionaryData["Id"]);
        }

        [TestMethod]
        public void ConfigurationData_correctly_validate()
        {
            IConfigurationData<IAction> data = new TestConfigurationData() { Id = "123" };
            Assert.IsTrue(data.ValidateData());
            data = new TestConfigurationData() { Id = null };
            Assert.IsFalse(data.ValidateData());
        }

        [TestMethod]
        public void ConfigurationData_returns_correct_xml()
        {
            IConfigurationData<IAction> data = new TestConfigurationData() { Id = "123" };
            System.Xml.Linq.XElement element = data.GetXml();
            Assert.AreEqual(@"<Test Id=""123"" />", element.ToString());
        }
    }
}
