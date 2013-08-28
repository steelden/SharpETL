using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.IO.Readers;

namespace SharpETL.IO.Test
{
    [TestClass]
    public class XlsDirectoryReaderTest
    {
        private IReaderFactory _factory;

        public XlsDirectoryReaderTest()
        {
            _factory = new ReaderFactory();
        }

        [TestMethod]
        public void XlsDirectoryReader_works_with_pattern()
        {
            IEnumerable<IFormatReader> files = _factory.CreateDirectoryReader(XlsReaderTest.TEST_XLS_PATH);
            IEnumerable<IFormatReader> filteredFiles = _factory.CreateDirectoryReader(XlsReaderTest.TEST_XLS_PATH, @"^.*\.(xls|xlsm)$");
            Assert.AreEqual(5, files.Count());
            Assert.AreEqual(3, filteredFiles.Count());
        }
    }
}
