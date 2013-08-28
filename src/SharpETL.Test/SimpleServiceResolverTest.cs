using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Services.Resolvers;
using SharpETL.Components;
using SharpETL.Configuration;
using SharpETL.Extensions;

namespace SharpETL.Test
{
    internal interface ITestService : IService
    {
        int GetAnswer();
    }

    internal class TestService : ITestService
    {
        public int GetAnswer() { return 42; }
    }

    [TestClass]
    public class SimpleServiceResolverTest
    {
        public IServiceResolver GetResolver()
        {
            return new SimpleServiceResolver();
        }

        [TestMethod]
        public void SimpleServiceResolver_registration_and_query_works()
        {
            IServiceResolver resolver = GetResolver();
            resolver.RegisterService<ITestService>(() => new TestService());
            var actual = resolver.QueryService<ITestService>();
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(ITestService));
            Assert.IsInstanceOfType(actual, typeof(TestService));
        }

        [TestMethod]
        public void SimpleServiceResolver_registration_and_get_works()
        {
            IServiceResolver resolver = GetResolver();
            resolver.RegisterService<ITestService>(() => new TestService());
            var actual = resolver.GetService<ITestService>();
            Assert.IsInstanceOfType(actual, typeof(ITestService));
            Assert.IsInstanceOfType(actual, typeof(TestService));
        }

        [TestMethod]
        public void SimpleServiceResolver_non_existant_returns_null()
        {
            IServiceResolver resolver = GetResolver();
            var actual = resolver.QueryService<TestService>();
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void SimpleServiceResolver_store_correct_type()
        {
            IServiceResolver resolver = GetResolver();
            resolver.RegisterService<ITestService>(() => new TestService());
            var actual = resolver.QueryService<TestService>();
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void SimpleServiceResolver_can_store_class_and_interface()
        {
            IServiceResolver resolver = GetResolver();
            ITestService expectedInterface = new TestService();
            TestService expectedClass = new TestService();
            resolver.RegisterService<ITestService>(() => expectedInterface);
            resolver.RegisterService<TestService>(() => expectedClass);
            ITestService actualInterface = resolver.GetService<ITestService>();
            TestService actualClass = resolver.GetService<TestService>();
            Assert.IsNotNull(actualInterface);
            Assert.IsNotNull(actualClass);
            Assert.AreNotSame(actualInterface, actualClass);
            Assert.AreEqual(expectedInterface, actualInterface);
            Assert.AreEqual(expectedClass, actualClass);
        }
    }
}
