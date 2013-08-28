using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Actions.Db
{
    public sealed class SqlInsertTextAction : SqlTextActionBase
    {
        public SqlInsertTextAction(string id)
            : base(id)
        {
        }

        protected override Tuple<string, string> GetSql(DataElement element)
        {
            return new Tuple<string,string>(GetInsertSql(element, _ => true), String.Empty);
        }

        protected override IEnumerable<string> GetPrimaryKeys(string name)
        {
            throw new NotImplementedException();
        }
    }
}
