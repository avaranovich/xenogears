using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.DebuggerVisualizers;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Traits.Hierarchy;
using System.Linq;

namespace XenoGears.DebuggerVisualizers.Hierarchy
{
    public class HierarchySource : VisualizerObjectSource
    {
        public override void GetData(Object target, Stream outgoingData)
        {
            var curr = target.AssertCast<IImmutableHierarchy>();
            var root = curr.Hierarchy().Last();

            var map = new Dictionary<IImmutableHierarchy, HierarchyNode>();
            root.Family().ForEach(node =>
            {
                var m_node = new HierarchyNode();
                map.Add(node, m_node);

                var m_parent = node.Parent == null ? null : map[node.Parent];
                if (m_parent != null)
                {
                    m_node.Parent = m_parent;
                    m_parent.Children.Add(m_node);
                }
            });

            var model = new HierarchyModel(map[root], map[curr]);
            new BinaryFormatter().Serialize(outgoingData, model);
        }

        public override Object CreateReplacementObject(Object target, Stream incomingData)
        {
            throw new NotImplementedException();
        }

        public override void TransferData(Object target, Stream incomingData, Stream outgoingData)
        {
            throw new NotImplementedException();
        }
    }
}