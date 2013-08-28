using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.IO.Readers.Options
{
    public interface IOptionsSet
    {
        bool IsSet(string name);
        object Get(string name);
        T Get<T>(string name);
        void Set(string name, object value);
        object this[string name] { get; set; }
        IOptionsSet AsReadOnly();
        IEnumerable<KeyValuePair<string, object>> All();
    }
}
