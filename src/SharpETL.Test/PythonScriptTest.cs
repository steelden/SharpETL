using SharpETL.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Scripting.Hosting;
using SharpETL.Components;
using Moq;
using SharpETL.Actions;
using SharpETL.Utility;
using SharpETL.Actions.Specialized;
using SharpETL.Actions.Script;
using System.Linq;

namespace SharpETL.Test
{
    [TestClass]
    public class PythonScriptTest
    {
        private readonly ScriptEngine _pythonEngine;

        public PythonScriptTest()
        {
            _pythonEngine = PythonScript.CreatePythonEngine();
        }

        [TestMethod]
        public void PythonScript_correctly_creates_python_engine()
        {
            var engine = PythonScript.CreatePythonEngine(Enumerable.Empty<Type>(), Enumerable.Empty<string>());
            Assert.IsNotNull(engine);
        }

        [TestMethod]
        public void PythonScript_constructor_works()
        {
            PythonScript script = new PythonScript("script", "x = 10", _pythonEngine);
            Assert.AreEqual("script", script.Id);
        }

        [TestMethod]
        public void PythonScript_OnElement_works()
        {
            string text = @"
from SharpETL.Components import Element
def OnElement(a, e):
  yield Element(e.Id, e.Id, [])";
            string testeid = "TestOK_OnElement";
            PythonScript script = new PythonScript("script", text, _pythonEngine);
            Mock<IScriptAction> maction = new Mock<IScriptAction>();
            Mock<IElement> melement = new Mock<IElement>();
            melement.Setup(x => x.Id).Returns(testeid);
            var result = script.OnElement(maction.Object, melement.Object);
            foreach (IElement e in result) {
                Assert.AreEqual(testeid, e.Name);
                Assert.AreEqual(testeid, e.Id);
            }
        }

        [TestMethod]
        public void PythonScript_OnCompleted_works()
        {
            string text = @"
from SharpETL.Components import Element
def OnCompleted(a):
  yield Element(a.Id, a.Id, [])
";
            PythonScript script = new PythonScript("script", text, _pythonEngine);
            Mock<IScriptAction> maction = new Mock<IScriptAction>();
            maction.Setup(x => x.Id).Returns("ok");
            var result = script.OnCompleted(maction.Object);
            var element = result.FirstOrDefault();
            Assert.IsNotNull(element);
            Assert.AreEqual("ok", element.Id);
            Assert.AreEqual("ok", element.Name);
        }

        [TestMethod]
        public void PythonScript_OnError_OnFinally_works()
        {
            string text = @"
from SharpETL.Components import Element
def OnError(a, ex):
    yield Element(ex.Message, ex.Message, [])
def OnFinally(a):
    pass
";
            PythonScript script = new PythonScript("script", text, _pythonEngine);
            Mock<IScriptAction> maction = new Mock<IScriptAction>();
            var result = script.OnError(maction.Object, new Exception("error"));
            var element = result.FirstOrDefault();
            Assert.IsNotNull(element);
            Assert.AreEqual("error", element.Id);
            Assert.AreEqual("error", element.Name);
            script.OnFinally(maction.Object);
        }
    }
}
