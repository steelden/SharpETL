using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Actions.Binding;
using SharpETL.Components;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharpETL.Actions.Specialized
{
    public sealed class BinarySerializeAction : BindedActionBase
    {
        private string _path;
        private Lazy<FileStream> _stream;
        private BinaryFormatter _serializer;

        public BinarySerializeAction(string id, string path, bool overwrite = false)
            : base(id)
        {
            if (String.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (!overwrite && File.Exists(path)) throw new InvalidOperationException("File already exists.");
            _path = path;
            _stream = new Lazy<FileStream>(() => File.Create(_path));
            _serializer = new BinaryFormatter();
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return ActionEvents.All;
        }

        protected override bool OnElement(IElement element, IObserver<IElement> sink)
        {
            _serializer.Serialize(_stream.Value, element);
            return true;
        }

        protected override bool OnCompleted(IObserver<IElement> sink)
        {
            _serializer.Serialize(_stream.Value, Element.Empty);
            return true;
        }

        protected override bool OnError(Exception exception, IObserver<IElement> sink)
        {
            if (_stream.IsValueCreated) {
                _stream.Value.Close();
                File.Delete(_path);
            }
            return true;
        }

        protected override void OnFinally()
        {
            if (_stream.IsValueCreated) {
                _stream.Value.Close();
            }
        }
    }
}
