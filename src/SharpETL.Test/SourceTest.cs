using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharpETL.Components;
using SharpETL.Extensions;

namespace SharpETL.Test
{
    [TestClass]
    public class SourceTest
    {
        private ISource _source;

        [TestInitialize]
        public void TestInit()
        {
            Mock<ISource> ms = new Mock<ISource>(MockBehavior.Strict);
            ms.Setup(x => x.GetTables()).Returns(new int[] { 0, 1 });
            ms.Setup(x => x.GetTableName(0)).Returns("test0");
            ms.Setup(x => x.GetTableName(1)).Returns("test1");
            ms.Setup(x => x.GetTableIndex("test0")).Returns(0);
            ms.Setup(x => x.GetTableIndex("test1")).Returns(1);
            ms.Setup(x => x.GetFields(0)).Returns(new int[] { 0, 1, 2 });
            ms.Setup(x => x.GetFields(1)).Returns(new int[] { 0 });
            ms.Setup(x => x.GetFieldName(0, 0)).Returns("field00");
            ms.Setup(x => x.GetFieldName(0, 1)).Returns("field01");
            ms.Setup(x => x.GetFieldName(0, 2)).Returns("field02");
            ms.Setup(x => x.GetFieldName(1, 0)).Returns("field10");
            ms.Setup(x => x.GetFieldIndex(0, "field00")).Returns(0);
            ms.Setup(x => x.GetFieldIndex(0, "field01")).Returns(1);
            ms.Setup(x => x.GetFieldIndex(0, "field02")).Returns(2);
            ms.Setup(x => x.GetFieldIndex(1, "field10")).Returns(0);
            _source = ms.Object;
        }

        [TestMethod]
        public void SourcesExt_GetTablesWithNames_works()
        {
            Tuple<int, string>[] tables = _source.GetTablesWithNames().ToArray();
            Assert.AreEqual(0, tables[0].Item1);
            Assert.AreEqual("test0", tables[0].Item2);
            Assert.AreEqual(1, tables[1].Item1);
            Assert.AreEqual("test1", tables[1].Item2);
        }
    }
}
