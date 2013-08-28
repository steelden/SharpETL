using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Planning;
using SharpETL.Components;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace SharpETL.Planning.Reactive
{
    public sealed class ReactivePlan : IPlan
    {
        private IConnectableObservable<IElement> _root;
        private IObservable<IElement> _final;

        public ReactivePlan(IConnectableObservable<IElement> root, IObservable<IElement> final)
        {
            if (root == null) throw new ArgumentNullException("root");
            if (final == null) throw new ArgumentNullException("final");
            _root = root;
            _final = final;
        }

        public void Execute()
        {
            BeginExecute();
            EndExecute();
        }

        public void BeginExecute()
        {
            _root.Connect();
        }

        public void EndExecute()
        {
            _final.Wait();
        }
    }
}
