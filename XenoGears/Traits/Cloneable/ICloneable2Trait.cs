using System;
using System.Diagnostics;

namespace XenoGears.Traits.Cloneable
{
    [DebuggerNonUserCode]
    public static class ICloneable2Trait
    {
        public static T ShallowClone<T>(this T cloneable)
            where T : ICloneable2
        {
            return cloneable == null ? (T)(Object)null : cloneable.ShallowClone<T>();
        }

        public static T DeepClone<T>(this T cloneable)
            where T : ICloneable2
        {
            return cloneable == null ? (T)(Object)null : cloneable.DeepClone<T>();
        }

        public static bool SameProto(this ICloneable2 obj1, ICloneable2 obj2)
        {
            if (obj1 == null || obj2 == null)
            {
                return obj1 == null && obj2 == null;
            }
            else
            {
                return obj1.ProtoId == obj2.ProtoId;
            }
        }
    }
}