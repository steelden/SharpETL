using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Extensions
{
    public static class ExtensionsToISource
    {
        public static IEnumerable<Tuple<int, string>> GetTablesWithNames(this ISource source)
        {
            return source.GetTables().Select(x => new Tuple<int, string>(x, source.GetTableName(x)));
        }
        public static IEnumerable<Tuple<int, string>> GetFieldsWithNames(this ISource source, int tableIndex)
        {
            return source.GetFields(tableIndex).Select(x => new Tuple<int, string>(x, source.GetFieldName(tableIndex, x)));

        }
        public static IEnumerable<Tuple<int, string>> GetFieldsWithNames(this ISource source, string tableName)
        {
            return source.GetFieldsWithNames(source.GetTableIndex(tableName));
        }

        public static IEnumerable<int> GetFields(this ISource source, string tableName)
        {
            return source.GetFields(source.GetTableIndex(tableName));
        }
        public static int GetFieldIndex(this ISource source, string tableName, string fieldName)
        {
            return source.GetFieldIndex(source.GetTableIndex(tableName), fieldName);
        }

        public static object[] GetRow(this ISource source, string tableName, int rowIndex)
        {
            return source.GetRow(source.GetTableIndex(tableName), rowIndex);
        }
        public static T[] GetRow<T>(this ISource source, int tableIndex, int rowIndex)
        {
            return source.GetRow(tableIndex, rowIndex).Cast<T>().ToArray();
        }
        public static T[] GetRow<T>(this ISource source, string tableName, int rowIndex)
        {
            return source.GetRow<T>(source.GetTableIndex(tableName), rowIndex);
        }

        public static object[] GetColumn(this ISource source, string tableName, int fieldIndex)
        {
            return source.GetColumn(source.GetTableIndex(tableName), fieldIndex);
        }
        public static T[] GetColumn<T>(this ISource source, int tableIndex, int fieldIndex)
        {
            return source.GetColumn(tableIndex, fieldIndex).Cast<T>().ToArray();
        }
        public static T[] GetColumn<T>(this ISource source, string tableName, int fieldIndex)
        {
            return source.GetColumn<T>(source.GetTableIndex(tableName), fieldIndex);
        }

        public static object GetCell(this ISource source, string tableName, int rowIndex, int fieldIndex)
        {
            return source.GetCell(source.GetTableIndex(tableName), rowIndex, fieldIndex);
        }
        public static T GetCell<T>(this ISource source, int tableIndex, int rowIndex, int fieldIndex)
        {
            return (T)source.GetCell(tableIndex, rowIndex, fieldIndex);
        }
        public static T GetCell<T>(this ISource source, string tableName, int rowIndex, int fieldIndex)
        {
            return (T)source.GetCell(tableName, rowIndex, fieldIndex);
        }

        public static IEnumerable<object[]> GetAllRows(this ISource source, int tableIndex)
        {
            int i = 0;
            for (object[] data = source.GetRow(tableIndex, 0); data != null; data = source.GetRow(tableIndex, ++i)) {
                yield return data;
            }
        }
        public static IEnumerable<object[]> GetAllRows(this ISource source, string tableName)
        {
            return source.GetAllRows(source.GetTableIndex(tableName));
        }

        public static IEnumerable<object[]> GetAllColumns(this ISource source, int tableIndex)
        {
            int i = 0;
            for (object[] data = source.GetColumn(tableIndex, 0); data != null; data = source.GetColumn(tableIndex, ++i)) {
                yield return data;
            }
        }
        public static IEnumerable<object[]> GetAllColumns(this ISource source, string tableName)
        {
            return source.GetAllColumns(source.GetTableIndex(tableName));
        }
    }
}
