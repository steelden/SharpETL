using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Configuration;
using System.IO;
using Moq;
using SharpETL.Services;
using SharpETL.Actions.Specialized;
using SharpETL.Services.Loggers;
using SharpETL.Services.Resolvers;
using SharpETL.Configuration.Configurators;

namespace SharpETL.Test
{
    [TestClass]
    public class XmlEngineConfiguratorTest
    {
        private string _xmlconfigEmpty = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<engine id=""TestEngine"">
    <services>
        <nulllogger id=""null_logger_1"" />
    </services>
    <sources>
        <nullsource id=""null_source_1"" />
    </sources>
    <scripts>
    </scripts>
    <actions>
        <nullaction id=""null_action_1"" />
        <nullaction id=""null_action_2"" />
    </actions>
    <links>
        <link from=""null_action_1"" to=""null_action_2"" />
    </links>
</engine>
";

        [TestMethod]
        public void XmlEngineConfigurator_works()
        {
            Type[] testtypes = new Type[] { typeof(NullLogger), typeof(NullAction) };
            IServiceResolver resolver = new SimpleServiceResolver();
            Mock<IMutableEngineConfiguration> mconfig = new Mock<IMutableEngineConfiguration>();
            mconfig.Setup(x => x.ServiceResolver).Returns(resolver);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(_xmlconfigEmpty))) {
                XmlEngineConfigurator xec = new XmlEngineConfigurator(stream);
                xec.Configure(mconfig.Object);
            }
        }
    }
}
