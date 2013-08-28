using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Planning.Enumerable
{
    internal class EnumerableToObservable<T> : IObservable<T>, IDisposable
    {
        private IEnumerable<T> _items;

        public EnumerableToObservable(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");
            _items = items;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");
            foreach (var item in _items) {
                observer.OnNext(item);
            }
            observer.OnCompleted();
            return this;
        }

        void IDisposable.Dispose()
        {
        }
    }
}
