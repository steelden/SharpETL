using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Actions.Binding;
using SharpETL.Components;
using SharpETL.Services;
using SharpETL.Services.Schema;
using SharpETL.Utility;

namespace SharpETL.Actions.Db
{
    public class ResolveReferencesError
    {
        public ResolveReferencesError(IAction action, DataElement element, string fieldName, string sql, object value)
        {
            Action = action;
            Element = element;
            FieldName = fieldName;
            Sql = sql;
            Value = value;
        }
        public readonly IAction Action;
        public readonly DataElement Element;
        public readonly string FieldName;
        public readonly string Sql;
        public readonly object Value;
    }

    public abstract class MapActionBase : BindedActionBase
    {
        protected readonly string _schemaPath;
        protected readonly string _connectionString;
        protected readonly Action<ResolveReferencesError> _errorHandler;
        protected readonly WeakCache<object> _valueCache = new WeakCache<object>();
        protected readonly WeakCache<string> _sqlCache = new WeakCache<string>();
        protected ISimpleDbSchema _schema;
        protected Lazy<IDbQueryServiceConnection> _db;

        public MapActionBase(string id, string schemaPath, string connectionString,
                                       Action<ResolveReferencesError> errorHandler = null)
            : base(id)
        {
            _schemaPath = schemaPath;
            _connectionString = connectionString;
            _errorHandler = errorHandler;
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return ActionEvents.Element | ActionEvents.Finally;
        }

        protected override void OnSetEngine()
        {
            base.OnSetEngine();
            var dbservice = Engine.GetService<IDbQueryService>();
            _db = new Lazy<IDbQueryServiceConnection>(() => dbservice.OpenConnection(_connectionString));
            var sservice = Engine.GetService<ISchemaService>();
            _schema = sservice.LoadSimpleSchemaXml(_schemaPath);
        }

        protected virtual void OnHandleError(DataElement element, string fieldName, string sql, object value)
        {
            if (_errorHandler != null) {
                _errorHandler(new ResolveReferencesError(this, element, fieldName, sql, value));
            }
        }

        protected override bool OnElement(IElement element, IObserver<IElement> sink)
        {
            var de = element as DataElement;
            if (de != null && de.Data != null && de.Data.Schema != null) {
                return MapReferences(de, sink);
            }
            return true;
        }

        protected override void OnFinally()
        {
            if (_db.IsValueCreated) {
                _db.Value.Close();
            }
        }

        protected virtual bool MapReferences(DataElement element, IObserver<IElement> sink)
        {
            var fields = _schema.GetTableSchemaDictionary(element.Name);
            var data = element.Data.Schema.Where(x => fields.ContainsKey(x.Key) && !String.IsNullOrEmpty(fields[x.Key].ValueAlias));
            foreach (var item in data) {
                string sql = GetSqlForValueAlias(fields[item.Key].ValueAlias);
                object value = element.Data.Values[item.Value];
                if (value != null) {
                    object newvalue = QueryValue(sql, value);
                    if (newvalue == null) {
                        OnHandleError(element, item.Key, sql, value);
                    } else {
                        element.Data.Values[item.Value] = newvalue;
                    }
                }
            }
            return true;
        }

        protected virtual object QueryValue(string sql, object value)
        {
            string key = String.Format("{0}${1}", sql, value);
            return _valueCache.Get(key, () => _db.Value.ExecuteScalar(sql, value));
        }

        protected abstract string GetSqlForValueAlias(string valueAlias);

        protected static string DQuote(string s)
        {
            string[] parts = s.Split('.');
            return String.Join(".", parts.Select(x => x == "*" ? x : String.Format("{0}{1}{0}", '"', x)));
        }
    }
}
