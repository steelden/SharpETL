using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Services.Schema;
using System.IO;

namespace SharpETL.Test
{
    [TestClass]
    public class SimpleDbSchemaTest
    {
        private SchemaFieldItem _testFieldItem = new SchemaFieldItem() {
            Id = 10,
            TableName = "test",
            FieldName = "test",
            FieldType = (int)SFIType.Number,
            IsPrimaryKey = true,
            IsNotNull = true,
            MinValue = 1.0m,
            MaxValue = 2.0m
        };

        private string _testXml = @"<?xml version=""1.0""?>
<SimpleDbSchema xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Fields>
    <Field>
      <Id>10</Id>
      <TableName>test</TableName>
      <FieldName>test</FieldName>
      <FieldType>1</FieldType>
      <IsPrimaryKey>true</IsPrimaryKey>
      <IsNotNull>true</IsNotNull>
      <MinValue>1.0</MinValue>
      <MaxValue>2.0</MaxValue>
      <MinDateValue xsi:nil=""true"" />
      <MaxDateValue xsi:nil=""true"" />
    </Field>
  </Fields>
</SimpleDbSchema>";

        [TestMethod]
        public void SimpleDbSchema_can_serialize_to_xml()
        {
            using (var stream = new MemoryStream()) {
                SimpleDbSchema service = new SimpleDbSchema(new[] { _testFieldItem });
                service.ToXml(stream);
                var bytes = stream.ToArray();
                string str = Encoding.Default.GetString(bytes);
                Assert.AreEqual(_testXml, str);
            }
        }

        [TestMethod]
        public void SimpleDbSchema_can_deserialize_from_xml()
        {
            var bytes = Encoding.Default.GetBytes(_testXml);
            using (var stream = new MemoryStream(bytes)) {
                var service = SimpleDbSchema.FromXml(stream);
                Assert.IsNotNull(service);
                var item = service.GetFieldSchema("test", "test");
                Assert.IsNotNull(item);
                Assert.AreEqual(10, item.Id);
            }
        }

        [TestMethod]
        public void SimpleDbSchema_can_find_item_by_table_and_field()
        {
            var service = new SimpleDbSchema(new[] { _testFieldItem });
            var item = service.GetFieldSchema("test", "test");
            Assert.IsNotNull(item);
        }
    }
}
