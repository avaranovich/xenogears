using System.Diagnostics;

namespace XenoGears.Collections.Weak
{
    // Provides a weak reference to a null target object, which, unlike
    // other weak references, is always considered to be alive. This 
    // facilitates handling null dictionary values, which are perfectly legal.
    [DebuggerNonUserCode]
    internal class WeakNullReference<T> : WeakReference<T> where T : class
    {
        public static readonly WeakNullReference<T> Singleton = new WeakNullReference<T>();

        private WeakNullReference() : base(null) { }

        public override bool IsAlive
        {
            get { return true; }
        }
    }
}