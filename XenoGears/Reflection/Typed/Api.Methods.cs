﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection.Emit;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;
using System.Linq;

namespace XenoGears.Reflection.Typed
{
    // note. this API is abandoned in favor of Truesight: http://code.google.com/p/truesight/
    // since it's very clumsy I was going to delete it completely
    // however, a significant piece of XenoGears relies on it, so I'll just make it "internal" for now


    [DebuggerNonUserCode]
    internal static partial class Api
    {
        public static T GetMethod<T>(this Object target, String name)
        {
            return target.GetMethod<T>(target.GetType(), name);
        }

        public static T GetMethod<T>(this Object target, String name, BindingFlags flags)
        {
            return target.GetMethod<T>(target.GetType(), name, flags);
        }

        public static T GetMethod<T>(this Type t, String name)
        {
            return ((Object)null).GetMethod<T>(t, name);
        }

        public static T GetMethod<T>(this Type t, String name, BindingFlags flags)
        {
            return ((Object)null).GetMethod<T>(t, name, flags);
        }

        public static T GetMethod<T>(this Object target, Type t, String name)
        {
            return target.GetMethod<T>(t, name, BF.All);
        }

        public static T GetMethod<T>(this Object target, Type t, String name, BindingFlags flags)
        {
            var sig = typeof(T).GetFunctionSignature();
            var sigRetval = sig.Ret();
            var sigArgs = sig.Params();

            // 1. Search for the methods that match the name, flags and signature passed via T
            // note. user should manually care about extension methods and param arrays
            var mis = t.GetMethods(flags) // todo. would be lovely to discover private methods in base classes
                .Where(mi => mi.Name == name)
                .Where(mi =>
                {
                    Func<MethodBase, Type[]> exposeSig = mi1 => mi1.GetParameters().Select(pi => pi.ParameterType).Concat(mi1.Ret()).ToArray();
                    return exposeSig(sig).AllMatch(exposeSig(mi), (t1, t2, i) =>
                    {
                        // Func<_, _> matches f: int -> int
                        // but does not match f: int -> void
                        if (t1 == typeof(_))
                        {
                            if (i < mi.GetParameters().Length)
                            {
                                return true;
                            }
                            else
                            {
                                return mi.ReturnType != typeof(void);
                            }
                        }
                        else if (t1.SameMetadataToken(typeof(_<>)))
                        {
                            var t_unwrapped = t1.XGetGenericArguments().Single();
                            // todo. this implementation is very incomplete
                            return t_unwrapped.IsAssignableFrom(t2);
                        }
                        else
                        {
                            return t1 == t2;
                        }
                    });
                });

            // 2. Check number of matches and make sure that there's only one that fits
            var match = mis.SingleOrDefault2();
            if (mis.IsEmpty())
            {
                return (T)(Object)null;
            }
            else if (mis.Count() >= 2)
            {
                throw new AmbiguousMatchException(String.Format(
                    "Resolving method '{1}' of signature '{2}' at type '{3}' with flags '{4}' " +
                        "was ambiguous:{0}{5}",
                    Environment.NewLine,
                    name,
                    typeof(T).GetFunctionSignature(),
                    t.FullName,
                    flags,
                    mis.StringJoin(Environment.NewLine)));
            }

            // 3. Create a static dynamic method that will be used for invocations
            // note. we need to treat static and instance invocations differently!
            // note2. it's essential to pass "true" as last parameter of dynamicmethod's ctor
            // otherwise, non-public invocations will crash with MethodAccessException
            DynamicMethod dyna;
            if (match.IsStatic)
            {
                dyna = new DynamicMethod(
                    String.Format("<>LCG__{0}__type:{1}__sig:{2}", name, t.FullName, typeof(T).FullName),
                    sigRetval, sigArgs, t, true); // nb! true is essential for accessing private stuff
                match.GetParameters().ForEach((pi, i) => dyna.DefineParameter(i + 1, ParmA.None, pi.Name));
            }
            else
            {
                dyna = new DynamicMethod(
                    String.Format("<>LCG__{0}__type:{1}__sig:{2}", name, t.FullName, typeof(T).FullName),
                    sigRetval, match.DeclaringType.Concat(sigArgs).ToArray(), t, true); // nb! true is essential for accessing private stuff
                dyna.DefineParameter(1, ParmA.None, "@this");
                match.GetParameters().ForEach((pi, i) => dyna.DefineParameter(i + 2, ParmA.None, pi.Name));
            }

            // 4.1. Load "this" onto the stack (if necessary)
            var il = dyna.GetILGenerator();
            (match.IsStatic ^ target != null).AssertTrue();
            if (!match.IsStatic) il.ldarg(0);

            // 4.2. Load args onto the stack (converting from _'s if necessary)
            var thisInducedOffset = match.IsStatic ? 0 : 1;
            for (var i = 0; i < sig.GetParameters().Length; i++)
            {
                il.ldarg(i + thisInducedOffset);

                var pi = sig.GetParameters()[i];
                if (pi.ParameterType == typeof(_) ||
                    pi.ParameterType.SameMetadataToken(typeof(_<>)))
                {
                    Label ifNull, afterInit;
                    il.def_label(out ifNull).def_label(out afterInit);

                    il.dup()
                      .brfalse(ifNull)
                      .callvirt(pi.ParameterType.GetProperty("Value").GetGetMethod(true))
                      .br(afterInit)
                      .label(ifNull)
                      .pop()
                      .ldnull()
                      .label(afterInit);
                }
            }

            // 4.3. Call the bound method
            // default behavior of reflection is callvirt, so we do the same here
            il.callvirt(match);

            // 4.4. Process the return result if any exists, and wrap it in _'s if necessary
            if (match.ReturnType == typeof(void))
            {
                var ret = sig.Ret();
                if (ret == typeof(_))
                {
                    throw AssertionHelper.Fail();
                }
                else
                {
                    (ret == typeof(void)).AssertTrue();
                }

                il.ret();
            }
            else
            {
                var ret = sig.Ret();

                if (ret == typeof(_))
                {
                    if (match.ReturnType.IsValueType) il.box(match.ReturnType);
                    il.newobj(typeof(_), typeof(Object));
                }
                else if (ret.SameMetadataToken(typeof(_<>)))
                {
                    il.newobj(ret, ret.XGetGenericArguments().Single());
                }

                il.ret();
            }

            // 5. Yield the delegate
            if (match.IsStatic)
            {
                return (T)(Object)dyna.CreateDelegate(typeof(T));
            }
            else
            {
                return (T)(Object)dyna.CreateDelegate(typeof(T), target);
            }
        }
    }
}