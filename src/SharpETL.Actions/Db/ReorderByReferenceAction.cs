using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Services;
using SharpETL.Services.Schema;
using SharpETL.Utility;

namespace SharpETL.Actions.Db
{
    public class ReorderByReferenceAction : BindedActionBase
    {
        private string _schemaPath;
        private Lazy<ISimpleDbSchema> _schema;
        private IDictionary<string, IList<IElement>> _dataCache;
        private IEnumerable<string> _tableOrder;

        public ReorderByReferenceAction(string id, string schemaPath)
            : base(id)
        {
            _schemaPath = schemaPath;
            _dataCache = new Dictionary<string, IList<IElement>>();
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return ActionEvents.Element | ActionEvents.Completed;
        }

        protected override void OnSetEngine()
        {
            base.OnSetEngine();
            var sservice = Engine.GetService<ISchemaService>();
            _schema = new Lazy<ISimpleDbSchema>(() => sservice.LoadSimpleSchemaXml(_schemaPath));
            _tableOrder = GetTablesInDependencyOrder();
        }

        protected override bool OnElement(IElement element, IObserver<IElement> sink)
        {
            IList<IElement> elements;
            if (!_dataCache.TryGetValue(element.Name, out elements)) {
                elements = new List<IElement>();
                _dataCache.Add(element.Name, elements);
            }
            elements.Add(element);
            return false;
        }

        protected override bool OnCompleted(IObserver<IElement> sink)
        {
            foreach (string tableName in _tableOrder) {
                IList<IElement> elements;
                if (_dataCache.TryGetValue(tableName, out elements)) {
                    foreach (IElement element in elements) {
                        sink.OnNext(element);
                    }
                }
            }
            return true;
        }

        private IEnumerable<string> GetTablesInDependencyOrder()
        {
            DependencyGraph<string> graph = new DependencyGraph<string>();
            foreach (string tableName in _schema.Value.GetTables()) {
                var items = _schema.Value.GetTableSchema(tableName).Where(x => !String.IsNullOrEmpty(x.References));
                foreach (SchemaFieldItem item in items) {
                    string[] parts = item.References.Split(';');
                    if (parts.Length > 0 && tableName != parts[0]) {
                        graph.Add(parts[0], tableName);
                    }
                }
            }
            return graph.GetItemsInDependencyOrder();
        }
    }
}
