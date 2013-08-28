using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Actions.Binding;
using SharpETL.Components;
using SharpETL.Extensions;
using SharpETL.Utility;

namespace SharpETL.Actions.Script
{
    public sealed class SourceAction : SourceActionBase
    {
        private WeakCache<IDictionary<string, int>> _schemaCache;

        public SourceAction(string id, ISource source)
            : base(id, source)
        {
            _schemaCache = new WeakCache<IDictionary<string, int>>();
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return ActionEvents.Completed;
        }

        protected override bool OnCompleted(IObserver<IElement> sink)
        {
            foreach (var table in Source.GetTablesWithNames()) {
                int tableIndex = table.Item1;
                string tableName = table.Item2;
                var tableSchema = _schemaCache.Get(tableName, () => {
                    return Source.GetFieldsWithNames(tableIndex).ToDictionary(x => x.Item2, x => x.Item1);
                });
                int i = 0;
                foreach (var items in Source.GetAllRows(tableIndex)) {
                    var element = new DataElement(tableName, (i++).ToString(), items, tableSchema);
                    sink.OnNext(element);
                }
            }
            return true;
        }
    }
}
