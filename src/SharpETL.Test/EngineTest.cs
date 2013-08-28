using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Moq;
using SharpETL.Components;
using SharpETL.Configuration;
using SharpETL.Planning;
using SharpETL.Actions;
using SharpETL.Extensions;
using SharpETL.Services;

namespace SharpETL.Test
{
    [TestClass]
    public class EngineTest
    {
        [TestMethod]
        public void Engine_configuration_works()
        {
            var mlogger = new Mock<ILoggerService>();
            var mplan = new Mock<IPlan>();
            IEngineFactory factory = new EngineFactory();
            IEngine engine = factory.Create(c => {
                c.UseDefaultServiceResolver();
                c.UseDefaultContextProvider();
                c.RegisterLogService(() => mlogger.Object);
                c.SetPlanProvider(() => mplan.Object);
            });
        }

        [TestMethod]
        public void Engine_test()
        {
            IActionFactory afactory = new ActionFactory();
            var null1 = afactory.CreateNullAction("null1");
            var null2 = afactory.CreateNullAction("null2");

            IEngineFactory efactory = new EngineFactory();
            IEngine engine = efactory.Create(c => {
                c.UseDefaultServiceResolver();
                c.UseDefaultContextProvider();
                c.UseReactivePlanner();
                c.AddLink(null1, null2);
            });
            engine.Run();
        }
    }
}
