using System;
using XenoGears.Ogre.Exploration;

namespace XenoGears.Ogre.Rendering
{
    public class Renderer<T>
    {
        public T Context { get; set; }
        public Action<T, IObjectGraph> Logic { get; set; }

        public void Render(IObjectGraph graph)
        {
            Logic(Context, graph);
        }
    }
}