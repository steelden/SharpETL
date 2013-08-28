using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using SharpETL.Services.Schema;

namespace SharpETL.Test
{
    [TestClass]
    public class SchemaFieldItemTest
    {
        private const string EMPTY_FIELD_ITEM_XML = @"<?xml version=""1.0""?>
<Field xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Id>0</Id>
  <FieldType>0</FieldType>
  <IsPrimaryKey>false</IsPrimaryKey>
  <IsNotNull>false</IsNotNull>
  <MinValue xsi:nil=""true"" />
  <MaxValue xsi:nil=""true"" />
  <MinDateValue xsi:nil=""true"" />
  <MaxDateValue xsi:nil=""true"" />
</Field>";
        
        [TestMethod]
        public void SchemaFieldItem_can_serialize_empty()
        {
            using (var stream = new MemoryStream()) {
                SchemaFieldItem item = new SchemaFieldItem();
                item.ToXml(stream);
                var bytes = stream.ToArray();
                var str = Encoding.Default.GetString(bytes);
                Assert.AreEqual(EMPTY_FIELD_ITEM_XML, str);
            }
        }

        [TestMethod]
        public void SchemaFieldItem_can_deserialize_empty()
        {
            var bytes = Encoding.Default.GetBytes(EMPTY_FIELD_ITEM_XML);
            using (var stream = new MemoryStream(bytes)) {
                SchemaFieldItem item = SchemaFieldItem.FromXml(stream);
                Assert.IsNotNull(item);
            }
        }
    }
}
