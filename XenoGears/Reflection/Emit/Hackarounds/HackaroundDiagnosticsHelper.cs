using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Reflection.Emit.Hackarounds
{
    [DebuggerNonUserCode]
    public static class HackaroundDiagnosticsHelper
    {
        public static Type SafeTypeForEmit(this Type t)
        {
            return t.IsRectMdArray() ? typeof(Object) : t;
        }

        public static bool IsSafeForEmit(this MemberInfo mi)
        {
            if (mi == null) return true;

            var t = mi as Type;
            if (t != null) return t.IsSafeForEmit();

            var f = mi as FieldInfo;
            if (f != null) return f.IsSafeForEmit();

            var m = mi as MethodBase;
            if (m != null) return m.IsSafeForEmit();

            var p = mi as PropertyInfo;
            if (p != null) return p.IsSafeForEmit();

            throw AssertionHelper.Fail();
        }

        public static bool IsSafeForEmit(this Type t)
        {
            return !t.IsRectMdArray();
        }

        public static bool IsSafeForEmit(this FieldInfo f)
        {
            if (f == null) return true;
            return f.FieldType.IsSafeForEmit();
        }

        public static bool IsSafeForEmit(this MethodBase m)
        {
            return false;
        }

        public static bool IsSafeForEmit(this PropertyInfo p)
        {
            if (p == null) return true;
            return p.PropertyType.IsSafeForEmit();
        }
    }
}