using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.IO.Readers;
using SharpETL.IO.Readers.Ascii;
using SharpETL.IO.Readers.Dbf;
using SharpETL.IO.Readers.Xls;

namespace SharpETL.IO.Test
{
    [TestClass]
    public class ReaderFactoryTest
    {
        private IReaderFactory _factory;

        public ReaderFactoryTest()
        {
            _factory = new ReaderFactory();
        }

        [TestMethod]
        public void ReaderFactory_CreateReader_works()
        {
            string name = "test1.csv";
            var reader = _factory.CreateReader(ReaderType.Ascii, name);
            Assert.IsNotNull(reader);
            Assert.IsInstanceOfType(reader, typeof(AsciiReader));
            Assert.AreEqual(name, reader.Options.FilePath);
        }

        [TestMethod]
        public void ReaderFactory_CreateReaderByExtension_works()
        {
            Dictionary<string, Type> checkList = new Dictionary<string, Type>() {
                { "test.csv", typeof(AsciiReader) },
                { "test.xls", typeof(XlsReader) },
                { "test.xlsx", typeof(XlsReader) },
                { "test.dbf", typeof(DbfReader) }
            };

            foreach (var item in checkList) {
                var reader = _factory.CreateReaderByExtension(item.Key);
                Assert.IsNotNull(reader);
                Assert.IsInstanceOfType(reader, item.Value);
            }
        }

        [TestMethod]
        public void ReaderFactory_CreateDirectoryReader_works()
        {
            var all = _factory.CreateDirectoryReader(XlsReaderTest.TEST_XLS_PATH);
            var filtered = _factory.CreateDirectoryReader(XlsReaderTest.TEST_XLS_PATH, @"^.*\.xlsx$");
            Assert.AreEqual(5, all.Count());
            Assert.AreEqual(2, filtered.Count());
        }
    }
}
