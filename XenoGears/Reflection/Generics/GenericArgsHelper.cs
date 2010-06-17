using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Reflection.Generics
{
    [DebuggerNonUserCode]
    internal static class GenericArgsHelper
    {
        public static Type ToSafeGarg(this Type t)
        {
            if (t.IsByRef) t = typeof(Ref<>).XMakeGenericType(t.GetElementType().ToSafeGarg());
            if (t.IsPointer) t = typeof(Ptr<>).XMakeGenericType(t.GetElementType().ToSafeGarg());
            return t;
        }

        public static IEnumerable<Type> ToSafeGargs(this IEnumerable<Type> types)
        {
            return types.Select(t => ToSafeGarg(t)).ToReadOnly();
        }

        public static Type FromSafeGarg(this Type t)
        {
            if (t.SameMetadataToken(typeof(Ref<>))) return t.XGetGenericArguments().AssertSingle().FromSafeGarg();
            if (t.SameMetadataToken(typeof(Ptr<>))) return t.XGetGenericArguments().AssertSingle().FromSafeGarg();
            return t;
        }

        public static IEnumerable<Type> FromSafeGargs(this IEnumerable<Type> types)
        {
            return types.Select(t => t.FromSafeGarg()).ToReadOnly();
        }
    }
}