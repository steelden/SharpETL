using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;
using SharpETL.Configuration;
using SharpETL.Components;

namespace SharpETL.Services.Loggers
{
    public sealed class NullLogger : ILoggerService, IProvideConfigurationData<IServiceInfo<ILoggerService, NullLogger>>
    {
        public void Debug(string format, params object[] param) { }
        public void Info(string format, params object[] param) { }
        public void Warn(string format, params object[] param) { }
        public void Error(string format, params object[] param) { }
        public void Fatal(string format, params object[] param) { }

        public IConfigurationData<IServiceInfo<ILoggerService, NullLogger>> GetConfiguration()
        {
            return new NullLoggerConfigurationData();
        }
    }

    [Serializable]
    public sealed class NullLoggerConfigurationData : ConfigurationDataBase<IServiceInfo<ILoggerService, NullLogger>>
    {
        public override IServiceInfo<ILoggerService, NullLogger> CreateObject()
        {
            return new ServiceInfo<ILoggerService, NullLogger>();
        }

        public override IDictionary<string, object> GetData()
        {
            return new Dictionary<string, object>() { { "Id", Id } };
        }
    }
}
