using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using SharpETL.Configuration;
using SharpETL.Actions;
using SharpETL.Scripts;
using SharpETL.IO.Readers;

namespace SharpETL.Ninject
{
    public class DataTransformEngineNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEngineFactory>().To<EngineFactory>().InSingletonScope();
            Bind<IActionFactory>().To<ActionFactory>().InSingletonScope();
            Bind<IScriptFactory>().To<ScriptFactory>().InSingletonScope();
            Bind<IReaderFactory>().To<ReaderFactory>().InSingletonScope();
        }
    }
}
