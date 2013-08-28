using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using SharpETL.Utility;

namespace SharpETL.Services.Schema
{
    public sealed class SimpleDbSchema : ISimpleDbSchema
    {
        private IEnumerable<SchemaFieldItem> _items;
        private WeakCache<IDictionary<string, SchemaFieldItem>> _tableSchemaCache = new WeakCache<IDictionary<string, SchemaFieldItem>>();

        public SimpleDbSchema()
        {
            _items = new List<SchemaFieldItem>();
        }

        public SimpleDbSchema(IEnumerable<SchemaFieldItem> items)
        {
            _items = items;
        }

        public IEnumerable<SchemaFieldItem> GetTableSchema(string tableName)
        {
            return _items.Where(x => x.TableName.Equals(tableName, StringComparison.CurrentCultureIgnoreCase));
        }

        public IDictionary<string, SchemaFieldItem> GetTableSchemaDictionary(string tableName)
        {
            return _tableSchemaCache.Get(tableName.ToLower(), () => {
                return GetTableSchema(tableName).ToDictionary(x => x.FieldName);
            });
        }

        public SchemaFieldItem GetFieldSchema(string tableName, string fieldName)
        {
            return _items.FirstOrDefault(x =>
                x.TableName.Equals(tableName, StringComparison.CurrentCultureIgnoreCase) &&
                x.FieldName.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<string> GetTables()
        {
            return _items.Select(x => x.TableName).Distinct();
        }

        public void ToXml(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SchemaServiceXmlProxy));
            var proxy = new SchemaServiceXmlProxy() { Fields = new List<SchemaFieldItem>(_items) };
            serializer.Serialize(stream, proxy);
        }

        public static ISimpleDbSchema FromXml(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SchemaServiceXmlProxy));
            var proxy = (SchemaServiceXmlProxy)serializer.Deserialize(stream);
            return new SimpleDbSchema(proxy.Fields);
        }

        public static ISimpleDbSchema FromXml(string xml)
        {
            var bytes = Encoding.Default.GetBytes(xml);
            using (var stream = new MemoryStream(bytes)) {
                return FromXml(stream);
            }
        }
    }

    [XmlRoot("SimpleDbSchema")]
    [Serializable]
    public class SchemaServiceXmlProxy
    {
        [XmlArrayItem("Field")]
        public List<SchemaFieldItem> Fields;
    }
}
