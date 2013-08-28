using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharpETL.Configuration.Configurators;
using SharpETL.Configuration;

namespace SharpETL.Test
{
    [TestClass]
    public class ManualEngineConfiguratorTest
    {
        [TestMethod]
        public void ManualEngineConfigurator_works()
        {
            bool flag = false;
            ManualEngineConfigurator mec = new ManualEngineConfigurator(x => flag = true);
            Mock<IMutableEngineConfiguration> mconfig = new Mock<IMutableEngineConfiguration>();
            mec.Configure(mconfig.Object);
            Assert.IsTrue(flag);
        }
    }
}
