using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using Ninject;

namespace SharpETL.Ninject
{
    public static class ExtensionsToIMEConfiguration
    {
        public static void UseNinjectServiceResolver(this IMutableEngineConfiguration mutableConfig, IKernel kernel)
        {
            if (kernel == null) throw new ArgumentNullException("kernel");
            mutableConfig.SetServiceResolver(new NinjectServiceResolver(kernel));
        }
    }
}
