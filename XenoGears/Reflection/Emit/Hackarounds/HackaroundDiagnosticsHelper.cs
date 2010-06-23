using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using XenoGears.Assertions;
using XenoGears.Reflection.Generics;

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
            if (f is FieldBuilder) return false;
            return f.FieldType.IsSafeForEmit();
        }

        public static bool IsSafeForEmit(this MethodBase m)
        {
            if (m is MethodBuilder) return false;
            return m.Ret().IsSafeForEmit() && m.Params().All(p => p.IsSafeForEmit());
        }

        public static bool IsSafeForEmit(this PropertyInfo p)
        {
            if (p == null) return true;
            if (p is PropertyBuilder) return false;
            return p.PropertyType.IsSafeForEmit();
        }
    }
}