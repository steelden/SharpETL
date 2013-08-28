using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SharpETL.Services.Schema
{
    [Serializable]
    public enum SFIType
    {
        String = 0,
        Number = 1,
        Date = 2
    }

    [XmlRoot("Field")]
    [Serializable]
    public sealed class SchemaFieldItem
    {
        public SchemaFieldItem()
        {
        }

        public int Id;
        public string TableName;
        public string TableDescription;
        public string FieldName;
        public string FieldDescription;
        public string ValueAlias;
        public int FieldType;
        public bool IsPrimaryKey;
        public bool IsNotNull;
        public decimal? MinValue;
        public decimal? MaxValue;
        public DateTime? MinDateValue;
        public DateTime? MaxDateValue;
        public string References;

        public static SchemaFieldItem FromXml(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SchemaFieldItem));
            return (SchemaFieldItem)serializer.Deserialize(stream);
        }

        public void ToXml(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SchemaFieldItem));
            serializer.Serialize(stream, this);
        }
    }
}
