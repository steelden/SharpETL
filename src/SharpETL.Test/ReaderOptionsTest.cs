using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Test
{
    [TestClass]
    public class ReaderOptionsTest
    {
        private IFileOptionsSet _options;

        public ReaderOptionsTest()
        {
            _options = new FileOptionsSet();
        }

        [TestMethod]
        public void ReaderOptions_indexer_works()
        {
            string name = "ItemTest";
            string value = "ItemTestItemValue";
            _options[name] = value;
            Assert.AreSame(value, _options[name]);
        }

        [TestMethod]
        public void ReaderOptions_Set_IsSet_works()
        {
            string nameGood = "SetTest";
            string nameBad = "Nonexistant-Key-Name";
            string value = "SetTestItemValue";
            _options.Set(nameGood, value);
            Assert.IsTrue(_options.IsSet(nameGood));
            Assert.IsFalse(_options.IsSet(nameBad));
        }

        [TestMethod]
        public void ReaderOptions_Get_works()
        {
            string nameGood = "GetTest";
            string nameBad = "Nonexistant-Key-Name";
            string valueString = "GetTestItemValue";
            int valueInt = 123456;
            bool valueBool = true;

            Assert.AreEqual(null, _options.Get(nameBad));

            _options.Set(nameGood, valueString);
            Assert.AreEqual(valueString, _options.Get(nameGood));
            Assert.AreEqual(valueString, _options.Get<string>(nameGood));

            _options.Set(nameGood, valueInt);
            Assert.AreEqual(valueInt, _options.Get(nameGood));
            Assert.AreEqual(valueInt, _options.Get<int>(nameGood));

            _options.Set(nameGood, valueBool);
            Assert.AreEqual(valueBool, _options.Get(nameGood));
            Assert.AreEqual(valueBool, _options.Get<bool>(nameGood));
        }
    }
}
