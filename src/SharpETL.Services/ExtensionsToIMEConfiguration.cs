using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using SharpETL.Extensions;
using SharpETL.Services.StringAlgorithms;
using SharpETL.Services.Loggers;
using SharpETL.Services.OleDbQuery;
using SharpETL.Services.Schema;
using SharpETL.Services.Resolvers;

namespace SharpETL.Services
{
    public static class ExtensionsToIMEConfiguration
    {
        public static void UseDefaultServiceResolver(this IMutableEngineConfiguration mutableConfig)
        {
            mutableConfig.SetServiceResolver(new SimpleServiceResolver());
        }

        public static void UseStringDistanceService(this IMutableEngineConfiguration mutableConfig)
        {
            mutableConfig.RegisterService<IStringDistanceService>(() => new StringDistanceService());
        }

        public static void RegisterDbService(this IMutableEngineConfiguration mutableConfig, Func<IDbQueryService> dbService)
        {
            if (dbService == null) throw new ArgumentNullException("dbService");
            mutableConfig.RegisterService<IDbQueryService>(dbService);
        }

        public static void RegisterLogService(this IMutableEngineConfiguration mutableConfig, Func<ILoggerService> logService)
        {
            if (logService == null) throw new ArgumentNullException("logService");
            mutableConfig.RegisterService<ILoggerService>(logService);
        }

        public static void UseOleDbService(this IMutableEngineConfiguration mutableConfig)
        {
            mutableConfig.RegisterDbService(() => new OleDbQueryService());
        }

        public static void UseConsoleLogger(this IMutableEngineConfiguration mutableConfig)
        {
            mutableConfig.RegisterLogService(() => new ConsoleLogger());
        }

        public static void UseNullLogger(this IMutableEngineConfiguration mutableConfig)
        {
            mutableConfig.RegisterLogService(() => new NullLogger());
        }

        public static void UseSchemaService(this IMutableEngineConfiguration mutableConfig)
        {
            mutableConfig.RegisterService<ISchemaService>(() => new SchemaService());
        }
    }
}
