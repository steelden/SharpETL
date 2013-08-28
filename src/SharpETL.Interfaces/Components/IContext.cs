using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Components
{
    public interface IContext
    {
        bool IsSet(string name);
        object Get(string name);
        object GetOrDefault(string name, object defaultValue = null);
        void Set(string name, object value);
        object this[string name] { get; set; }
    }
}
