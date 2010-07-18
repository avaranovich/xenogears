using QuickGraph;

namespace XenoGears.Ogre.Exploration
{
    public interface IObjectGraph : IImplicitGraph<Vertex, Edge>
    {
        Vertex Root { get; }
    }
}