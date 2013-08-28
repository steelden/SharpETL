using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.IO.Sources
{
    public class DeferredSource : ISource
    {
        private Lazy<ISource> _source;

        public DeferredSource(Func<ISource> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            _source = new Lazy<ISource>(source);
        }

        public string Id
        {
            get { return _source.Value.Id; }
        }

        public IEnumerable<int> GetTables()
        {
            return _source.Value.GetTables();
        }

        public string GetTableName(int tableIndex)
        {
            return _source.Value.GetTableName(tableIndex);
        }

        public int GetTableIndex(string tableName)
        {
            return _source.Value.GetTableIndex(tableName);
        }

        public IEnumerable<int> GetFields(int tableIndex)
        {
            return _source.Value.GetFields(tableIndex);
        }

        public string GetFieldName(int tableIndex, int fieldIndex)
        {
            return _source.Value.GetFieldName(tableIndex, fieldIndex);
        }

        public int GetFieldIndex(int tableIndex, string fieldName)
        {
            return _source.Value.GetFieldIndex(tableIndex, fieldName);
        }

        public object[] GetRow(int tableIndex, int rowIndex)
        {
            return _source.Value.GetRow(tableIndex, rowIndex);
        }

        public object[] GetColumn(int tableIndex, int fieldIndex)
        {
            return _source.Value.GetColumn(tableIndex, fieldIndex);
        }

        public object GetCell(int tableIndex, int rowIndex, int fieldIndex)
        {
            return _source.Value.GetCell(tableIndex, rowIndex, fieldIndex);
        }

        public object this[int tableIndex, int rowIndex, int fieldIndex]
        {
            get { return _source.Value[tableIndex, rowIndex, fieldIndex]; }
        }

        public void Close()
        {
            if (_source.IsValueCreated) {
                _source.Value.Close();
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
