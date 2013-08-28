using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.IO.Readers;

namespace SharpETL.IO.Test
{
    [TestClass]
    public class DbReaderTest
    {
        private IReaderFactory _factory;

        public DbReaderTest()
        {
            _factory = new ReaderFactory();
        }

        [TestMethod]
        public void DbReader_can_read_dbf()
        {
            string connectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=dBASE IV;", DbfReaderTest.TEST_DBF_PATH);
            string query = String.Format("select * from {0}", DbfReaderTest.TEST_DBF_NAME1);
            var reader = _factory.CreateDbReader("testid", connectionString, "TestQuery", query);
            DataSet ds = reader.ToDataSet();
            Assert.IsNotNull(ds);
            Assert.AreEqual(ds.Tables[0].Columns.Count, DbfReaderTest.TEST_DBF1_COLUMN_COUNT);
            Assert.AreEqual(ds.Tables[0].Rows.Count, DbfReaderTest.TEST_DBF1_ROW_COUNT);
        }
    }
}
