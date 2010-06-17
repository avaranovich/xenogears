using System.Collections.ObjectModel;

namespace XenoGears.Traits.Hierarchy
{
    public interface IImmutableHierarchy
    {
        IImmutableHierarchy Parent { get; }
        ReadOnlyCollection<IImmutableHierarchy> Children { get; }

        int Index { get; }
        IImmutableHierarchy Prev { get; }
        IImmutableHierarchy Next { get; }
    }

    public interface IImmutableHierarchy<T> : IImmutableHierarchy
        where T : class, IImmutableHierarchy<T>
    {
        new T Parent { get; }
        new ReadOnlyCollection<T> Children { get; }

        new int Index { get; }
        new T Prev { get; }
        new T Next { get; }
    }
}