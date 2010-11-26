using System;

namespace XenoGears.DebuggerVisualizers.Hierarchy
{
    [Serializable]
    internal class HierarchyModel
    {
        public HierarchyNode Root { get; set; }
        public HierarchyNode Subject { get; set; }

        public HierarchyModel(HierarchyNode model, HierarchyNode subject)
        {
            Root = model;
            Subject = subject;
        }
    }
}