using System.Collections.ObjectModel;

namespace XenoGears.Traits.Hierarchy
{
    public interface IImmutableHierarchy2
    {
        IImmutableHierarchy2 Parent { get; }
        ReadOnlyCollection<IImmutableHierarchy2> Children { get; }

        int Index { get; }
        IImmutableHierarchy2 Prev { get; }
        IImmutableHierarchy2 Next { get; }
    }

    public interface IImmutableHierarchy2<T> : IImmutableHierarchy2
        where T : class, IImmutableHierarchy2<T>
    {
        new T Parent { get; }
        new ReadOnlyCollection<T> Children { get; }

        new int Index { get; }
        new T Prev { get; }
        new T Next { get; }
    }
}