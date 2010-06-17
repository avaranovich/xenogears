using System;
using XenoGears.Ogre.Exploration;
using XenoGears.Ogre.Rendering.TreeView;

namespace XenoGears.Ogre.Rendering
{
    public static class Tools
    {
        public static void VisualizeObjectGraph(this Object obj)
        {
            obj.VisualizeObjectGraph(new SimpleExplorer(), new SimpleTreeViewRenderer());
        }

        public static void VisualizeObjectGraph(this Object obj, IExplorer explorer)
        {
            obj.VisualizeObjectGraph(explorer, new SimpleTreeViewRenderer());
        }

        public static void VisualizeObjectGraph(this Object obj, ITreeViewRenderer renderer)
        {
            obj.VisualizeObjectGraph(new SimpleExplorer(), renderer);
        }

        public static void VisualizeObjectGraph(this Object obj, IExplorer explorer, ITreeViewRenderer renderer)
        {
            renderer.Context = new TreeViewRenderForm();
            renderer.Render(obj.ObjectGraph(explorer));
            renderer.Context.ShowDialog();
        }
    }
}
