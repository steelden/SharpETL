using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Components;
using SharpETL.IO.Readers;
using SharpETL.IO.Sources;

namespace SharpETL.IO.Test
{
    [TestClass]
    public class SourceFactoryTest
    {
        private SourceFactory _factory;

        public SourceFactoryTest()
        {
            _factory = new SourceFactory(new ReaderFactory());
        }

        [TestMethod]
        public void SourceFactory_can_create_xls_source()
        {
            string path = Path.Combine(XlsReaderTest.TEST_XLS_PATH, XlsReaderTest.TEST_XLS_NAME);
            ISource source = _factory.CreateXlsSource(path);
            Assert.IsNotNull(source);
            Assert.IsInstanceOfType(source, typeof(DeferredSource));            
        }
    }
}
