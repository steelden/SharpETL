using SharpETL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SharpETL.Components;
using SharpETL.Utility;

namespace SharpETL.Test
{
    [TestClass]
    public class ContextTest
    {
        [TestMethod]
        public void Context_Set_and_Get_work()
        {
            Context target = new Context();
            target.Set("#1", 1);
            target.Set("#2", "a2");
            Assert.AreEqual(1, target.Get("#1"));
            Assert.AreEqual("a2", target.Get("#2"));
        }

        [TestMethod]
        public void Context_GetOrDefault_works()
        {
            Context target = new Context();
            target.Set("#1", 10);
            Assert.AreEqual(10, target.GetOrDefault("#1"));
            Assert.AreEqual(null, target.GetOrDefault("#2"));
        }

        [TestMethod]
        public void Context_indexer_works()
        {
            Context target = new Context();
            target["#1"] = 100;
            target["#2"] = "a200";
            Assert.AreEqual(100, target["#1"]);
            Assert.AreEqual("a200", target["#2"]);
        }

        [TestMethod]
        public void Context_IsSet_works()
        {
            Context target = new Context();
            target.Set("#1", 123);
            Assert.IsTrue(target.IsSet("#1"));
            Assert.IsFalse(target.IsSet("#2"));
        }
    }
}
