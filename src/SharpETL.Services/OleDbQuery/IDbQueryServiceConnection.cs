using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Services
{
    public interface IDbQueryServiceConnection : IDisposable
    {
        ISource ExecuteQuery(string sql, params object[] parameters);
        ISource ExecuteQuery(string sql, IEnumerable<object> parameters);
        void ExecuteCommand(string sql, params object[] parameters);
        void ExecuteCommand(string sql, IEnumerable<object> parameters);
        object ExecuteScalar(string sql, params object[] parameters);
        object ExecuteScalar(string sql, IEnumerable<object> parameters);
        T ExecuteScalar<T>(string sql, params object[] parameters);
        T ExecuteScalar<T>(string sql, IEnumerable<object> parameters);
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        bool InTransaction();
        void Close();
    }
}
