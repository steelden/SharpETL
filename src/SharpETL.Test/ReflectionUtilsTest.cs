using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Utility;
using SharpETL.Configuration;
using SharpETL.Components;
using System.Reflection;

namespace SharpETL.Test
{
    internal interface RUT_TestInterface { }
    internal class RUT_TestClass : RUT_TestInterface { }

    [TestClass]
    public class ReflectionUtilsTest
    {
        [TestMethod]
        public void ReflectionUtil_correctly_enumerating_assemblies()
        {
            var all = ReflectionUtils.GetAssemblies(true);
            foreach (var assembly in all) {
                Assert.IsFalse(assembly.FullName.StartsWith("System."));
                Assert.IsFalse(assembly.FullName.StartsWith("System,"));
            }
        }

        [TestMethod]
        public void ReflectionUtil_correctly_finds_derived_types()
        {
            var assemblies = new Assembly[] { Assembly.GetExecutingAssembly() };
            var interfaces = new Type[] { typeof(IConfigurationData<object>) };
            var types = ReflectionUtils.GetTypesImplementingInterfaces(assemblies, interfaces);
            Assert.IsTrue(types.Count() > 0);
            Type t = types.FirstOrDefault(x => x == typeof(TestConfigurationData));
            Assert.IsNotNull(t);
        }

        [TestMethod]
        public void ReflectionUtils_correctly_finds_all_derived_types()
        {
            var assemblies = ReflectionUtils.GetAssemblies(true);
            var interfaces = new Type[] { typeof(RUT_TestInterface) };
            var types = ReflectionUtils.GetTypesImplementingInterfaces(assemblies, interfaces);
            Assert.AreEqual(1, types.Count());
            Assert.AreEqual(typeof(RUT_TestClass), types.First());
        }
    }
}
