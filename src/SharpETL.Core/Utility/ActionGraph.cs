using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;
using QuickGraph.Algorithms;
using SharpETL.Configuration;
using SharpETL.Components;

namespace SharpETL.Utility
{
    public sealed class ActionGraph
    {
        private IBidirectionalGraph<IAction, Edge<IAction>> _graph;

        public ActionGraph(IEngineConfiguration config)
        {
            if (config == null) throw new ArgumentNullException("config");
            var graph = new BidirectionalGraph<IAction, Edge<IAction>>(false);
            graph.AddVertexRange(config.Actions);
            var edges = config.Links.Select(x => new Edge<IAction>(x.From, x.To));
            graph.AddEdgeRange(edges);
            _graph = graph;
        }

        public IEnumerable<IAction> GetActionsInDependencyOrder()
        {
            return _graph.TopologicalSort();
        }

        public IEnumerable<IAction> GetPrevActions(IAction action)
        {
            if (action == null) throw new ArgumentNullException("action");
            return _graph.InEdges(action).Select(x => x.Source);
        }

        public int GetPrevActionsCount(IAction action)
        {
            if (action == null) throw new ArgumentNullException("action");
            return _graph.InDegree(action);
        }

        public IEnumerable<IAction> GetNextActions(IAction action)
        {
            if (action == null) throw new ArgumentNullException("action");
            return _graph.OutEdges(action).Select(x => x.Target);
        }

        public int GetNextActionsCount(IAction action)
        {
            if (action == null) throw new ArgumentNullException("action");
            return _graph.OutDegree(action);
        }
    }
}
