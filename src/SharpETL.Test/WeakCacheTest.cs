using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Utility;

namespace SharpETL.Test
{
    [TestClass]
    public class WeakCacheTest
    {
        [TestMethod]
        public void WeakCache_Set_works()
        {
            WeakCache<string> wc = new WeakCache<string>();
            wc.Set("test", "123");
            Assert.AreEqual("123", wc.Get("test", null));
        }

        [TestMethod]
        public void WeakCache_Get_works()
        {
            WeakCache<string> wc = new WeakCache<string>();
            var value = wc.Get("test", () => "12345");
            Assert.AreEqual("12345", value);
        }

        [TestMethod]
        public void WeakCache_callback_called_at_right_times()
        {
            bool called = false;
            WeakCache<object> wc = new WeakCache<object>();
            Func<object> callback = () => { called = true; return new object(); };

            wc.Get("test", callback);
            Assert.IsTrue(called);

            called = false;
            wc.Get("test", callback);
            Assert.IsFalse(called);

            GC.Collect();

            called = false;
            wc.Get("test", callback);
            Assert.IsTrue(called);
        }
    }
}
