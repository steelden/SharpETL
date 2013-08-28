using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Components
{
    public interface ISource : IDisposable
    {
        string Id { get; }

        IEnumerable<int> GetTables();
        string GetTableName(int tableIndex);
        int GetTableIndex(string tableName);

        IEnumerable<int> GetFields(int tableIndex);
        string GetFieldName(int tableIndex, int fieldIndex);
        int GetFieldIndex(int tableIndex, string fieldName);

        object[] GetRow(int tableIndex, int rowIndex);
        object[] GetColumn(int tableIndex, int fieldIndex);
        object GetCell(int tableIndex, int rowIndex, int fieldIndex);
        object this[int tableIndex, int rowIndex, int fieldIndex] { get; }

        void Close();
    }
}
