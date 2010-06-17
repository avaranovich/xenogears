using System;
using System.Collections.Generic;
using XenoGears.Functional;

namespace XenoGears.Ogre.Exploration
{
    public class Postprocessor : IPostprocessor
    {
        public IObjectGraph Graph { get; set; }
        public Func<Vertex, IObjectGraph, IEnumerable<Edge>> Logic { get; set; }

        public bool IsValid(Vertex v)
        {
            return !Logic(v, Graph).IsEmpty();
        }

        public IEnumerable<Edge> Apply(Vertex source)
        {
            foreach (var kvp in Logic(source, Graph))
            {
                yield return kvp;
            }
        }

        public static IPostprocessor Empty
        {
            get
            {
                var postprocessor = new Postprocessor();
                postprocessor.Logic = (v, g) => g.OutEdges(v);
                return postprocessor;
            }
        }
    }
}