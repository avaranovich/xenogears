using System;
using System.Collections.Generic;
using QuickGraph;
using QuickGraph.Algorithms.Exploration;

namespace XenoGears.Ogre.Exploration
{
    public class ObjectGraph : IObjectGraph
    {
        private Vertex _root;
        private IImplicitGraph<Vertex, Edge> _impl;

        public ObjectGraph(Object root)
            : this(root, new SimpleExplorer())
        {
        }

        public ObjectGraph(Object root, IExplorer explorer)
        {
            _root = new Vertex(root, 0, 0);
            _impl = new TransitionFactoryImplicitGraph<Vertex, Edge>{TransitionFactories = {explorer}};
        }

        public Vertex Root
        {
            get { return _root; }
        }

        public bool IsDirected
        {
            get { return _impl.IsDirected; }
        }

        public bool AllowParallelEdges
        {
            get { return _impl.AllowParallelEdges; }
        }

        public bool ContainsVertex(Vertex vertex)
        {
            return _impl.ContainsVertex(vertex);
        }

        public bool IsOutEdgesEmpty(Vertex v)
        {
            return _impl.IsOutEdgesEmpty(v);
        }

        public int OutDegree(Vertex v)
        {
            return _impl.OutDegree(v);
        }

        public IEnumerable<Edge> OutEdges(Vertex v)
        {
            return _impl.OutEdges(v);
        }

        public bool TryGetOutEdges(Vertex v, out IEnumerable<Edge> edges)
        {
            return _impl.TryGetOutEdges(v, out edges);
        }

        public Edge OutEdge(Vertex v, int index)
        {
            return _impl.OutEdge(v, index);
        }
    }
}