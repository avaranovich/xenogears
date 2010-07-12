using System.Diagnostics;

namespace XenoGears.Collections.Weak
{
    // Provides a weak reference to an object of the given type to be used in
    // a WeakDictionary along with the given comparer.
    [DebuggerNonUserCode]
    internal sealed class WeakKeyReference<T> : WeakReference<T> where T : class
    {
        public readonly int HashCode;

        public WeakKeyReference(T key, WeakKeyComparer<T> comparer)
            : base(key)
        {
            // retain the object's hash code immediately so that even
            // if the target is GC'ed we will be able to find and
            // remove the dead weak reference.
            this.HashCode = comparer.GetHashCode(key);
        }
    }
}