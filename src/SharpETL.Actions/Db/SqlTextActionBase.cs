using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Actions;
using SharpETL.Actions.Binding;
using SharpETL.Components;
using System.IO;

namespace SharpETL.Actions.Db
{
    public abstract class SqlTextActionBase : BindedActionBase
    {
        protected const string NULL_VALUE = "NULL";
        protected const string INSERT_TEMPLATE = "insert into {0} ({1}) values ({2})";
        protected const string UPDATE_TEMPLATE = "update {0} set {1} where {2}";
        protected const string DELETE_TEMPLATE = "delete from {0} where {1}";
        protected const string SELECT_TEMPLATE = "select {1} from {0} where {2}";
        protected const string SELECT_COUNT_TEMPLATE = "select count(*) from {0} where {1}";

        private readonly IDictionary<string, int> _schema = 
            new Dictionary<string, int>() { { "sql", 0 }, { "rsql", 1 } };

        public SqlTextActionBase(string id)
            : base(id)
        {
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return ActionEvents.Element;
        }

        protected override bool OnElement(IElement element, IObserver<IElement> sink)
        {
            DataElement de = element as DataElement;
            if (de != null && de.Data != null && de.Data.Schema != null) {
                var sqlTuple = GetSql(de);
                if (!String.IsNullOrEmpty(sqlTuple.Item1)) {
                    sink.OnNext(new DataElement(element.Name, element.Id, new[] { sqlTuple.Item1, sqlTuple.Item2 }, _schema));
                }
            }
            return false;
        }

        protected abstract Tuple<string, string> GetSql(DataElement element);
        protected abstract IEnumerable<string> GetPrimaryKeys(string name);

        protected virtual string ValueToString(object value)
        {
            if (value == null) return NULL_VALUE;
            if (typeof(DateTime).IsAssignableFrom(value.GetType())) {
                DateTime dt = (DateTime)value;
                return String.Format("to_date('{0}', 'dd.mm.yyyy hh24:mi:ss')", dt.ToString("dd.MM.yyyy HH:mm:ss")); //HACK: Oracle hack, TODO: convert to value providers
            }
            return String.Format("'{0}'", value);
        }

        protected virtual string DQuote(string s)
        {
            string[] parts = s.Split('.');
            return String.Join(".", parts.Select(x => x == "*" ? x : String.Format("{0}{1}{0}", '"', x)));
        }

        protected virtual IEnumerable<string> GetEqualsStrings(DataElement element, IEnumerable<string> fields)
        {
            return fields.Select(x => String.Format("{0} = {1}", DQuote(x), ValueToString(element.Data.Values[element.Data.Schema[x]])));
        }

        protected virtual string GetKeysCondition(DataElement element)
        {
            var pkeys = GetPrimaryKeys(element.Name);
            var parts = GetEqualsStrings(element, pkeys);
            return String.Join(" and ", parts);
        }

        protected string GetInsertSql(DataElement element, Func<string, bool> fieldFilter)
        {
            var elements = element.Data.Schema.Where(x => fieldFilter(x.Key));
            if (elements.Count() == 0) return String.Empty;
            var fields = elements.Select(x => DQuote(x.Key));
            var values = elements.Select(x => ValueToString(element.Data.Values[x.Value]));
            return String.Format(INSERT_TEMPLATE, DQuote(element.Name), String.Join(", ", fields), String.Join(", ", values));
        }

        protected string GetUpdateSql(DataElement element, Func<string, bool> fieldFilter)
        {
            var elements = element.Data.Schema.Keys.Where(fieldFilter);
            if (elements.Count() == 0) return String.Empty;
            var sets = GetEqualsStrings(element, elements);
            return String.Format(UPDATE_TEMPLATE, DQuote(element.Name), String.Join(", ", sets), GetKeysCondition(element));
        }

        protected string GetDeleteSql(DataElement element)
        {
            return String.Format(DELETE_TEMPLATE, DQuote(element.Name), GetKeysCondition(element));
        }

        protected string GetSelectSql(DataElement element, Func<string, bool> fieldFilter)
        {
            var elements = element.Data.Schema.Where(x => fieldFilter(x.Key));
            if (elements.Count() == 0) return String.Empty;
            var fields = elements.Select(x => DQuote(x.Key));
            return String.Format(SELECT_TEMPLATE, DQuote(element.Name), String.Join(", ", fields), GetKeysCondition(element));
        }

        protected string GetSelectCountSql(DataElement element)
        {
            return String.Format(SELECT_COUNT_TEMPLATE, DQuote(element.Name), GetKeysCondition(element));
        }
    }
}
