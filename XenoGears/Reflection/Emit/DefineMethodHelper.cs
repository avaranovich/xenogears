using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using XenoGears.Functional;
using XenoGears.Assertions;

namespace XenoGears.Reflection.Emit
{
    [DebuggerNonUserCode]
    public static class DefineMethodHelper
    {
        private static MethodBuilder DefineMethod(this TypeBuilder t, String name, MethodAttributes attrs, Delegate @delegate)
        {
            var implementor = @delegate.Method;
            var args = @delegate.Method.GetParameters();
            var thisArg = args.SingleOrDefault(pi => pi.Name == "this");
            var realArgs = args.Except(thisArg);

            // 1. validation:
            //   * we cannot support non-static lambdas since they require closure thunks
            //   * if we define a static method, lambda cannot define the @this argument
            var @static = ((attrs & MethodAttributes.Static) == MethodAttributes.Static) ? 1 : 0;
            implementor.IsStatic.AssertTrue();
            (thisArg != null && @static == 1).AssertFalse();

            // 2. now we define a method and copy its parameters types and names
            var m = t.DefineMethod(name, attrs, implementor.ReturnType, realArgs.Select(arg => arg.ParameterType).ToArray());
            // define parameter always indexes parameters starting from 1 - regardless of whether the method is static or not
            realArgs.ForEach((pi, i) => m.DefineParameter(i + 1, pi.Attributes, pi.Name));

            // 3. compose the mapping from method's args to implementor's args
            var m2iArgMap = new Dictionary<int, int>();
            if (thisArg != null && @static == 0) m2iArgMap.Add(0, thisArg.Position);
            realArgs.ForEach((pi, i) => m2iArgMap.Add(i + 1 - @static, pi.Position));

            // 3. call MethodBase.Invoke to invoke the lambda
            // we cannot use call here since lambdas are usually private
            m.il().ld_method_info(implementor); // 1. method = lambda's implementor
            m.il().ldnull(); // 2. target = null, since we only support static lambdas
            LocalBuilder thunk; // 3. args packed into Object[]
            m.il().ldc_i4(args.Count()).newarr(typeof(Object)).def_local(typeof(Object[]), out thunk).stloc(thunk);
            var margsOrder = m2iArgMap.OrderBy(kvp => kvp.Value).Select(kvp => kvp.Key);
            Func<int, Type> margType = i_i => args.Nth(i_i).ParameterType;
            margsOrder.ForEach((m_i, i_i) => m.il().ldloc(thunk).ldc_i4(i_i).ldarg(m_i).stelem(margType(i_i)));
            m.il().ldloc(thunk);

            m.il().call(typeof(MethodBase).GetMethod("Invoke", new[] { typeof(Object), typeof(Object[]) }));
            if (implementor.ReturnType == typeof(void)) m.il().pop();
            m.il().ret();

            return m;
        }

        public static MethodBuilder DefineMethod(this TypeBuilder t, String name, MethodAttributes attrs, Action lambda)
        {
            return t.DefineMethod(name, attrs, (Delegate)lambda);
        }

        public static MethodBuilder DefineMethod<T1>(this TypeBuilder t, String name, MethodAttributes attrs, Action<T1> lambda)
        {
            return t.DefineMethod(name, attrs, (Delegate)lambda);
        }

        public static MethodBuilder DefineMethod<T1, T2>(this TypeBuilder t, String name, MethodAttributes attrs, Action<T1, T2> lambda)
        {
            return t.DefineMethod(name, attrs, (Delegate)lambda);
        }

        public static MethodBuilder DefineMethod<T1, T2, T3>(this TypeBuilder t, String name, MethodAttributes attrs, Action<T1, T2, T3> lambda)
        {
            return t.DefineMethod(name, attrs, (Delegate)lambda);
        }

        public static MethodBuilder DefineMethod<T1, T2, T3, T4>(this TypeBuilder t, String name, MethodAttributes attrs, Action<T1, T2, T3, T4> lambda)
        {
            return t.DefineMethod(name, attrs, (Delegate)lambda);
        }

        public static MethodBuilder DefineMethod<T1>(this TypeBuilder t, String name, MethodAttributes attrs, Func<T1> lambda)
        {
            return t.DefineMethod(name, attrs, (Delegate)lambda);
        }

        public static MethodBuilder DefineMethod<T1, T2>(this TypeBuilder t, String name, MethodAttributes attrs, Func<T1, T2> lambda)
        {
            return t.DefineMethod(name, attrs, (Delegate)lambda);
        }

        public static MethodBuilder DefineMethod<T1, T2, T3>(this TypeBuilder t, String name, MethodAttributes attrs, Func<T1, T2, T3> lambda)
        {
            return t.DefineMethod(name, attrs, (Delegate)lambda);
        }

        public static MethodBuilder DefineMethod<T1, T2, T3, T4>(this TypeBuilder t, String name, MethodAttributes attrs, Func<T1, T2, T3, T4> lambda)
        {
            return t.DefineMethod(name, attrs, (Delegate)lambda);
        }
    }
}
