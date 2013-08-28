using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Utility;

namespace SharpETL.Services.Loggers
{
    public sealed class ConsoleLogger : ExtendableLogger
    {
        public ConsoleLogger()
        {
            OnAll((f, o) => Console.WriteLine(String.Format(f, o)));
        }
    }
}
