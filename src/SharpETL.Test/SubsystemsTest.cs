using SharpETL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using System.Collections;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Reactive;
using SharpETL.Utility;

namespace SharpETL.Test
{
    [TestClass]
    public class SubsystemsTest
    {
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        //[TestMethod]
        //public void Magnum_DependencyGraph_works_correctly()
        //{
        //    DependencyGraph<int> g = new DependencyGraph<int>();
        //    g.Add(2, 3);
        //    g.Add(2, 5);
        //    g.Add(3, 4);
        //    g.Add(1, 6);
        //    g.Add(1, 2);
            
        //    int i = 0;

        //    var order1 = g.GetItemsInDependencyOrder();
        //    var expected1 = new int[] { 4, 3, 5, 2, 6, 1 };
        //    foreach (var n in order1) {
        //        Assert.AreEqual(expected1[i++], n);
        //    }

        //    i = 0;
        //    var order2 = g.GetItemsInDependencyOrder(2);
        //    var expected2 = new int[] { 4, 3, 5, 2 };
        //    foreach (var n in order2) {
        //        Assert.AreEqual(expected2[i++], n);
        //    }
        //}

        private string _python_yield_test = @"
def test():
  n = 10
  yield 'test', n, [1, 2, 3, 8]
  n += 10
  yield 'test', n, [4, 5, 6, 7]
";

        [TestMethod]
        public void IronPython_works_correctly()
        {
            ScriptEngine engine = IronPython.Hosting.Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            ScriptSource source = engine.CreateScriptSourceFromString(_python_yield_test,
                Microsoft.Scripting.SourceCodeKind.Statements);
            CompiledCode code = source.Compile();
            code.Execute(scope);
            object fnTest = scope.GetVariable("test");
            IEnumerable result = (IEnumerable)engine.Operations.Invoke(fnTest);
            foreach (IList x in result) {
                Assert.AreEqual(3, x.Count);
                Assert.AreEqual(4, ((IList)x[2]).Count);
            }
        }

        [TestMethod]
        public void Rx_works_correctly()
        {
            var data = Observable.Range(0, 10, Scheduler.Immediate);
            int actualSum = 0;
            bool completed = false;
            var receiver = Observer.Create<int>(x => actualSum = x, () => completed = true);
            data.Sum().Subscribe(receiver);
            Assert.AreEqual(45, actualSum);
            Assert.IsTrue(completed);
        }
    }
}
