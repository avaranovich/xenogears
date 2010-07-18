
using System.Diagnostics;

namespace XenoGears.Unsafe
{
    [DebuggerNonUserCode]
    public static class UnsafeHelper
    {
        public static PointerToPinnedObject<T> Pin<T>(this T obj)
            where T : class
        {
            return new PointerToPinnedObject<T>(obj);
        }
    }
}
