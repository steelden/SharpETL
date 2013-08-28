using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Actions;
using SharpETL.Actions.Binding;
using SharpETL.Components;
using SharpETL.Services.Schema;
using SharpETL.Services;
using SharpETL.Utility;

namespace SharpETL.Actions.Db
{
    public sealed class SqlPreparationAction : SqlTextActionBase
    {
        private const decimal PRECISION = 0.00001m;
        private readonly string _connectionString;
        private string _schemaPath;
        private Lazy<ISimpleDbSchema> _schema;
        private Lazy<IDbQueryServiceConnection> _db;
        private IList<object> _values;
        private WeakCache<string[]> _pkCache;
        private bool _updateOnlyIfNewValueIsNotNull;

        public SqlPreparationAction(string id, string schemaPath, string connectionString, bool updateOnlyIfNewValueIsNotNull = false)
            : base(id)
        {
            _connectionString = connectionString;
            _schemaPath = schemaPath;
            _values = new List<object>();
            _pkCache = new WeakCache<string[]>();
            _updateOnlyIfNewValueIsNotNull = updateOnlyIfNewValueIsNotNull;
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return base.OnGetExpectedEvents() | ActionEvents.Finally;
        }

        protected override void OnSetEngine()
        {
            base.OnSetEngine();
            var sservice = Engine.GetService<ISchemaService>();
            _schema = new Lazy<ISimpleDbSchema>(() => sservice.LoadSimpleSchemaXml(_schemaPath));
            var dbservice = Engine.GetService<IDbQueryService>();
            _db = new Lazy<IDbQueryServiceConnection>(() => dbservice.OpenConnection(_connectionString));
        }

        protected override Tuple<string, string> GetSql(DataElement element)
        {
            var fields = _schema.Value.GetTableSchemaDictionary(element.Name);
            string sql = GetSelectSql(element, x => fields.ContainsKey(x));
            string rsql;

            var results = _db.Value.ExecuteQuery(sql);
            var row = results.GetRow(0, 0);
            if (row == null) {
                sql = GetInsertSql(element, x => fields.ContainsKey(x));
                rsql = GetDeleteSql(element);
            } else {
                sql = GetUpdateSql(element, x => fields.ContainsKey(x) && !CompareFields(results, row, element, x));
                var relement = GetElementFromRow(results, row, element);
                rsql = GetUpdateSql(relement, x => fields.ContainsKey(x) && !CompareFields(results, row, element, x));
            }
            if (results.GetRow(0, 1) != null) {
                // select by primary keys can never return more than 1 row
                throw new InvalidOperationException("Invalid schema (select by primary keys returned more than one row).");
            }
            return new Tuple<string,string>(sql, rsql);
        }

        private DataElement GetElementFromRow(ISource results, object[] row, DataElement element)
        {
            object[] preparedRow = new object[element.Data.Values.Length];
            foreach (var item in element.Data.Schema) {
                int idx = results.GetFieldIndex(0, item.Key);
                preparedRow[item.Value] = (idx < 0 ? null : row[idx]);
            }
            return new DataElement(element.Name, element.Id, preparedRow, element.Data.Schema);
        }

        private bool CompareFields(ISource results, object[] row, DataElement element, string fieldName)
        {
            object value = element.Data.Values[element.Data.Schema[fieldName]];
            object other = row[results.GetFieldIndex(0, fieldName)];
            return CompareValues(value, other);
        }

        private bool CompareValues(object value, object other)
        {
            if (value == null) {
                return (_updateOnlyIfNewValueIsNotNull ? true : other == null);
            }
            if (other == null) {
                return false;
            }
            bool equals = value.Equals(other);
            if (equals) {
                return true;
            }
            if (typeof(decimal).IsAssignableFrom(value.GetType()) &&
                typeof(decimal).IsAssignableFrom(other.GetType())) {
                return Math.Abs((decimal)value - (decimal)other) < PRECISION;
            }
            if (typeof(double).IsAssignableFrom(value.GetType()) &&
                typeof(double).IsAssignableFrom(other.GetType())) {
                return Math.Abs((double)value - (double)other) < (double)PRECISION;
            }
            return false;
        }

        protected override IEnumerable<string> GetPrimaryKeys(string name)
        {
            return _pkCache.Get(name, () => {
                var d = _schema.Value.GetTableSchemaDictionary(name);
                return d.Where(x => x.Value.IsPrimaryKey).Select(x => x.Value.FieldName).ToArray();
            });
        }

        protected override void OnFinally()
        {
            base.OnFinally();
            if (_db.IsValueCreated) {
                _db.Value.Close();
            }
            _schema = null;
            _db = null;
            _values = null;
            _pkCache = null;
        }
    }
}
