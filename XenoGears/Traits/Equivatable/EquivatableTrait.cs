using System.Diagnostics;
using XenoGears.Assertions;

namespace XenoGears.Traits.Equivatable
{
    [DebuggerNonUserCode]
    public static class EquivatableTrait
    {
        public static bool Equiv<T>(this IEquivatable<T> obj1, IEquivatable<T> obj2)
            where T : IEquivatable<T>
        {
            if (ReferenceEquals(null, obj1)) return ReferenceEquals(null, obj2);
            if (ReferenceEquals(null, obj2)) return ReferenceEquals(null, obj1);
            if (ReferenceEquals(obj1, obj2)) return true;
            return ((IEquivatable<T>)obj1).Equiv(obj2.AssertCast<T>());
        }

        public static bool NotEquiv<T>(this IEquivatable<T> obj1, IEquivatable<T> obj2)
            where T : IEquivatable<T>
        {
            return !Equiv(obj1, obj2);
        }

        public static int EquivHashCode<T>(this IEquivatable<T> obj)
            where T : IEquivatable<T>
        {
            return obj == null ? 0 : obj.EquivHashCode();
        }
    }
}