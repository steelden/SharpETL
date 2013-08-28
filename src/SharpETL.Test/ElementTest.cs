using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Components;

namespace SharpETL.Test
{
    [TestClass]
    public class ElementTest
    {
        [TestMethod]
        public void ElementT_works()
        {
            Element<int> e = new Element<int>("testname", "testid", 123);
            Assert.AreEqual("testname", e.Name);
            Assert.AreEqual("testid", e.Id);
            Assert.AreEqual(123, e.Data);
        }

        [TestMethod]
        public void ElementT_correct_equals()
        {
            Element<int> e1 = new Element<int>("testname", "testid", 123);
            Element<int> e2 = new Element<int>("testname", "testid", 345);
            Assert.IsTrue(e1.Equals(e2));
            Assert.IsTrue(e1.Equals((object)e2));
            Assert.IsTrue(e1.GetHashCode() == e2.GetHashCode());
        }
    }
}
