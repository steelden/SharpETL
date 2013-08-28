using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace SharpETL.Test
{
    public static class XAssert
    {
        public static void ThrowsAny(Action action)
        {
            Throws<Exception>(action);
        }

        [DebuggerStepThrough]
        public static void Throws<T>(Action action) where T : Exception
        {
            bool thrown = false;
            try {
                action();
            }
            catch (T) {
                thrown = true;
            }
            if (!thrown) {
                Assert.Fail("Expected exception <{0}> was not thrown.", typeof(T).Name);
            }
        }
    }
}
