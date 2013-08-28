using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Configuration;
using SharpETL.Ninject;
using SharpETL.Extensions;
using Ninject;

namespace SharpETL.Test
{
    [TestClass]
    public class NinjectServiceResolverTest
    {
        private IServiceResolver GetResolver()
        {
            var kernel = new StandardKernel();
            //kernel.Load<DataTransformEngineNinjectModule>();
            return new NinjectServiceResolver(kernel);
        }

        [TestMethod]
        public void NinjectServiceResolver_registration_and_query_works()
        {
            IServiceResolver resolver = GetResolver();
            resolver.RegisterService<ITestService>(() => new TestService());
            var actual = resolver.QueryService<ITestService>();
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(ITestService));
            Assert.IsInstanceOfType(actual, typeof(TestService));
        }

        [TestMethod]
        public void NinjectServiceResolver_registration_and_get_works()
        {
            IServiceResolver resolver = GetResolver();
            resolver.RegisterService<ITestService>(() => new TestService());
            var actual = resolver.GetService<ITestService>();
            Assert.IsInstanceOfType(actual, typeof(ITestService));
            Assert.IsInstanceOfType(actual, typeof(TestService));
        }

        [TestMethod]
        public void NinjectServiceResolver_non_existant_returns_null()
        {
            IServiceResolver resolver = GetResolver();
            var actual = resolver.QueryService<TestService>();
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void NinjectServiceResolver_store_correct_type()
        {
            IServiceResolver resolver = GetResolver();
            resolver.RegisterService<ITestService>(() => new TestService());
            var actual = resolver.QueryService<TestService>();
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void NinjectServiceResolver_can_store_class_and_interface()
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
