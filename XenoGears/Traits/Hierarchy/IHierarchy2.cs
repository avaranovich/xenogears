using System;
using XenoGears.Collections.Observable;

namespace XenoGears.Traits.Hierarchy
{
    public interface IHierarchy2 : IImmutableHierarchy2
    {
        new IHierarchy2 Parent { get; set; }
        event Action<IHierarchy2, IHierarchy2> ParentChanged;

        new IObservableList<IHierarchy2> Children { get; }
        event Action<IHierarchy2> ChildAdded;
        event Action<IHierarchy2> ChildRemoved;
    }

    public interface IHierarchy2<T> : IImmutableHierarchy2<T>, IHierarchy2
        where T : class, IHierarchy2<T>
    {
        new T Parent { get; set; }
        new event Action<T, T> ParentChanged;

        new IObservableList<T> Children { get; }
        new event Action<T> ChildAdded;
        new event Action<T> ChildRemoved;
    }
}