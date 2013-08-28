using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Actions.Binding;
using SharpETL.Components;
using SharpETL.Services;

namespace SharpETL.Actions.Db
{
    public enum SqlExecuteTarget
    {
        Normal = 0,
        Reversed = 1
    }

    public sealed class SqlExecutionAction : BindedActionBase
    {
        private string _connectionString;
        private Lazy<IDbQueryServiceConnection> _db;
        private SqlExecuteTarget _target;

        public SqlExecutionAction(string id, string connectionString, SqlExecuteTarget target)
            : base(id)
        {
            _connectionString = connectionString;
            _target = target;
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return ActionEvents.All;
        }

        protected override void OnSetEngine()
        {
            base.OnSetEngine();
            var dbservice = Engine.GetService<IDbQueryService>();
            _db = new Lazy<IDbQueryServiceConnection>(() => dbservice.OpenConnection(_connectionString));
        }

        protected override bool OnElement(IElement element, IObserver<IElement> sink)
        {
            DataElement de = element as DataElement;
            if (de != null && de.Data != null && de.Data.Schema != null) {
                string name = (_target == SqlExecuteTarget.Normal ? "sql" : "rsql");
                string sql = (string)de.Data.Values[de.Data.Schema[name]];
                if (!_db.Value.InTransaction()) {
                    _db.Value.BeginTransaction();
                }
                _db.Value.ExecuteCommand(sql);
            }
            return true;
        }

        protected override bool OnCompleted(IObserver<IElement> sink)
        {
            if (_db.IsValueCreated) {
                _db.Value.CommitTransaction();
            }
            return true;
        }

        protected override bool OnError(Exception exception, IObserver<IElement> sink)
        {
            if (_db.IsValueCreated) {
                _db.Value.RollbackTransaction();
            }
            return true;
        }

        protected override void OnFinally()
        {
            if (_db.IsValueCreated) {
                _db.Value.Close();
            }
            _db = null;
        }
    }
}
