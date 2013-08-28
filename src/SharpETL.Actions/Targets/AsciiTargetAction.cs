using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Actions;
using SharpETL.Actions.Binding;
using SharpETL.Components;
using SharpETL.Extensions;
using System.IO;

namespace SharpETL.Actions.Targets
{
    public sealed class AsciiTargetAction : BindedActionBase
    {
        private string _root;
        private string _extension;
        private IDictionary<string, TextWriter> _writers;
        private string _columnSeparator;
        private string _recordSeparator;
        private string _quoteChar;
        private string _quoteCharReplacement;
        private bool _addHeader;

        public AsciiTargetAction(string id, string root, string extension, bool addHeader,
            string columnSeparator, string recordSeparator, string quoteChar, string quoteCharReplacement)
            : base(id)
        {
            if (String.IsNullOrEmpty(root)) {
                root = ".";
            }
            _root = Path.GetFullPath(root);
            if (!Directory.Exists(root)) {
                Directory.CreateDirectory(root);
            }
            _extension = extension;
            _writers = new Dictionary<string, TextWriter>();
            _addHeader = addHeader;
            _columnSeparator = columnSeparator;
            _recordSeparator = recordSeparator;
            _quoteChar = quoteChar;
            _quoteCharReplacement = quoteCharReplacement;
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return ActionEvents.Element | ActionEvents.Finally;
        }

        protected override bool OnElement(IElement element, IObserver<IElement> sink)
        {
            var data = element.Data as DataElementValues;
            if (data != null) {
                TextWriter writer;
                if (!_writers.TryGetValue(element.Name, out writer)) {
                    writer = new StreamWriter(GetFileName(element.Name), false, Encoding.UTF8);
                    _writers[element.Name] = writer;
                    WriteHeaderRow(data.Schema, writer);
                }
                WriteLine(data.Values, writer);
            }
            return true;
        }

        protected override void OnFinally()
        {
            foreach (var writer in _writers.Values) {
                writer.Close();
            }
        }

        private string GetFileName(string tableName)
        {
            string name = String.Format("{0}.{1}", tableName, _extension);
            return Path.Combine(_root, name);
        }

        private void WriteHeaderRow(IDictionary<string, int> schema, TextWriter writer)
        {
            if (_addHeader && schema != null) {
                WriteLine(schema.Select(x => x.Key), writer);
            }
        }

        private string AddQuotes(object obj)
        {
            if (obj == null) return String.Empty;
            string s = obj.ToString();
            if (!String.IsNullOrEmpty(_quoteCharReplacement)) {
                s = s.Replace(_quoteChar, _quoteCharReplacement);
            }
            return (String.IsNullOrEmpty(_quoteChar) ? s : String.Format("{0}{1}{0}", _quoteChar, s));
        }

        private void WriteLine(IEnumerable<object> parts, TextWriter writer)
        {
            string line = String.Join(_columnSeparator, parts.Select(x => AddQuotes(x))) + _recordSeparator;
            writer.Write(line);
        }
    }
}
