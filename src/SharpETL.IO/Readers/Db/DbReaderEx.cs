using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Readers.Db
{
    public class DbReaderEx : FormatReaderBase<DbReaderOptions>
    {
        private OleDbConnection _connection;
        private DataSet _dataset;
        private string _id;

        public DbReaderEx(string id, string connectionString)
            : this(id, connectionString, new Dictionary<string, string>())
        {
        }

        public DbReaderEx(string id, string connectionString, IDictionary<string, string> namedQueries)
            : this(id, new DbReaderOptions(connectionString, namedQueries))
        {
            if (namedQueries == null) throw new ArgumentNullException("namedQueries");
        }

        public DbReaderEx(string id, IOptionsSet options)
            : base(new DbReaderOptions(options))
        {
            _connection = new OleDbConnection(Options.ConnectionString);
            _id = id;
            _dataset = null;
        }

        public override DataSet ToDataSet()
        {
            if (_connection == null) throw new InvalidOperationException("Connection closed.");
            if (_dataset == null) {
                _dataset = new DataSet();
                foreach (var namedQuery in Options.NamedQueries) {
                    ExecuteQuery(_dataset, _connection, namedQuery.Key, namedQuery.Value);
                }
            }
            return _dataset;
        }

        public void AddQuery(string name, string query)
        {
            if (_connection == null) throw new InvalidOperationException("Connection closed.");
            Options.NamedQueries[name] = query;
            if (_dataset != null) {
                if (_dataset.Tables.IndexOf(name) >= 0) {
                    _dataset.Tables.Remove(name);
                }
                ExecuteQuery(_dataset, _connection, name, query);
            }
        }

        private void ExecuteQuery(DataSet ds, OleDbConnection connection, string name, string query)
        {
            using (OleDbCommand command = new OleDbCommand(query, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command)) {
                adapter.Fill(ds, name);
            }
        }

        public void Close()
        {
            if (_connection != null) {
                _connection.Close();
                _connection = null;
                _dataset = null;
            }
        }
    }
}
