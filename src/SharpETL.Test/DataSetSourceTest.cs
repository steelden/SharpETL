using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Components;
using System.Data;

namespace SharpETL.Test
{
    [TestClass]
    public class DataSetSourceTest
    {
        private DataSet GetDataSet()
        {
            DataSet ds = new DataSet("testds");
            DataTable dt = ds.Tables.Add("testtable");
            dt.Columns.Add("column0", typeof(int));
            dt.Columns.Add("column1", typeof(string));
            dt.Rows.Add(0, "testrow0");
            dt.Rows.Add(1, null);
            return ds;
        }

        [TestMethod]
        public void DataSetSource_works()
        {
            DataSet ds = GetDataSet();
            DataSetSource dss = new DataSetSource(ds);
            Assert.AreEqual("testds", dss.Id);
        }

        [TestMethod]
        public void DataSetSource_table_methods_works()
        {
            DataSet ds = GetDataSet();
            DataSetSource dss = new DataSetSource(ds);
            int[] tables = dss.GetTables().ToArray();
            Assert.AreEqual(1, tables.Length);
            Assert.AreEqual("testtable", dss.GetTableName(tables[0]));
            Assert.AreEqual(tables[0], dss.GetTableIndex("testtable"));
        }

        [TestMethod]
        public void DataSetSource_field_methods_works()
        {
            DataSet ds = GetDataSet();
            DataSetSource dss = new DataSetSource(ds);
            int table = dss.GetTables().First();

            int[] fields = dss.GetFields(table).ToArray();
            Assert.AreEqual(2, fields.Length);
            Assert.AreEqual("column0", dss.GetFieldName(table, fields[0]));
            Assert.AreEqual("column1", dss.GetFieldName(table, fields[1]));
            Assert.AreEqual(fields[0], dss.GetFieldIndex(table, "column0"));
            Assert.AreEqual(fields[1], dss.GetFieldIndex(table, "column1"));
        }

        [TestMethod]
        public void DataSetSource_get_row_works()
        {
            DataSet ds = GetDataSet();
            DataSetSource dss = new DataSetSource(ds);
            int table = dss.GetTables().First();

            object[] row0 = dss.GetRow(table, 0);
            object[] row1 = dss.GetRow(table, 1);
            object[] row2 = dss.GetRow(table, 2);
            Assert.IsNotNull(row0);
            Assert.IsNotNull(row1);
            Assert.IsNull(row2);
            Assert.AreEqual(2, row0.Length);
            Assert.AreEqual(2, row1.Length);
            Assert.IsInstanceOfType(row0[0], typeof(int));
            Assert.IsInstanceOfType(row0[1], typeof(string));
            Assert.IsInstanceOfType(row1[0], typeof(int));
            Assert.AreEqual(0, row0[0]);
            Assert.AreEqual(1, row1[0]);
            Assert.AreEqual("testrow0", row0[1]);
            Assert.AreEqual(null, row1[1]);
        }

        [TestMethod]
        public void DataSetSource_get_column_works()
        {
            DataSet ds = GetDataSet();
            DataSetSource dss = new DataSetSource(ds);
            int table = dss.GetTables().First();

            object[] col0 = dss.GetColumn(table, 0);
            object[] col1 = dss.GetColumn(table, 1);
            object[] col2 = dss.GetColumn(table, 2);
            Assert.IsNotNull(col0);
            Assert.IsNotNull(col1);
            Assert.IsNull(col2);
            Assert.AreEqual(2, col0.Length);
            Assert.AreEqual(2, col1.Length);
            Assert.IsInstanceOfType(col0[0], typeof(int));
            Assert.IsInstanceOfType(col0[1], typeof(int));
            Assert.IsInstanceOfType(col1[0], typeof(string));
            Assert.AreEqual(0, col0[0]);
            Assert.AreEqual(1, col0[1]);
            Assert.AreEqual("testrow0", col1[0]);
            Assert.AreEqual(null, col1[1]);
        }

        [TestMethod]
        public void DataSetSource_get_cell_works()
        {
            DataSet ds = GetDataSet();
            DataSetSource dss = new DataSetSource(ds);
            int table = dss.GetTables().First();

            object c00 = dss.GetCell(table, 0, 0);
            object c01 = dss.GetCell(table, 0, 1);
            object c10 = dss.GetCell(table, 1, 0);
            object c11 = dss.GetCell(table, 1, 1);
            object c22 = dss.GetCell(table, 2, 2);
            Assert.IsNotNull(c00);
            Assert.IsNotNull(c01);
            Assert.IsNotNull(c10);
            Assert.IsNull(c11);
            Assert.IsNull(c22);
            Assert.IsInstanceOfType(c00, typeof(int));
            Assert.IsInstanceOfType(c01, typeof(string));
            Assert.IsInstanceOfType(c10, typeof(int));
            Assert.AreEqual(0, c00);
            Assert.AreEqual("testrow0", c01);
            Assert.AreEqual(1, c10);
        }

        [TestMethod]
        public void DataSetSource_indexer_works()
        {
            DataSet ds = GetDataSet();
            DataSetSource dss = new DataSetSource(ds);
            int table = dss.GetTables().First();

            object c00 = dss[table, 0, 0];
            object c01 = dss[table, 0, 1];
            object c10 = dss[table, 1, 0];
            object c11 = dss[table, 1, 1];
            object c22 = dss[table, 2, 2];
            Assert.IsNotNull(c00);
            Assert.IsNotNull(c01);
            Assert.IsNotNull(c10);
            Assert.IsNull(c11);
            Assert.IsNull(c22);
            Assert.IsInstanceOfType(c00, typeof(int));
            Assert.IsInstanceOfType(c01, typeof(string));
            Assert.IsInstanceOfType(c10, typeof(int));
            Assert.AreEqual(0, c00);
            Assert.AreEqual("testrow0", c01);
            Assert.AreEqual(1, c10);
        }

        [TestMethod]
        public void DataSetSource_close_and_dispose_works()
        {
            DataSet ds = GetDataSet();
            DataSetSource dss = new DataSetSource(ds);
            dss.Close();
            dss.Dispose();
        }
    }
}
