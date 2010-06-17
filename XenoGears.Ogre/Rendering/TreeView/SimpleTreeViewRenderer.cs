using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XenoGears.Functional;
using XenoGears.Ogre.Exploration;

namespace XenoGears.Ogre.Rendering.TreeView
{
    public class SimpleTreeViewRenderer : Renderer<TreeViewRenderForm>, ITreeViewRenderer
    {
        public int MaxDepth { get; set; }
        public int MaxNodes { get; set; }
        public bool ExpandAlreadyReferencedNodes { get; set; }

        public Func<Edge, String> TextRenderer { get; set; }
        public bool ShowToStringInHeader { get; set; }
        public bool ShowIndicesInHeader { get; set; }

        public SimpleTreeViewRenderer()
        {
            Logic = RenderImpl;

            MaxDepth = 5;
            MaxNodes = int.MaxValue;
            ExpandAlreadyReferencedNodes = false;
            ShowToStringInHeader = true;
            ShowIndicesInHeader = true;
            TextRenderer = RenderTextImpl;
        }

        private void RenderImpl(TreeViewRenderForm form, IObjectGraph graph)
        {
            form.Splitter.Panel2Collapsed = true;
            form.Tree.Nodes.Clear();

            try
            {
                form.Tree.BeginUpdate();

                if (MaxDepth <= 0 || MaxNodes <= 0)
                {
                    return;
                }
                else
                {
                    var root = CreateNodeRecursive(graph, new Edge(null, null, graph.Root), new State());
                    form.Tree.Nodes.Add(root);

                    var visible = root.Flatten(n => n.Nodes.Cast<TreeNode>().Where(n1 => 
                        n1.Tag != null && ((Tag)n1.Tag).ShouldBeInitiallyVisible));
                    visible.ForEach(n => n.EnsureVisible());
                    root.EnsureVisible();
                    form.Tree.SelectedNode = null;

                    form.Tree.AfterSelect += (o, e) =>
                    {
                        var tag = e.Node.Tag as Tag;
                        form.Splitter.Panel2Collapsed = !tag.IsAbbreviated;
                        form.Details.Text = tag.FullText;
                    };

                    form.Tree.BeforeExpand += (o, e) =>
                    {
                        var tag = e.Node.Tag as Tag;
                        (tag.DeferredExpand ?? (() => { }))();
                    };
                }
            }
            finally
            {
                form.Tree.EndUpdate();
            }
        }

        private class Tag
        {
            public bool IsAbbreviated { get; set; }
            public String FullText { get; set; }

            public bool ShouldBeInitiallyVisible { get; set; }
            public Action DeferredExpand { get; set; }
        }

        private class State
        {
            public int Depth = -1;
            public int Nodes = -1;
            public Dictionary<Object, List<TreeNode>> Visited = new Dictionary<Object, List<TreeNode>>();
        }

        private TreeNode CreateNodeRecursive(IObjectGraph graph, Edge e, State s)
        {
            s.Depth++;
            s.Nodes++;

            var fullText = TextRenderer(e);
            const int maxLength = 70;
            var shouldAbbr = fullText.Length >= maxLength || fullText.IndexOfAny(new char[] {'\r', '\n', '\t'}) != -1;
            var fullTextPp = fullText.Replace("\r\n", " ").Replace("\n", " ").Replace("\t", " ");
            var text = shouldAbbr ? fullTextPp.Substring(0, Math.Min(fullTextPp.Length, maxLength - 3)) + "..." : fullText;

//            using(var sw = new StreamWriter(@"d:\recursion.log", true))
//            {
//                sw.WriteLine(new String('.', e.Target.DistanceFromRoot) + "(" + e.Target.TraversalId + ") " + (e.Name ?? "null"));
//            }

            var node = new TreeNode(text);
            node.Tag = new Tag { IsAbbreviated = shouldAbbr, FullText = fullText };
            ((Tag)node.Tag).ShouldBeInitiallyVisible = s.Depth < MaxDepth && s.Nodes < MaxNodes && e.Name != "SyncRoot";

            var subEdges = graph.OutEdges(e.Target).ToArray();
            if (!subEdges.IsEmpty())
            {
                Action expandChildren = () =>
                {
                    node.Nodes.Clear();

                    var children = subEdges.Select(e1 => CreateNodeRecursive(graph, e1, s));
                    children = children.OrderBy(c => c.Name).ToArray();
                    children.ForEach(c => node.Nodes.Add(c));

                    ((Tag)node.Tag).DeferredExpand = null;
                };

                var eagerExpansion = s.Depth < MaxDepth && s.Nodes < MaxNodes &&
                    (ExpandAlreadyReferencedNodes || !s.Visited.ContainsKey(e.Target) || 
                    !(s.Visited[e.Target].Any(vn => ((Tag)vn.Tag).ShouldBeInitiallyVisible)));

                if (!s.Visited.ContainsKey(e.Target))
                    s.Visited[e.Target] = new List<TreeNode>();
                s.Visited[e.Target].Add(node);

                if (eagerExpansion)
                {
                    expandChildren();
                }
                else
                {
                    node.Nodes.Add("dummy");
                    ((Tag)node.Tag).DeferredExpand = expandChildren;
                }
            }

            // this should be in a "finally" block
            s.Depth--;

            return node;
        }

        private Object SafeGet(Func<Object> obj)
        {
            try
            {
                return obj();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private bool IsGrouping(Object obj)
        {
            return obj != null && obj.GetType().GetInterfaces().Any(iface =>
                iface.MetadataToken == typeof(IGrouping<,>).MetadataToken &&
                iface.Module == typeof(IGrouping<,>).Module);
        }

        private String RenderTextImpl(Edge e)
        {
            var buffer = new StringBuilder();

            if (e.Name == null)
            {
                if (!ShowToStringInHeader)
                {
                    buffer.Append("$Root");
                }
            }
            else
            {
                var m = Regex.Match(e.Name, @"^\$(?<index>\d*)$");
                if (!m.Success || !ShowToStringInHeader || ShowIndicesInHeader)
                {
                    buffer.Append(m.Success ? e.Name.Substring(1) : e.Name);
                }
            }

            if (ShowToStringInHeader)
            {
                buffer.Append(buffer.Length > 0 ? ": " : String.Empty);
                if (IsGrouping(e.Target.Object))
                {
                    var key = SafetyTools.SafeEval(() =>
                        e.Target.Object.GetType().GetProperty("Key").GetValue(e.Target.Object, null));
                    if (key != null)
                    {
                        buffer.Append(key);
                    }
                    else
                    {
                        buffer.Append(SafeGet(() => e.Target.ToString()));
                    }
                    
                }
                else
                {
                    buffer.Append(SafeGet(() => e.Target.ToString()));
                }
            }

            return buffer.Replace("\t", "    ").ToString();
        }
    }
}