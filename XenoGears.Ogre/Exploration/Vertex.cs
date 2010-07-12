using System;
using System.Collections.Generic;

namespace XenoGears.Ogre.Exploration
{
    public class Vertex : ICloneable
    {
        public Object Object { get; set; }
        public int TraversalId { get; set; }
        public int DistanceFromRoot { get; set; }
        public int CloneId { get; set; }

        public Vertex(Object obj, int traversalId, int distanceFromRoot)
            : this(obj, traversalId, distanceFromRoot, 0)
        {
        }

        private Vertex(Object obj, int traversalId, int distanceFromRoot, int cloneId)
        {
            Object = obj;
            TraversalId = traversalId;
            DistanceFromRoot = distanceFromRoot;
            CloneId = cloneId;
        }

        public Object Clone()
        {
            lock(this)
            {
                return new Vertex(Object, TraversalId, DistanceFromRoot, CloneId++);
            }
        }

        public override string ToString()
        {
            return Object == null ? "null" : Object.ToString();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Vertex;

            return 
                this.Object == other.Object &&
                this.CloneId == other.CloneId;
        }

        public override int GetHashCode()
        {
            var num = 0x10cee8ad;
            num = (-1521134295 * num) + (Object == null ? 0 : Object.GetHashCode());
            return (-1521134295 * num) + CloneId;
        }
    }
}