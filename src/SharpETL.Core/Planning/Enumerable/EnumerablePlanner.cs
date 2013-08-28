using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Planning;
using SharpETL.Configuration;
using SharpETL.Utility;
using SharpETL.Components;

namespace SharpETL.Planning.Enumerable
{
    public class EnumerablePlanner : IPlanner
    {
        public IPlan GetPlan(IEngineConfiguration config)
        {
            if (config == null) throw new ArgumentNullException("config");

            ActionGraph graph = new ActionGraph(config);
            IEnumerable<IAction> actionsInOrder = graph.GetActionsInDependencyOrder();
            IObservable<IElement> root = System.Linq.Enumerable.Empty<IElement>().AsObservable();
            IList<IEnumerable<IElement>> finals = new List<IEnumerable<IElement>>();

            foreach (IAction action in actionsInOrder) {
                if (graph.GetPrevActionsCount(action) > 0) {
                    var outputs = graph.GetPrevActions(action).Select(x => x.Output.AsEnumerable());
                    action.SetInput(outputs.Merge().AsObservable());
                } else {
                    action.SetInput(root);
                }
                if (graph.GetNextActionsCount(action) == 0) {
                    finals.Add(action.Output.AsEnumerable());
                }
            }

            return new EnumerablePlan(finals.Merge());
        }
    }
}
