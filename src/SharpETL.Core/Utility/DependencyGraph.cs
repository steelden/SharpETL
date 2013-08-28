using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;
using System.Threading;
using System.Collections;
using QuickGraph.Algorithms;

namespace SharpETL.Utility
{
    public sealed class DependencyGraph<T> : IDependencyGraph<T>
    {
        private IMutableVertexAndEdgeListGraph<T, Edge<T>> _graph;

        public DependencyGraph()
        {
            _graph = new AdjacencyGraph<T, Edge<T>>();
        }

        public void Add(T source, T target)
        {
            _graph.AddVertex(source);
            _graph.AddVertex(target);
            _graph.AddEdge(new Edge<T>(source, target));
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _graph.Vertices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        public IEnumerable<T> GetItemsInDependencyOrder()
        {
            return _graph.TopologicalSort();
        }
    }
}
