using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using SharpETL.Planning;
using SharpETL.Components;
using SharpETL.Extensions;
using SharpETL.Planning.Reactive;
using SharpETL.Planning.Enumerable;

namespace SharpETL.Extensions
{
    public static class ExtensionsToIMEConfiguration
    {
        public static void RegisterService<T>(this IMutableEngineConfiguration mutableConfig, Func<IService> serviceProvider) where T : IService
        {
            mutableConfig.ServiceResolver.RegisterService<T>(serviceProvider);
        }

        public static void UseReactivePlanner(this IMutableEngineConfiguration mutableConfig)
        {
            mutableConfig.SetPlanProvider(() => new ReactivePlanner().GetPlan(mutableConfig));
        }

        public static void UseEnumerablePlanner(this IMutableEngineConfiguration mutableConfig)
        {
            mutableConfig.SetPlanProvider(() => new EnumerablePlanner().GetPlan(mutableConfig));
        }

        public static void UseDefaultContextProvider(this IMutableEngineConfiguration mutableConfig)
        {
            mutableConfig.SetContextProvider(() => new Context());
        }

        public static void AddAction(this IMutableEngineConfiguration mutableConfig, IEnumerable<IAction> actions)
        {
            foreach (var action in actions) {
                mutableConfig.AddAction(action);
            }
        }

        public static void AddLink(this IMutableEngineConfiguration mutableConfig, IEnumerable<IAction> sourceList, IAction destination)
        {
            foreach (var source in sourceList) {
                mutableConfig.AddLink(source, destination);
            }
        }

        public static void AddLink(this IMutableEngineConfiguration mutableConfig, IAction source, IEnumerable<IAction> destinationList)
        {
            foreach (var destination in destinationList) {
                mutableConfig.AddLink(source, destination);
            }
        }
    }
}
