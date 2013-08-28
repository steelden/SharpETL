using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Utility
{
    public sealed class WeakCache<T>
    {
        private IDictionary<string, WeakReference> _data;

        public WeakCache()
        {
            _data = new Dictionary<string, WeakReference>();
        }

        public void Set(string key, T value)
        {
            _data[key] = new WeakReference(value);
        }

        public T Get(string key, Func<T> provider)
        {
            WeakReference wr;
            if (_data.TryGetValue(key, out wr) && wr.IsAlive) {
                return (T)wr.Target;
            }
            T value = provider();
            Set(key, value);
            return value;
        }
    }
}
