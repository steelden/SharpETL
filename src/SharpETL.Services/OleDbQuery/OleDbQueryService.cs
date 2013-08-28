using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using SharpETL.Utility;
using SharpETL.Components;

namespace SharpETL.Services.OleDbQuery
{
    public sealed class OleDbQueryService : IDbQueryService
    {
        public IDbQueryServiceConnection OpenConnection(string connectionString)
        {
            return new OleDbQueryServiceConnection(connectionString);
        }
    }
}
