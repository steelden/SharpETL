using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Configuration;

namespace SharpETL.IO.Sources
{
    public sealed class NullSource : ISource, IProvideConfigurationData<NullSource>
    {
        private string _id;

        public NullSource(string id)
        {
            _id = id;
        }

        public string Id
        {
            get { return _id; }
        }

        public IEnumerable<int> GetTables()
        {
            return Enumerable.Empty<int>();
        }

        public string GetTableName(int tableIndex)
        {
            return String.Empty;
        }

        public int GetTableIndex(string tableName)
        {
            return 0;
        }

        public IEnumerable<int> GetFields(int tableIndex)
        {
            return Enumerable.Empty<int>();
        }

        public string GetFieldName(int tableIndex, int fieldIndex)
        {
            return String.Empty;
        }

        public int GetFieldIndex(int tableIndex, string fieldName)
        {
            return 0;
        }

        public object[] GetRow(int tableIndex, int rowIndex)
        {
            return null;
        }

        public object[] GetColumn(int tableIndex, int fieldIndex)
        {
            return null;
        }

        public object GetCell(int tableIndex, int rowIndex, int fieldIndex)
        {
            return null;
        }

        public object this[int tableIndex, int rowIndex, int fieldIndex]
        {
            get { return null;}
        }

        public void Close()
        {
        }

        public void Dispose()
        {
            Close();
        }

        public IConfigurationData<NullSource> GetConfiguration()
        {
            return new NullSourceConfigurationData() { Id = this.Id };
        }
    }

    [Serializable]
    public sealed class NullSourceConfigurationData : ConfigurationDataBase<NullSource>
    {
        public override NullSource CreateObject()
        {
            ValidateThrow();
            return new NullSource(Id);
        }

        public override IDictionary<string, object> GetData()
        {
            return new Dictionary<string, object>() { { "Id", Id } };
        }
    }
}
