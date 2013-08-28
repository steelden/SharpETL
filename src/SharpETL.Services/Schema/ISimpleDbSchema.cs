using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Services.Schema
{
    public interface ISimpleDbSchema
    {
        IEnumerable<string> GetTables();
        IEnumerable<SchemaFieldItem> GetTableSchema(string tableName);
        IDictionary<string, SchemaFieldItem> GetTableSchemaDictionary(string tableName);
        SchemaFieldItem GetFieldSchema(string tableName, string fieldName);
    }
}
