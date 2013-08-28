using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Planning;
using SharpETL.Components;

namespace SharpETL.Planning.Enumerable
{
    public class EnumerablePlan : IPlan
    {
        private IEnumerable<IElement> _elements;

        public EnumerablePlan(IEnumerable<IElement> elements)
        {
            _elements = elements;
        }

        public void Execute()
        {
            BeginExecute();
            EndExecute();
        }

        public void BeginExecute()
        {
            // do nothing
        }

        public void EndExecute()
        {
            foreach (var element in _elements) {
                // throw out everything
            }
        }
    }
}
