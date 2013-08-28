using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SharpETL.Components
{
    public sealed class DataSetSource : ISource
    {
        private DataSet _dataset;
        private bool _fixDbNull;

        public DataSetSource(DataSet dataset, bool fixDbNull = true)
        {
            if (dataset == null) throw new ArgumentNullException("reader");
            _dataset = dataset;
            _fixDbNull = fixDbNull;
        }

        private void ThrowIfDataSetIsNull()
        {
            if (_dataset == null) throw new InvalidOperationException("DataSet is already closed.");
        }

        private DataSet DataSet
        {
            get { ThrowIfDataSetIsNull(); return _dataset; }
        }

        public string Id
        {
            get { return DataSet.DataSetName; }
        }

        public IEnumerable<int> GetTables()
        {
            return Enumerable.Range(0, DataSet.Tables.Count);
        }

        public string GetTableName(int tableIndex)
        {
            return DataSet.Tables[tableIndex].TableName;
        }

        public int GetTableIndex(string tableName)
        {
            return DataSet.Tables.IndexOf(tableName);
        }

        public IEnumerable<int> GetFields(int tableIndex)
        {
            return Enumerable.Range(0, DataSet.Tables[tableIndex].Columns.Count);
        }

        public string GetFieldName(int tableIndex, int fieldIndex)
        {
            return DataSet.Tables[tableIndex].Columns[fieldIndex].Caption;
        }

        public int GetFieldIndex(int tableIndex, string fieldName)
        {
            return DataSet.Tables[tableIndex].Columns.IndexOf(fieldName);
        }

        public object[] GetRow(int tableIndex, int rowIndex)
        {
            DataTable table = DataSet.Tables[tableIndex];
            return table.Rows.Count > rowIndex ? FixDBNull(table.Rows[rowIndex].ItemArray) : null;
        }

        public object[] GetColumn(int tableIndex, int fieldIndex)
        {
            DataTable table = DataSet.Tables[tableIndex];
            if (fieldIndex >= table.Columns.Count) return null;
            return FixDBNull(table.AsEnumerable().Select(x => x.Field<object>(fieldIndex)).ToArray());
        }

        public object GetCell(int tableIndex, int rowIndex, int fieldIndex)
        {
            DataTable table = DataSet.Tables[tableIndex];
            return table.Rows.Count > rowIndex ? FixDBNull(table.Rows[rowIndex].ItemArray[fieldIndex]) : null;
        }

        public object this[int tableIndex, int rowIndex, int fieldIndex]
        {
            get { return GetCell(tableIndex, rowIndex, fieldIndex); }
        }

        private object FixDBNull(object value)
        {
            return value == DBNull.Value ? null : value;
        }

        private object[] FixDBNull(object[] values)
        {
            object[] result = values;
            if (_fixDbNull) {
                result = new object[values.Length];
                for (int i = 0; i < values.Length; ++i) {
                    result[i] = FixDBNull(values[i]);
                }
            }
            return result;
        }

        public void Close()
        {
            _dataset = null;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
