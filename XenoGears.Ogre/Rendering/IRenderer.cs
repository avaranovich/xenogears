using XenoGears.Ogre.Exploration;

namespace XenoGears.Ogre.Rendering
{
    public interface IRenderer<T>
    {
        T Context { get; set; }
        void Render(IObjectGraph graph);
    }
}