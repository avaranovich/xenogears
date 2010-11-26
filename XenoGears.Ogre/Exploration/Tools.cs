using System;

namespace XenoGears.Ogre.Exploration
{
    public static class Tools
    {
        public static IObjectGraph ObjectGraph(this Object obj)
        {
            return new ObjectGraph(obj);
        }

        public static IObjectGraph ObjectGraph(this Object obj, IExplorer strategy)
        {
            return new ObjectGraph(obj, strategy);
        }

        public static IExplorer Chain(this IExplorer explorer, IPostprocessor processor)
        {
            // todo. implement this in a lazy way
            throw new NotImplementedException();
        }
    }
}