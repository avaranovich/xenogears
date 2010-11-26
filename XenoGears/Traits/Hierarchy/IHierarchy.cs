using System;
using XenoGears.Collections.Observable;

namespace XenoGears.Traits.Hierarchy
{
    public interface IHierarchy : IImmutableHierarchy
    {
        new IHierarchy Parent { get; set; }
        event Action<IHierarchy, IHierarchy> ParentChanging;
        event Action<IHierarchy, IHierarchy> ParentChanged;

        new IObservableList<IHierarchy> Children { get; }
        event Action<IHierarchy> ChildAdding;
        event Action<IHierarchy> ChildAdded;
        event Action<IHierarchy> ChildRemoving;
        event Action<IHierarchy> ChildRemoved;
    }

    public interface IHierarchy<T> : IImmutableHierarchy<T>, IHierarchy
        where T : class, IHierarchy<T>
    {
        new T Parent { get; set; }
        new event Action<T, T> ParentChanging;
        new event Action<T, T> ParentChanged;

        new IObservableList<T> Children { get; }
        new event Action<T> ChildAdding;
        new event Action<T> ChildAdded;
        new event Action<T> ChildRemoving;
        new event Action<T> ChildRemoved;
    }
}
