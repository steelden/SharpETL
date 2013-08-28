using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Services
{
    public interface IDbQueryService : IService
    {
        IDbQueryServiceConnection OpenConnection(string connectionString);
    }
}
