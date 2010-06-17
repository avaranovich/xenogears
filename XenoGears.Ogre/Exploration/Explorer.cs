using System;
using System.Collections.Generic;
using XenoGears.Functional;

namespace XenoGears.Ogre.Exploration
{
    public class Explorer : IExplorer
    {
        public Func<Object, IEnumerable<Pair<String, Object>>> Logic { get; set; }

        private int _idFactory = -1;
        private Dictionary<Object, Vertex> _cache = new Dictionary<Object, Vertex>();

        public bool IsValid(Vertex v)
        {
            return !Logic(v.Object).IsEmpty();
        }

        public IEnumerable<Edge> Apply(Vertex source)
        {
            foreach (var kvp_iter in Logic(source.Object))
            {
                var kvp = kvp_iter; // just make resharper happeh
                Func<Vertex> createVertex = () => new Vertex(kvp.Value, ++_idFactory, source.DistanceFromRoot + 1);

                if (kvp.Value == null)
                {
                    yield return new Edge(kvp.Key, source, createVertex());
                }
                else
                {
                    var contains = SafetyTools.SafeEval(() => _cache.ContainsKey(kvp.Value), true);
                    if (!contains)
                    {
                        SafetyTools.SafeDo(() => _cache.Add(kvp.Value, createVertex()));
                    }

                    var vertex = SafetyTools.SafeEval(() => _cache[kvp.Value], createVertex);
                    yield return new Edge(kvp.Key, source, vertex);
                }
            }
        }
    }
}