using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.IO.Readers.Options
{
    public class OptionsSet : IOptionsSet
    {
        private IDictionary<string, object> _options;
        private bool _readonly;

        public static readonly OptionsSet Empty = new OptionsSet(true);

        private OptionsSet(bool isReadonly)
            : this()
        {
            _readonly = isReadonly;
        }

        private OptionsSet(IOptionsSet other, bool isReadonly)
            : this(other)
        {
            _readonly = isReadonly;
        }

        public OptionsSet()
        {
            _readonly = false;
            _options = new Dictionary<string, object>();
        }

        public OptionsSet(IOptionsSet other)
        {
            _readonly = false;
            OptionsSet otherOS = other as OptionsSet;
            if (otherOS != null) {
                _options = new Dictionary<string, object>(otherOS._options);
            } else {
                foreach (var pair in other.All()) {
                    _options.Add(pair);
                }
            }
        }

        public IOptionsSet AsReadOnly()
        {
            return new OptionsSet(this, true);
        }

        public bool IsSet(string name)
        {
            return _options.ContainsKey(name);
        }

        public object Get(string name)
        {
            object value;
            return (_options.TryGetValue(name, out value) ? value : null);
        }

        public T Get<T>(string name)
        {
            object value;
            return (_options.TryGetValue(name, out value) ? (T)value : default(T));
        }

        public void Set(string name, object value)
        {
            if (_readonly) throw new InvalidOperationException();
            _options[name] = value;
        }

        public object this[string name]
        {
            get { return Get(name); }
            set { Set(name, value); }
        }

        public IEnumerable<KeyValuePair<string, object>> All()
        {
            return _options;
        }
    }
}
