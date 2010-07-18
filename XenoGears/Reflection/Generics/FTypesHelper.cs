using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Reflection.Generics
{
    [DebuggerNonUserCode]
    public static class FTypesHelper
    {
        public static bool IsDelegate(this Type t)
        {
            if (t.SameMetadataToken(typeof(Delegate))) return false;
            if (t.SameMetadataToken(typeof(MulticastDelegate))) return false;

            for (var current = t; current != null; current = current.BaseType)
                if (current.SameMetadataToken(typeof(Delegate))) return true;

            return false;
        }

        public static bool IsFType(this Type t)
        {
            if (t == null) return false;
            return t.IsDelegate();
        }

        public static bool IsAction(this Type t)
        {
            if (t == null) return false;
            return t.IsFType() && t.Ret() == typeof(void);
        }

        public static bool IsFunc(this Type t)
        {
            if (t == null) return false;
            return t.IsFType() && t.Ret() != typeof(void);
        }

        public static Type Ret(this MethodBase mi)
        {
            if (mi == null) return null;
            if (mi is MethodInfo || mi is MethodBuilder || mi is DynamicMethod)
            {
                return mi.AssertCast<MethodInfo>().ReturnType;
            }
            else if (mi is ConstructorBuilder || mi is ConstructorInfo)
            {
                return typeof(void);
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        public static Type Param(this MethodBase mi, int i)
        {
            return mi.GetParameters()[i].ParameterType;
        }

        public static int Paramc(this MethodBase mi)
        {
            return mi.Params().Count();
        }

        public static Type[] Params(this MethodBase mi)
        {
            if (mi == null) return null;
            return mi.GetParameters().Select(p => p.ParameterType).ToArray();
        }

        public static Type Ret(this Type t)
        {
            if (t == null) return null;
            t.IsFType().AssertTrue();
            var ret = t.GetFunctionSignature().Ret();
            return ret.FromSafeGarg();
        }

        public static Type Param(this Type t, int i)
        {
            t.IsFType().AssertTrue();
            var param_i = t.GetFunctionSignature().Param(i);
            return param_i.FromSafeGarg();
        }

        public static int Paramc(this Type t)
        {
            t.IsFType().AssertTrue();
            return t.GetFunctionSignature().Paramc();
        }

        public static Type[] Params(this Type t)
        {
            if (t == null) return null;
            t.IsFType().AssertTrue();
            var @params = t.GetFunctionSignature().Params();
            return @params.FromSafeGargs().ToArray();
        }

        public static MethodInfo GetFunctionSignature(this Type t)
        {
            if (t == null) return null;
            t.IsFType().AssertTrue();
            if (t.IsDelegate())
            {
                return t.GetMethod("Invoke");
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        public static Type GetSignatureFunction(this MethodBase t)
        {
            throw new NotImplementedException();
        }

        public static Type ToFunc(this int argCount)
        {
            switch (argCount)
            {
                case 0:
                    return typeof(Func<>);
                case 1:
                    return typeof(Func<,>);
                case 2:
                    return typeof(Func<,,>);
                case 3:
                    return typeof(Func<,,,>);
                case 4:
                    return typeof(Func<,,,,>);
                case 5:
                    return typeof(Func<,,,,,>);
                case 6:
                    return typeof(Func<,,,,,,>);
                case 7:
                    return typeof(Func<,,,,,,,>);
                case 8:
                    return typeof(Func<,,,,,,,,>);
                case 9:
                    return typeof(Func<,,,,,,,,,>);
                case 10:
                    return typeof(Func<,,,,,,,,,,>);
                case 11:
                    return typeof(Func<,,,,,,,,,,,>);
                case 12:
                    return typeof(Func<,,,,,,,,,,,,>);
                default:
                    throw new NotSupportedException(String.Format(
                        "Funcs with '{0}' arg(s) are not supported", argCount));
            }
        }

        public static Type ToFunc(this IEnumerable<Type> args)
        {
            if (args == null) return null;
            var genericDef = ToFunc(args.Count() - 1);
            var safeGargs = args.ToSafeGargs().ToArray();
            return genericDef.XMakeGenericType(safeGargs);
        }

        public static Type ToFunc(this IEnumerable<Type> args, Type retVal)
        {
            if (args == null || retVal == null) return null;
            return args.Concat(retVal).ToFunc();
        }

        public static Type ToAction(this int argCount)
        {
            switch (argCount)
            {
                case 0:
                    return typeof(Action);
                case 1:
                    return typeof(Action<>);
                case 2:
                    return typeof(Action<,>);
                case 3:
                    return typeof(Action<,,>);
                case 4:
                    return typeof(Action<,,,>);
                case 5:
                    return typeof(Action<,,,,>);
                case 6:
                    return typeof(Action<,,,,,>);
                case 7:
                    return typeof(Action<,,,,,,>);
                case 8:
                    return typeof(Action<,,,,,,,>);
                case 9:
                    return typeof(Action<,,,,,,,,>);
                case 10:
                    return typeof(Action<,,,,,,,,,>);
                case 11:
                    return typeof(Action<,,,,,,,,,,>);
                case 12:
                    return typeof(Action<,,,,,,,,,,,>);
                default:
                    throw new NotSupportedException(String.Format(
                        "Actions with '{0}' arg(s) are not supported", argCount));
            }
        }

        public static Type ToAction(this IEnumerable<Type> args)
        {
            if (args == null) return null;
            var genericDef = ToAction(args.Count());
            var safeGargs = args.ToSafeGargs().ToArray();
            return genericDef.XMakeGenericType(safeGargs);
        }

        public static Type AsmFType(this IEnumerable<Type> args)
        {
            if (args == null) return null;
            if (args.LastOrDefault() == typeof(void))
            {
                return args.SkipLast(1).ToAction();
            }
            else
            {
                return args.SkipLast(1).ToFunc(args.Last());
            }
        }

        public static ReadOnlyCollection<Type> DasmFType(this Type ftype)
        {
            if (ftype == null) return null;
            var sig = ftype.GetFunctionSignature();
            return sig.Params().Concat(sig.Ret()).ToReadOnly();
        }

        public static Type ToAction(this Type ftype)
        {
            if (ftype == null) return null;
            return ftype.ToAction(t => true);
        }

        public static Type ToAction(this Type ftype, Func<Type, bool> argFilter)
        {
            if (ftype == null) return null;
            ftype.IsFType().AssertTrue();
            return ftype.Params().Where(argFilter).ToAction();
        }

        public static Type ToAction(this Type ftype, Func<IEnumerable<Type>, IEnumerable<Type>> xform)
        {
            if (ftype == null) return null;
            ftype.IsFType().AssertTrue();
            return xform(ftype.Params()).ToAction();
        }

        public static Type ToFunc(this Type ftype, Type ret)
        {
            if (ftype == null || ret == null) return null;
            return ftype.ToFunc(t => true, ret);
        }

        public static Type ToFunc(this Type ftype, Func<Type, bool> argFilter, Type ret)
        {
            if (ftype == null || ret == null) return null;
            ftype.IsFType().AssertTrue();
            return ftype.Params().Where(argFilter).ToFunc(ret);
        }

        public static Type ToFunc(this Type ftype, Func<IEnumerable<Type>, IEnumerable<Type>> xform, Type ret)
        {
            if (ftype == null || ret == null) return null;
            ftype.IsFType().AssertTrue();
            return xform(ftype.Params()).ToFunc(ret);
        }
    }
}