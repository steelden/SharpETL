using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;

namespace SharpETL.Planning.Enumerable
{
    internal class ObservableToEnumerable<T> : IEnumerable<T>
    {
        private IObservable<T> _observable;

        public ObservableToEnumerable(IObservable<T> observable)
        {
            _observable = observable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var observer = new ObservableReceiver<T>();
            _observable.Subscribe(observer);
            return observer.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
    }

    internal class ObservableReceiver<T> : IObserver<T>
    {
        private volatile bool _finished;
        private Exception _error;
        private ConcurrentQueue<T> _queue;

        public ObservableReceiver()
        {
            _finished = false;
            _queue = new ConcurrentQueue<T>();
        }

        public void OnCompleted()
        {
            _finished = true;
        }

        public void OnError(Exception error)
        {
            _error = error;
            _finished = true;
        }

        public void OnNext(T value)
        {
            if (!_finished) {
                _queue.Enqueue(value);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            T item;
            while (true) {
                if (_finished) break;
                if (_queue.TryDequeue(out item)) {
                    yield return item;
                } else {
                    Thread.Sleep(1);
                }
            }
            if (_error != null) {
                throw new InvalidOperationException("Observable exception", _error);
            }
        }
    }
}
