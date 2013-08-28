using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Scripts;
using Moq;
using SharpETL.Components;

namespace SharpETL.Test
{
    [TestClass]
    public class SharpScriptTest
    {
        private const string _codeOnElement = @"
public IEnumerable<IElement> OnElement(IAction action, IElement element) {
  yield return new DataElement(action.Id, element.Id, new object[] { });
}
";
        private const string _codeOnCompleted = @"
public IEnumerable<IElement> OnCompleted(IAction action) {
  yield return new DataElement(action.Id, ""0"", new object[] { });
}
";
        private readonly string _codeAll = String.Format("{0}{1}{2}", _codeOnElement, Environment.NewLine, _codeOnCompleted);

        private Mock<IAction> _maction = new Mock<IAction>();
        private Mock<IElement> _melement = new Mock<IElement>();

        [TestInitialize]
        public void TestInit()
        {
            _maction.Setup(x => x.Id).Returns("testaction");
            _melement.Setup(x => x.Id).Returns("testelement");
        }

        [TestMethod]
        public void SharpScript_works_with_only_OnElement()
        {
            SharpScript script = new SharpScript("test", _codeOnElement);
            IEnumerable<IElement> result = script.OnElement(_maction.Object, _melement.Object);
            IEnumerable<IElement> result2 = script.OnCompleted(_maction.Object);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(0, result2.Count());
            var elem = result.First();
            Assert.AreEqual("testaction", elem.Name);
            Assert.AreEqual("testelement", elem.Id);
        }

        [TestMethod]
        public void SharpScript_works_with_only_OnCompleted()
        {
            SharpScript script = new SharpScript("test", _codeOnCompleted);
            IEnumerable<IElement> result = script.OnElement(_maction.Object, _melement.Object);
            IEnumerable<IElement> result2 = script.OnCompleted(_maction.Object);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual(0, result.Count());
            Assert.AreEqual(1, result2.Count());
            var elem = result2.First();
            Assert.AreEqual("testaction", elem.Name);
            Assert.AreEqual("0", elem.Id);
        }

        [TestMethod]
        public void SharpScript_works_with_code()
        {
            SharpScript script = new SharpScript("test", _codeAll);
            IEnumerable<IElement> result = script.OnElement(_maction.Object, _melement.Object);
            IEnumerable<IElement> result2 = script.OnCompleted(_maction.Object);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result2.Count());
        }

        [TestMethod]
        public void SharpScript_works_with_functions()
        {
            bool onelement = false;
            bool oncompleted = false;
            bool onerror = false;
            bool onfinally = false;
            SharpScript script = new SharpScript("test", (a, e) => { onelement = true; return null; }, a => { oncompleted = true; return null; },
                (a, ex) => { onerror = true; return null; }, a => { onfinally = true; });
            IEnumerable<IElement> result = script.OnElement(_maction.Object, _melement.Object);
            IEnumerable<IElement> result2 = script.OnCompleted(_maction.Object);
            IEnumerable<IElement> result3 = script.OnError(_maction.Object, new Exception());
            script.OnFinally(_maction.Object);
            Assert.IsTrue(onelement);
            Assert.IsTrue(oncompleted);
            Assert.IsTrue(onerror);
            Assert.IsTrue(onfinally);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreEqual(0, result.Count());
            Assert.AreEqual(0, result2.Count());
            Assert.AreEqual(0, result3.Count());
        }
    }
}
