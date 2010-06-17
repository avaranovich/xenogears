using System;
using System.Collections.Generic;
using QuickGraph;

namespace XenoGears.Ogre.Exploration
{
    public class Edge : Edge<Vertex>
    {
        public String Name { get; set; }

        public Edge(String name, Vertex source, Vertex target)
            : base(source, target)
        {
            Name = name;
        }

        public override string ToString()
        {
            return String.Format("{0} ({1} -> {2})", Name,
                Source == null ? "null" : (Source.Object == null ? "null" : Source.Object.ToString()),
                Target == null ? "null" : (Target.Object == null ? "null" : Target.Object.ToString()));
        }

        public override bool Equals(object obj)
        {
            var other = obj as Edge;

            return
                this.Name == other.Name &&
                ReferenceEquals(this.Source == null ? null : this.Source.Object, other.Source == null ? null : other.Source.Object) &&
                ReferenceEquals(this.Target == null ? null : this.Target.Object, other.Target == null ? null : other.Target.Object);
        }

        public override int GetHashCode()
        {
            var num = 0x10cee8ad;
            num = (-1521134295 * num) + (Name == null ? 0 : Name.GetHashCode());
            num = (-1521134295 * num) + (Source == null ? 0 : (Source.Object == null ? 0 : Source.Object.GetHashCode()));
            return (-1521134295 * num) + (Target == null ? 0 : (Target.Object == null ? 0 : Target.Object.GetHashCode()));
        }
    }
}