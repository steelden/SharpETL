using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharpETL.Actions.Specialized
{
    public sealed class BinaryDeserializeAction : BindedActionBase
    {
        private string _path;

        public BinaryDeserializeAction(string id, string path)
            : base(id)
        {
            if (String.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (!File.Exists(path)) throw new FileNotFoundException(path);
            _path = path;
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return ActionEvents.Completed;
        }

        protected override bool OnCompleted(IObserver<IElement> sink)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            using (FileStream stream = File.OpenRead(_path)) {
                while (true) {
                    IElement element = serializer.Deserialize(stream) as IElement;
                    if (element == null || (element.Name == null && element.Id == null)) {
                        break;
                    }
                    sink.OnNext(element);
                }
            }
            return true;
        }
    }
}
