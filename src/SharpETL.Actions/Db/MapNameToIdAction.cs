using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Actions.Db
{
    public sealed class MapNameToIdAction : MapActionBase
    {
        public MapNameToIdAction(string id, string schemaPath, string connectionString,
                                 Action<ResolveReferencesError> errorHandler = null)
            : base(id, schemaPath, connectionString, errorHandler)
        {
        }

        protected override string GetSqlForValueAlias(string valueAlias)
        {
            // table_name;name_field;id_field;criteria
            // dict_g;description;id;grp=2570
            return _sqlCache.Get(valueAlias, () => {
                string[] parts = valueAlias.Split(';');
                var tableName = DQuote(parts[0]);
                var nameField = DQuote(parts[1]);
                var idField = DQuote(parts[2]);
                var whereCondition = (parts.Length > 3 ? " and " + parts[3] : String.Empty);
                return String.Format("select {0} from {1} where {2} = ?{3}", idField, tableName, nameField, whereCondition);
            });
        }
    }
}
