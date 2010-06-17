using System;
using System.Diagnostics;

namespace XenoGears.Reflection.Emit.Hackarounds
{
    [DebuggerNonUserCode]
    public static class SafeTypeForEmitHelper
    {
        public static Type SafeTypeForEmit(this Type t)
        {
            return t.IsRectMdArray() ? typeof(Object) : t;
        }
    }
}