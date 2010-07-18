using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XenoGears.Functional;
using XenoGears.Traits.Hierarchy;

namespace XenoGears.DebuggerVisualizers.Hierarchy
{
    [Serializable]
    internal class HierarchyNode : IImmutableHierarchy<HierarchyNode>
    {
        public HierarchyNode Parent { get; set; }
        public List<HierarchyNode> Children { get; private set; }
        public int Index { get { return Parent == null ? -1 : Parent.Children.IndexOf(this); } }
        public HierarchyNode Prev { get { return Parent == null ? null : (Index == 0 ? null : Parent.Children[Index - 1]); } }
        public HierarchyNode Next { get { return Parent == null ? null : (Index == Parent.Children.Count() - 1 ? null : Parent.Children[Index + 1]); } }

        public HierarchyNode()
            : this(null)
        {
        }

        public HierarchyNode(HierarchyNode parent)
        {
            Parent = parent;
            Children = new List<HierarchyNode>();
        }

        public HierarchyNode(HierarchyNode parent, IEnumerable<HierarchyNode> children)
        {
            Parent = parent;
            Children = (children ?? Seq.Empty<HierarchyNode>()).ToList();
        }

        public HierarchyNode(HierarchyNode parent, params HierarchyNode[] children)
            : this(parent, (IEnumerable<HierarchyNode>)children)
        {
        }

        #region Boring boilerplate

        IImmutableHierarchy IImmutableHierarchy.Parent { get { return Parent; } }
        ReadOnlyCollection<IImmutableHierarchy> IImmutableHierarchy.Children { get { return Children.Cast<IImmutableHierarchy>().ToReadOnly(); } }
        IImmutableHierarchy IImmutableHierarchy.Prev { get { return Prev; } }
        IImmutableHierarchy IImmutableHierarchy.Next { get { return Next; } }
        ReadOnlyCollection<HierarchyNode> IImmutableHierarchy<HierarchyNode>.Children { get { return Children.Cast<HierarchyNode>().ToReadOnly(); } }

        #endregion
    }
}