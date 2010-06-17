using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Traits.Hierarchy
{
    [DebuggerNonUserCode]
    public static class IHierarchyExtensions
    {
        public static ReadOnlyCollection<IImmutableHierarchy> Parents(this IImmutableHierarchy node)
        {
            if (node == null) return Seq.Empty<IImmutableHierarchy>().ToReadOnly();
            return node.Unfolde(n => n.Parent, n => n != null).ToReadOnly();
        }

        public static ReadOnlyCollection<T> Parents<T>(this IImmutableHierarchy<T> node)
            where T : class, IImmutableHierarchy<T>
        {
            return ((IImmutableHierarchy)node).Parents().AssertCast<T>().ToReadOnly();
        }

        public static ReadOnlyCollection<T> Parents<T>(this Hierarchy<T> node)
            where T : class, IHierarchy<T>
        {
            return ((IImmutableHierarchy)node).Parents().AssertCast<T>().ToReadOnly();
        }

        public static ReadOnlyCollection<IImmutableHierarchy> Hierarchy(this IImmutableHierarchy node)
        {
            if (node == null) return Seq.Empty<IImmutableHierarchy>().ToReadOnly();
            return node.Unfoldi(n => n.Parent, n => n != null).ToReadOnly();
        }

        public static ReadOnlyCollection<T> Hierarchy<T>(this IImmutableHierarchy<T> node)
            where T : class, IImmutableHierarchy<T>
        {
            return ((IImmutableHierarchy)node).Hierarchy().AssertCast<T>().ToReadOnly();
        }

        public static ReadOnlyCollection<T> Hierarchy<T>(this Hierarchy<T> node)
            where T : class, IHierarchy<T>
        {
            return ((IImmutableHierarchy)node).Hierarchy().AssertCast<T>().ToReadOnly();
        }

        public static ReadOnlyCollection<IImmutableHierarchy> ChildrenRecursive(this IImmutableHierarchy node)
        {
            if (node == null) return Seq.Empty<IImmutableHierarchy>().ToReadOnly();
            return node.Children.Flatten(n => n == null ? Seq.Empty<IImmutableHierarchy>() : n.Children).ToReadOnly();
        }

        public static ReadOnlyCollection<T> ChildrenRecursive<T>(this IImmutableHierarchy<T> node)
            where T : class, IImmutableHierarchy<T>
        {
            return ((IImmutableHierarchy)node).ChildrenRecursive().AssertCast<T>().ToReadOnly();
        }

        public static ReadOnlyCollection<T> ChildrenRecursive<T>(this Hierarchy<T> node)
            where T : class, IHierarchy<T>
        {
            return ((IImmutableHierarchy)node).ChildrenRecursive().AssertCast<T>().ToReadOnly();
        }

        public static ReadOnlyCollection<IImmutableHierarchy> Family(this IImmutableHierarchy node)
        {
            if (node == null) return Seq.Empty<IImmutableHierarchy>().ToReadOnly();
            return node.Concat(node.ChildrenRecursive()).ToReadOnly();
        }

        public static ReadOnlyCollection<T> Family<T>(this IImmutableHierarchy<T> node)
            where T : class, IImmutableHierarchy<T>
        {
            return ((IImmutableHierarchy)node).Family().AssertCast<T>().ToReadOnly();
        }

        public static ReadOnlyCollection<T> Family<T>(this Hierarchy<T> node)
            where T : class, IHierarchy<T>
        {
            return ((IImmutableHierarchy)node).Family().AssertCast<T>().ToReadOnly();
        }

        public static IEnumerable<IImmutableHierarchy2> Children2(this IImmutableHierarchy2 node)
        {
            return node.Children;
        }

        public static IEnumerable<T> Children2<T>(this IImmutableHierarchy2<T> node)
            where T : class, IImmutableHierarchy2<T>
        {
            return node.Children;
        }

        public static ReadOnlyCollection<IImmutableHierarchy2> Parents2(this IImmutableHierarchy2 node)
        {
            if (node == null) return Seq.Empty<IImmutableHierarchy2>().ToReadOnly();
            return node.Unfolde(n => n.Parent, n => n != null).ToReadOnly();
        }

        public static ReadOnlyCollection<T> Parents2<T>(this IImmutableHierarchy2<T> node)
            where T : class, IImmutableHierarchy2<T>
        {
            return ((IImmutableHierarchy2)node).Parents2().AssertCast<T>().ToReadOnly();
        }

        public static ReadOnlyCollection<IImmutableHierarchy2> Hierarchy2(this IImmutableHierarchy2 node)
        {
            if (node == null) return Seq.Empty<IImmutableHierarchy2>().ToReadOnly();
            return node.Unfoldi(n => n.Parent, n => n != null).ToReadOnly();
        }

        public static ReadOnlyCollection<T> Hierarchy2<T>(this IImmutableHierarchy2<T> node)
            where T : class, IImmutableHierarchy2<T>
        {
            return ((IImmutableHierarchy2)node).Hierarchy2().AssertCast<T>().ToReadOnly();
        }

        public static ReadOnlyCollection<IImmutableHierarchy2> ChildrenRecursive2(this IImmutableHierarchy2 node)
        {
            if (node == null) return Seq.Empty<IImmutableHierarchy2>().ToReadOnly();
            return node.Children.Flatten(n => n == null ? Seq.Empty<IImmutableHierarchy2>() : n.Children).ToReadOnly();
        }

        public static ReadOnlyCollection<T> ChildrenRecursive2<T>(this IImmutableHierarchy2<T> node)
            where T : class, IImmutableHierarchy2<T>
        {
            return ((IImmutableHierarchy2)node).ChildrenRecursive2().AssertCast<T>().ToReadOnly();
        }

        public static ReadOnlyCollection<IImmutableHierarchy2> Family2(this IImmutableHierarchy2 node)
        {
            if (node == null) return Seq.Empty<IImmutableHierarchy2>().ToReadOnly();
            return node.Concat(node.ChildrenRecursive2()).ToReadOnly();
        }

        public static ReadOnlyCollection<T> Family2<T>(this IImmutableHierarchy2<T> node)
            where T : class, IImmutableHierarchy2<T>
        {
            return ((IImmutableHierarchy2)node).Family2().AssertCast<T>().ToReadOnly();
        }
    }
}