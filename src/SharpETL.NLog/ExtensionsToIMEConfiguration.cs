using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using SharpETL.Extensions;
using SharpETL.Services;

namespace SharpETL.NLog
{
    public static class ExtensionsToIMEConfiguration
    {
        public static void UseNLog(this IMutableEngineConfiguration config)
        {
            config.RegisterLogService(() => new NLogEngineLogger());
        }

        public static void UseNLog(this IMutableEngineConfiguration config, string loggerName)
        {
            config.RegisterLogService(() => new NLogEngineLogger(loggerName));
        }
    }
}
