using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.IO.Readers.Db;
using SharpETL.IO.Readers;

namespace SharpETL.IO.Sources
{
    public class DbReaderExSource : ISource
    {
        private readonly string[] NAME_QUERY_SEPARATOR = new string[] { "#" };
        private DataSetSource _source;
        private DbReaderEx _reader;

        public DbReaderExSource(DbReaderEx reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            _reader = reader;
            _source = new DataSetSource(_reader.ToDataSet());
        }

        public int GetTableIndex(string tableName)
        {
            int index = _source.GetTableIndex(tableName);
            if (index < 0) {
                string[] parts = tableName.Split(NAME_QUERY_SEPARATOR, 2, StringSplitOptions.None);
                string name, query;
                if (parts.Length > 1) {
                    name = parts[0];
                    query = parts[1];
                } else {
                    name = tableName;
                    query = tableName;
                }
                _reader.AddQuery(name, query);
                _source = new DataSetSource(_reader.ToDataSet());
                index = _source.GetTableIndex(name);
            }
            return index;
        }

        public string Id { get { return _source.Id; } }
        public IEnumerable<int> GetTables() { return _source.GetTables(); }
        public string GetTableName(int tableIndex) { return _source.GetTableName(tableIndex); }
        public IEnumerable<int> GetFields(int tableIndex) { return _source.GetFields(tableIndex); }
        public string GetFieldName(int tableIndex, int fieldIndex) { return _source.GetFieldName(tableIndex, fieldIndex); }
        public int GetFieldIndex(int tableIndex, string fieldName) { return _source.GetFieldIndex(tableIndex, fieldName); }
        public object[] GetRow(int tableIndex, int rowIndex) { return _source.GetRow(tableIndex, rowIndex); }
        public object[] GetColumn(int tableIndex, int fieldIndex) { return _source.GetColumn(tableIndex, fieldIndex); }
        public object GetCell(int tableIndex, int rowIndex, int fieldIndex) { return _source.GetCell(tableIndex, rowIndex, fieldIndex); }
        public object this[int tableIndex, int rowIndex, int fieldIndex] { get { return _source[tableIndex, rowIndex, fieldIndex]; } }
        public void Close() { _source.Close(); }
        public void Dispose() { _source.Dispose(); }
    }
}
