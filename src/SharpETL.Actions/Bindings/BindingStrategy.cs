using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SharpETL.Actions.Binding
{
    public class BindingStrategy : IConfigurableBindingStrategy
    {
        private ISubject<IElement> _subject;
        private Func<IObservable<IElement>, IObservable<IElement>> _onBind;
        private Func<IElement, IObserver<IElement>, bool> _onElement;
        private Func<IObserver<IElement>, bool> _onCompleted;
        private Func<Exception, IObserver<IElement>, bool> _onError;
        private Action _onFinally;

        public BindingStrategy()
        {
        }

        public IObservable<IElement> DataStream { get { return _subject.AsObservable(); } }
        public IObserver<IElement> DataSink { get { return _subject; } }

        public IConfigurableBindingStrategy OnBind(Func<IObservable<IElement>, IObservable<IElement>> onBind)
        {
            if (onBind == null) throw new ArgumentNullException("onBind");
            _onBind = onBind;
            return this;
        }

        public IConfigurableBindingStrategy OnElement(Func<IElement, IObserver<IElement>, bool> onElement)
        {
            if (onElement == null) throw new ArgumentNullException("onElement");
            _onElement = onElement;
            return this;
        }

        public IConfigurableBindingStrategy OnCompleted(Func<IObserver<IElement>, bool> onCompleted)
        {
            if (onCompleted == null) throw new ArgumentNullException("onCompleted");
            _onCompleted = onCompleted;
            return this;
        }

        public IConfigurableBindingStrategy OnError(Func<Exception, IObserver<IElement>, bool> onError)
        {
            if (onError == null) throw new ArgumentNullException("onError");
            _onError = onError;
            return this;
        }

        public IConfigurableBindingStrategy OnFinally(Action onFinally)
        {
            if (onFinally == null) throw new ArgumentNullException("onFinally");
            _onFinally = onFinally;
            return this;
        }

        public IObservable<IElement> Bind(IObservable<IElement> input)
        {
            _subject = OnCreateSubject();
            return OnBind(input);
        }

        protected virtual ISubject<IElement> OnCreateSubject()
        {
            return new Subject<IElement>();
        }

        protected virtual IObservable<IElement> OnBind(IObservable<IElement> input)
        {
            if (_onBind != null) {
                input = _onBind(input);
            }
            input.Subscribe(OnElement, OnError, OnCompleted);
            return _subject;
        }

        protected virtual void OnElement(IElement element)
        {
            if (_onElement == null || _onElement(element, _subject)) {
                _subject.OnNext(element);
            }
        }

        protected virtual void OnCompleted()
        {
            if (_onCompleted == null || _onCompleted(_subject)) {
                _subject.OnCompleted();
            }
            OnFinally();
        }

        protected virtual void OnError(Exception ex)
        {
            if (_onError == null || _onError(ex, _subject)) {
                _subject.OnError(ex);
            }
            OnFinally();
        }

        protected virtual void OnFinally()
        {
            if (_onFinally != null) {
                _onFinally();
            }
        }
    }
}
