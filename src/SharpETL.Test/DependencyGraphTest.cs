using SharpETL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections;
using SharpETL.Utility;

namespace SharpETL.Test
{
    [TestClass]
    public class DependencyGraphTest
    {
        [TestMethod]
        public void DependencyGraph_works_correctly()
        {
            IDependencyGraph<int> g = new DependencyGraph<int>() {
                { 2, 3 },
                { 2, 5 },
                { 3, 4 },
                { 1, 6 },
                { 1, 2 }
            };

            int i = 0;

            var order1 = g.GetItemsInDependencyOrder();
            var expected1 = new int[] { 1, 6, 2, 5, 3, 4 };
            foreach (var n in order1) {
                Assert.AreEqual(expected1[i++], n);
            }
        }
    }
}
