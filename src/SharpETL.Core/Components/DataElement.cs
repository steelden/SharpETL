using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Components
{
    [Serializable]
    public class DataElementValues
    {
        public DataElementValues(object[] values, IDictionary<string, int> schema = null)
        {
            if (values == null) throw new ArgumentNullException("values");
            Schema = schema;
            Values = values;
        }

        public readonly IDictionary<string, int> Schema;
        public readonly object[] Values;

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Schema == null) {
                sb.Append("[");
                sb.Append(String.Join(", ", Values.Select(x => String.Format("\"{0}\"", x ?? "null"))));
                sb.Append("]");
            } else {
                sb.Append("{");
                sb.Append(String.Join(", ", Schema.Select(x => String.Format("\"{0}\": \"{1}\"", x.Key, Values[x.Value] ?? "null"))));
                sb.Append("}");
            }
            return sb.ToString();
        }
    }

    [Serializable]
    public class DataElement : Element<DataElementValues>
    {
        public DataElement(string name, string id, object[] values, IDictionary<string, int> schema = null)
            : base(name, id, new DataElementValues(values, schema))
        {
        }

        public DataElement(string name, string id, IEnumerable<object> values, IDictionary<string, int> schema = null)
            : this(name, id, values.ToArray(), schema)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{ ");
            sb.AppendFormat("\"{0}\", \"{1}\", {2}", Name, Id, Data);
            sb.Append(" }");
            return sb.ToString();
        }
    }
}
