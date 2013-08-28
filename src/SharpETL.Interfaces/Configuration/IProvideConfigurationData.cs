using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Configuration
{
    public interface IProvideConfigurationData<out T> where T : class
    {
        IConfigurationData<T> GetConfiguration();
    }
}
