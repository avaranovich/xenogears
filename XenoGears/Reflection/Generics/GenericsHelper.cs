using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Reflection.Generics
{
    [DebuggerNonUserCode]
    public static class GenericsHelper
    {
        public static Type[] XGetGenericArguments(this Type t)
        {
            if (t == null) return null;
            return t.IsGenericType ? t.GetGenericArguments() : new Type[0];
        }

        public static Type[] XGetGenericArguments(this MethodBase mb)
        {
            if (mb == null) return null;
            return mb.IsGenericMethod ? mb.GetGenericArguments() : new Type[0];
        }

        public static Type XGetGenericDefinition(this Type t)
        {
            if (t == null) return null;
            return t.IsGenericType ? t.GetGenericTypeDefinition() : t;
        }

        public static MethodBase XGetGenericDefinition(this MethodBase mb)
        {
            if (mb == null) return null;
            var mi = mb as MethodInfo;
            return mi == null ? mb : mi.XGetGenericDefinition();
        }

        public static MethodInfo XGetGenericDefinition(this MethodInfo mi)
        {
            if (mi == null) return null;
            return mi.IsGenericMethod ? mi.GetGenericMethodDefinition() : mi;
        }

        public static Type XMakeGenericType(this Type t, IEnumerable<Type> targs)
        {
            return t.XMakeGenericType(targs.ToArray());
        }

        public static Type XMakeGenericType(this Type t, params Type[] targs)
        {
            if (t == null) return null;
            if (!t.IsGenericType || t.IsGenericParameter)
            {
                if (targs.Length != 0)
                {
                    throw new NotSupportedException(targs.Length.ToString());
                }
                else
                {
                    return t;
                }
            }
            else
            {
                return t.GetGenericTypeDefinition().MakeGenericType(targs);
            }
        }

        public static MethodInfo XMakeGenericMethod(this MethodBase mb, IEnumerable<Type> margs)
        {
            return mb.XMakeGenericMethod(margs.ToArray());
        }

        public static MethodInfo XMakeGenericMethod(this MethodBase mb, params Type[] margs)
        {
            return mb.XMakeGenericMethod(mb.DeclaringType.XGetGenericArguments(), margs);
        }

        public static MethodInfo XMakeGenericMethod(this MethodBase mb, IEnumerable<Type> targs, Type[] margs)
        {
            return mb.XMakeGenericMethod(targs.ToArray(), margs.ToArray());
        }

        public static MethodInfo XMakeGenericMethod(this MethodBase mb, Type[] targs, IEnumerable<Type> margs)
        {
            return mb.XMakeGenericMethod(targs.ToArray(), margs.ToArray());
        }

        public static MethodInfo XMakeGenericMethod(this MethodBase mb, IEnumerable<Type> targs, IEnumerable<Type> margs)
        {
            return mb.XMakeGenericMethod(targs.ToArray(), margs.ToArray());
        }

        public static MethodInfo XMakeGenericMethod(this MethodBase mb, Type[] targs, Type[] margs)
        {
            if (mb == null) return null;
            var pattern = (MethodInfo)mb.Module.ResolveMethod(mb.MetadataToken);

            var typeImpl = pattern.DeclaringType;
            if (!targs.IsNullOrEmpty()) typeImpl = typeImpl.MakeGenericType(targs);

            var methodImpl = typeImpl.GetMethods(BF.All).Single(mi2 => mi2.SameMetadataToken(mb));
            if (!margs.IsNullOrEmpty()) methodImpl = methodImpl.MakeGenericMethod(margs);

            return methodImpl;
        }
    }
}