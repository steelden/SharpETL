using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using SharpETL.Components;

namespace SharpETL.Services.OleDbQuery
{
    public sealed class OleDbQueryServiceConnection : IDbQueryServiceConnection
    {
        private OleDbConnection _connection;
        private OleDbTransaction _transaction;

        public OleDbQueryServiceConnection(string connectionString)
        {
            _connection = new OleDbConnection(connectionString);
            _connection.Open();
            _transaction = null;
        }

        public void ExecuteCommand(string sql, IEnumerable<object> parameters)
        {
            using (OleDbCommand command = new OleDbCommand(sql, _connection, _transaction)) {
                command.AddParameters(parameters);
                command.ExecuteNonQuery();
            }
        }

        public void ExecuteCommand(string sql, params object[] parameters)
        {
            ExecuteCommand(sql, parameters.AsEnumerable());
        }

        public ISource ExecuteQuery(string sql, IEnumerable<object> parameters)
        {
            DataSet ds = new DataSet();
            using (OleDbCommand command = new OleDbCommand(sql, _connection, _transaction)) {
                command.AddParameters(parameters);
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command)) {
                    adapter.Fill(ds);
                }
            }
            return new DataSetSource(ds);
        }

        public ISource ExecuteQuery(string sql, params object[] parameters)
        {
            return ExecuteQuery(sql, parameters.AsEnumerable());
        }

        public object ExecuteScalar(string sql, IEnumerable<object> parameters)
        {
            using (OleDbCommand command = new OleDbCommand(sql, _connection, _transaction)) {
                command.AddParameters(parameters);
                object result = command.ExecuteScalar();
                return result == DBNull.Value ? null : result;
            }
        }

        public object ExecuteScalar(string sql, params object[] parameters)
        {
            return ExecuteScalar(sql, parameters.AsEnumerable());
        }

        public T ExecuteScalar<T>(string sql, IEnumerable<object> parameters)
        {
            object result = ExecuteScalar(sql, parameters);
            return result == null ? default(T) : (T)result;
        }

        public T ExecuteScalar<T>(string sql, params object[] parameters)
        {
            return ExecuteScalar<T>(sql, parameters.AsEnumerable());
        }

        public void BeginTransaction()
        {
            if (_transaction == null) {
                _transaction = _connection.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (_transaction != null) {
                _transaction.Commit();
                _transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction != null) {
                _transaction.Rollback();
                _transaction = null;
            }
        }

        public bool InTransaction()
        {
            return _transaction != null;
        }

        public void Close()
        {
            if (_connection != null) {
                _connection.Close();
                _connection = null;
            }
        }

        public void Dispose()
        {
            Close();
        }
    }

    internal static class ExtensionsToOleDbCommand
    {
        public static void AddParameters(this OleDbCommand command, IEnumerable<object> parameters)
        {
            int i = 0;
            foreach (var parameter in parameters) {
                command.Parameters.AddWithValue((i++).ToString(), parameter);
            }
        }

        public static void SetParameters(this OleDbCommand command, IEnumerable<object> parameters)
        {
            int i = 0;
            foreach (var parameter in parameters) {
                command.Parameters[i++].Value = parameter;
            }
        }
    }
}
