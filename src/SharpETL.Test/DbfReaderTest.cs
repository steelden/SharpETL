using System.Data;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.IO.Readers;
using SharpETL.IO.Readers.Dbf;

namespace SharpETL.IO.Test
{
    [TestClass]
    public class DbfReaderTest
    {
        public static readonly string TEST_DBF_PATH = Path.GetFullPath(@"..\..\..\SharpETL.Test\TestData\DbfData");
        public static readonly string TEST_DBF_NAME1 = "test1.dbf";
        public static readonly string TEST_DBF_NAME2 = "test2.dbf";

        public static readonly int TEST_DBF1_COLUMN_COUNT = 31;
        public static readonly int TEST_DBF1_ROW_COUNT = 0;
        public static readonly int TEST_DBF2_COLUMN_COUNT = 1;
        public static readonly int TEST_DBF2_ROW_COUNT = 145;

        private IReaderFactory _factory;

        public DbfReaderTest()
        {
            _factory = new ReaderFactory();
        }

        private IFormatReader<DbfReaderOptions> GetReader(string filename)
        {
            return _factory.CreateDbfReader(Path.Combine(TEST_DBF_PATH, filename));
        }
        
        [TestMethod]
        public void DbfReader_correctly_reads_dbf1()
        {
            var reader = GetReader(TEST_DBF_NAME1);
            DataSet ds = reader.ToDataSet();
            Assert.IsNotNull(ds);
            Assert.AreEqual(ds.Tables[0].Columns.Count, TEST_DBF1_COLUMN_COUNT);
            Assert.AreEqual(ds.Tables[0].Rows.Count, TEST_DBF1_ROW_COUNT);
        }

        [TestMethod]
        public void DbfReader_correctly_reads_dbf2()
        {
            var reader = GetReader(TEST_DBF_NAME2);
            DataSet ds = reader.ToDataSet();
            Assert.IsNotNull(ds);
            Assert.AreEqual(ds.Tables[0].Columns.Count, TEST_DBF2_COLUMN_COUNT);
            Assert.AreEqual(ds.Tables[0].Rows.Count, TEST_DBF2_ROW_COUNT);
        }
    }
}
