using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;

namespace SharpETL.Services.Loggers
{
    using LogAction = Action<string, object[]>;
    public class ExtendableLogger : ILoggerService
    {
        private LogAction _onDebug;
        private LogAction _onInfo;
        private LogAction _onWarn;
        private LogAction _onError;
        private LogAction _onFatal;

        public ExtendableLogger OnDebug(LogAction action) { _onDebug = action; return this; }
        public ExtendableLogger OnInfo(LogAction action) { _onInfo = action; return this; }
        public ExtendableLogger OnWarn(LogAction action) { _onWarn = action; return this; }
        public ExtendableLogger OnError(LogAction action) { _onError = action; return this; }
        public ExtendableLogger OnFatal(LogAction action) { _onFatal = action; return this; }
        public ExtendableLogger OnAll(LogAction action) { return OnDebug(action).OnInfo(action).OnWarn(action).OnError(action).OnFatal(action); }

        public void Debug(string format, params object[] param)
        {
            if (_onDebug != null) {
                _onDebug(format, param);
            }
        }

        public void Info(string format, params object[] param)
        {
            if (_onInfo != null) {
                _onInfo(format, param);
            }
        }

        public void Warn(string format, params object[] param)
        {
            if (_onWarn != null) {
                _onWarn(format, param);
            }
        }

        public void Error(string format, params object[] param)
        {
            if (_onError != null) {
                _onError(format, param);
            }
        }

        public void Fatal(string format, params object[] param)
        {
            if (_onFatal != null) {
                _onFatal(format, param);
            }
        }
    }
}
