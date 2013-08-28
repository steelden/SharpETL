using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;

namespace SharpETL.Services.Loggers
{
    public sealed class FilterLogger : ILoggerService
    {
        private LoggerEventTypes _enabled;
        private ILoggerService _logger;

        public FilterLogger(ILoggerService logger)
        {
            _logger = logger;
            _enabled = LoggerEventTypes.All;
        }

        public FilterLogger Enable(LoggerEventTypes flags)
        {
            _enabled |= flags;
            return this;
        }

        public FilterLogger Disable(LoggerEventTypes flags)
        {
            _enabled &= ~flags;
            return this;
        }

        public FilterLogger EnableAll() { return Enable(LoggerEventTypes.All); }
        public FilterLogger DisableAll() { return Disable(LoggerEventTypes.All); }

        public void Debug(string format, params object[] param)
        {
            if ((_enabled & LoggerEventTypes.Debug) > 0) {
                _logger.Debug(format, param);
            }
        }

        public void Info(string format, params object[] param)
        {
            if ((_enabled & LoggerEventTypes.Info) > 0) {
                _logger.Info(format, param);
            }
        }

        public void Warn(string format, params object[] param)
        {
            if ((_enabled & LoggerEventTypes.Warn) > 0) {
                _logger.Warn(format, param);
            }
        }

        public void Error(string format, params object[] param)
        {
            if ((_enabled & LoggerEventTypes.Error) > 0) {
                _logger.Error(format, param);
            }
        }

        public void Fatal(string format, params object[] param)
        {
            if ((_enabled & LoggerEventTypes.Fatal) > 0) {
                _logger.Fatal(format, param);
            }
        }
    }
}
