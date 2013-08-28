using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Components
{
    [Serializable]
    public class Context : IContext
    {
        private readonly IDictionary<string, object> _context = new Dictionary<string, object>();

        public bool IsSet(string name)
        {
            return _context.ContainsKey(name);
        }

        public object Get(string name)
        {
            return _context[name];
        }

        public object GetOrDefault(string name, object defaultValue = null)
        {
            object value;
            return (_context.TryGetValue(name, out value) ? value : defaultValue);
        }

        public void Set(string name, object value)
        {
            _context[name] = value;
        }

        public object this[string name]
        {
            get { return Get(name); }
            set { Set(name, value); }
        }
    }
}
