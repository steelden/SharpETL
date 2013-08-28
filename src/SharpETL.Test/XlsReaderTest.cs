using System;
using System.Data;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.IO.Readers;
using SharpETL.IO.Readers.Xls;

namespace SharpETL.IO.Test
{
    [TestClass]
    public class XlsReaderTest
    {
        private IReaderFactory _factory;

        public static readonly string TEST_XLS_PATH = Path.GetFullPath(@"..\..\..\SharpETL.Test\TestData\XlsData");
        public static readonly string TEST_XLS_NAME = "test1.xls";
        public static readonly string TEST_XLSX_NAME = "test1.xlsx";
        public static readonly string TEST_XLSM_NAME = "test1.xlsm";

        public XlsReaderTest()
        {
            _factory = new ReaderFactory();
        }

        private IFormatReader<XlsReaderOptions> GetReader(string filename)
        {
            return _factory.CreateXlsReader(Path.Combine(TEST_XLS_PATH, filename));
        }

        private DataSet GetDataSet(string filename, Action<XlsReaderOptions> configureOptions = null)
        {
            var reader = GetReader(filename);
            if (configureOptions != null) {
                configureOptions(reader.Options);
            }
            DataSet ds = reader.ToDataSet();
            Assert.IsNotNull(ds);
            return ds;
        }

        private void TestXlsOptions(string filename)
        {
            DataSet ds = GetDataSet(filename, o => {
                o.CommentSymbol = "!";
                o.UseSheetNameAsTableName = true;
                o.UseFirstRowAsHeader = true;
            });
            Assert.AreEqual(3, ds.Tables.Count);
            Assert.AreEqual("numtest1", ds.Tables[0].TableName);
            Assert.AreEqual(6, ds.Tables[0].Columns.Count);
            Assert.AreEqual("test1", ds.Tables[0].Columns[0].ColumnName);
            Assert.AreEqual(10, ds.Tables[0].Rows.Count);
        }

        [TestMethod]
        public void XlsReader_ToDataSet_works_xls()
        {
            DataSet ds = GetDataSet(TEST_XLS_NAME);
            Assert.AreEqual(5, ds.Tables.Count);
        }

        [TestMethod]
        public void XlsReader_ToDataSet_works_xlsx()
        {
            DataSet ds = GetDataSet(TEST_XLSX_NAME);
            Assert.AreEqual(5, ds.Tables.Count);
        }

        [TestMethod]
        public void XlsReader_filter_by_sheet_name_works()
        {
            DataSet ds = GetDataSet(TEST_XLS_NAME, o => o.SheetNames = "numtest1, !hidden1");
            Assert.AreEqual(2, ds.Tables.Count);
        }

        [TestMethod]
        public void XlsReader_options_works_xls()
        {
            TestXlsOptions(TEST_XLS_NAME);
        }

        [TestMethod]
        public void XlsReader_options_works_xlsx()
        {
            TestXlsOptions(TEST_XLSX_NAME);
        }

        [TestMethod]
        public void XlsReader_correctly_reads_date_fields_xls()
        {
            DataSet ds = GetDataSet(TEST_XLS_NAME);
            object[] row = ds.Tables[2].Rows[1].ItemArray;
            Assert.IsNotNull(row);
            for (int i = 0; i < row.Length; ++i) {
                Assert.IsInstanceOfType(row[i], typeof(DateTime));
            }
        }
    }
}
