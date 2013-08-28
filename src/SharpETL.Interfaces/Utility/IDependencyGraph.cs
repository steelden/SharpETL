using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Utility
{
    public interface IDependencyGraph<T> : IEnumerable<T>
    {
        void Add(T source, T target);
        IEnumerable<T> GetItemsInDependencyOrder();
    }
}
