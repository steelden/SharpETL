using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using SharpETL.Services;

namespace SharpETL.NLog
{
    public sealed class NLogEngineLogger : ILoggerService
    {
        public const string DEFAULT_LOGGER_NAME = "Default";

        private Logger _logger;

        public NLogEngineLogger()
            : this(DEFAULT_LOGGER_NAME)
        {
        }

        public NLogEngineLogger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        public void Debug(string format, params object[] param)
        {
            _logger.Debug(format, param);
        }

        public void Info(string format, params object[] param)
        {
            _logger.Info(format, param);
        }

        public void Warn(string format, params object[] param)
        {
            _logger.Warn(format, param);
        }

        public void Error(string format, params object[] param)
        {
            _logger.Error(format, param);
        }

        public void Fatal(string format, params object[] param)
        {
            _logger.Fatal(format, param);
        }
    }
}
