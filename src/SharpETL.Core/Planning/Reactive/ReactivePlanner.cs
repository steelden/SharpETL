using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using SharpETL.Planning;
using SharpETL.Configuration;
using SharpETL.Utility;
using SharpETL.Components;
using System.Reactive.Subjects;

namespace SharpETL.Planning.Reactive
{
    public sealed class ReactivePlanner : IPlanner
    {
        public IPlan GetPlan(IEngineConfiguration config)
        {
            if (config == null) throw new ArgumentNullException("config");

            ActionGraph graph = new ActionGraph(config);
            var actionsInOrder = graph.GetActionsInDependencyOrder();
            var root = Observable.Empty<IElement>().Publish();
            IList<IAction> finals = new List<IAction>();

            foreach (IAction action in actionsInOrder) {
                if (graph.GetPrevActionsCount(action) > 0) {
                    var outputs = graph.GetPrevActions(action).Select(x => x.Output);
                    if (outputs.Count() == 1) {
                        action.SetInput(outputs.First());
                    } else {
                        action.SetInput(outputs.Merge());
                    }
                } else {
                    action.SetInput(root);
                }
                if (graph.GetNextActionsCount(action) == 0) {
                    finals.Add(action);
                }
            }

            var final = finals
                .Select(x => x.Output.IgnoreElements())
                .Merge()
                .DefaultIfEmpty();

            return new ReactivePlan(root, final);
        }
    }
}
