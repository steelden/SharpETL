using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Readers.Db
{
    public class DbReader : FormatReaderBase<DbReaderOptions>
    {
        private string _id;

        public DbReader(string id, string connectionString, IDictionary<string, string> namedQueries)
            : this(new DbReaderOptions(connectionString, namedQueries))
        {
            _id = id;
        }

        public DbReader(IOptionsSet options) : base(new DbReaderOptions(options)) { }

        public override DataSet ToDataSet()
        {
            DataSet result = new DataSet(_id);
            using (OleDbConnection connection = new OleDbConnection(Options.ConnectionString)) {
                foreach (var namedQuery in Options.NamedQueries) {
                    ExecuteQuery(result, connection, namedQuery.Key, namedQuery.Value);
                }
            }
            return result;
        }

        private void ExecuteQuery(DataSet ds, OleDbConnection connection, string name, string query)
        {
            using (OleDbCommand command = new OleDbCommand(query, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command)) {
                adapter.Fill(ds, name);
            }
        }
    }
}
