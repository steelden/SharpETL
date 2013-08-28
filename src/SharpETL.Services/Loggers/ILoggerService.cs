using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Services
{
    [Flags]
    public enum LoggerEventTypes
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warn = 4,
        Error = 8,
        Fatal = 16,
        All = Debug | Info | Warn | Error | Fatal
    }

    public interface ILoggerService : IService
    {
        void Debug(string format, params object[] param);
        void Info(string format, params object[] param);
        void Warn(string format, params object[] param);
        void Error(string format, params object[] param);
        void Fatal(string format, params object[] param);
    }
}
