using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Collections.Observable;
using XenoGears.ComponentModel;
using XenoGears.Functional;

namespace XenoGears.Traits.Hierarchy
{
    [DebuggerNonUserCode]
    public abstract class Hierarchy<T> : Freezable.Freezable, IHierarchy<T>
        where T : class, IHierarchy<T>
    {
        protected virtual IObservableList<T> InitChildren() { return new List<T>().Observe(); }
        protected virtual void OnChildAdding(T child){}
        protected virtual void OnChildAdded(T child){}
        protected virtual void OnChildRemoving(T child){}
        protected virtual void OnChildRemoved(T child){}
        protected virtual void OnParentChanging(T oldParent, T newParent){}
        protected virtual void OnParentChanged(T oldParent, T newParent){}

        protected Hierarchy(params T[] children)
            : this((IEnumerable<T>)children)
        {
        }

        protected Hierarchy(IEnumerable<T> children)
        {
            _children = InitChildren();
            _children.ListChanging += OnListChanging;
            _children.ListChanged += OnListChanged;
            children.ForEach(c => _children.Add(c));
        }

        private void OnListChanging(Object o, ListChangeEventArgs e)
        {
            foreach (T c in e.NewItems)
            {
                OnChildAdding(c);
                if (ChildAdding != null)
                {
                    ChildAdding(c);
                }
            }

            foreach (T c in e.OldItems)
            {
                OnChildRemoving(c);
                if (ChildRemoving != null)
                {
                    ChildRemoving(c);
                }
            }

            IsFrozen.AssertFalse();
        }

        private void OnListChanged(Object o, ListChangeEventArgs e)
        {
            var this_as_t = this.AssertCast<T>();
            var eqc = EqualityComparer<T>.Default;

            foreach (T c in e.NewItems)
            {
                OnChildAdded(c);
                if (ChildAdded != null) ChildAdded(c);

                if (c != null && !eqc.Equals(c.Parent, this_as_t))
                {
                    c.Parent = this_as_t;
                }
            }

            foreach (T c in e.OldItems)
            {
                OnChildRemoved(c);
                if (ChildRemoved != null) ChildRemoved(c);

                if (c != null && eqc.Equals(c.Parent, this_as_t))
                {
                    c.Parent = null;
                }
            }
        }

        public event Action<T, T> ParentChanging;
        public event Action<T, T> ParentChanged;
        T IImmutableHierarchy<T>.Parent { get { return Parent; } }
        private T _parent;
        public T Parent
        {
            get { return _parent; }
            set
            {
                var oldParent = _parent;
                var newParent = value;

                OnParentChanging(oldParent, newParent);
                if (ParentChanging != null) ParentChanging(oldParent, newParent);
                IsFrozen.AssertFalse();
                _parent = value;
                OnParentChanged(oldParent, newParent);
                if (ParentChanged != null) ParentChanged(oldParent, newParent);

                if (oldParent != null && oldParent.Children.Contains(this))
                {
                    oldParent.Children.Remove(this);
                }

                if (newParent != null && !newParent.Children.Contains(this))
                {
                    newParent.Children.Add(this);
                }
            }
        }

        public event Action<T> ChildAdding;
        public event Action<T> ChildAdded;
        public event Action<T> ChildRemoving;
        public event Action<T> ChildRemoved;
        private readonly IObservableList<T> _children;
        ReadOnlyCollection<T> IImmutableHierarchy<T>.Children { get { return Children.Cast<T>().ToReadOnly(); } }
        public IObservableList<T> Children { get { return _children; } }

        public int Index { get { return Parent == null ? -1 : Parent.Children.IndexOf(this); } }
        public T Prev { get { return Parent == null ? null : (Index == 0 ? null : Parent.Children[Index - 1]); } }
        public T Next { get { return Parent == null ? null : (Index == Parent.Children.Count() - 1 ? null : Parent.Children[Index + 1]); } }

        protected override void OnFreezing()
        {
            var children = Children.AssertCast<Hierarchy<T>>();
            children.Where(c => c != null).ForEach(c => c.Freeze());
        }

        protected override void OnUnfreezing()
        {
            var children = Children.AssertCast<Hierarchy<T>>();
            children.Where(c => c != null).ForEach(c => c.Unfreeze());
        }

        #region Non-generic boilerplate

        IImmutableHierarchy IImmutableHierarchy.Parent { get { return Parent; } }
        ReadOnlyCollection<IImmutableHierarchy> IImmutableHierarchy.Children { get { return Children.Cast<IImmutableHierarchy>().ToReadOnly(); } }
        IImmutableHierarchy IImmutableHierarchy.Prev { get { return Prev; } }
        IImmutableHierarchy IImmutableHierarchy.Next { get { return Next; } }
        IHierarchy IHierarchy.Parent { get { return Parent; } set { Parent = value.AssertCast<T>(); } }
        event Action<IHierarchy, IHierarchy> IHierarchy.ParentChanging { add { throw new NotImplementedException(); } remove { throw new NotImplementedException(); } }
        event Action<IHierarchy, IHierarchy> IHierarchy.ParentChanged { add { throw new NotImplementedException(); } remove { throw new NotImplementedException(); } }
        IObservableList<IHierarchy> IHierarchy.Children { get { throw new NotImplementedException(); } }
        event Action<IHierarchy> IHierarchy.ChildAdding { add { throw new NotImplementedException(); } remove { throw new NotImplementedException(); } }
        event Action<IHierarchy> IHierarchy.ChildAdded { add { throw new NotImplementedException(); } remove { throw new NotImplementedException(); } }
        event Action<IHierarchy> IHierarchy.ChildRemoving { add { throw new NotImplementedException(); } remove { throw new NotImplementedException(); } }
        event Action<IHierarchy> IHierarchy.ChildRemoved { add { throw new NotImplementedException(); } remove { throw new NotImplementedException(); } }

        #endregion
    }
}