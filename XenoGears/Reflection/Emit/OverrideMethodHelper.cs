using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Reflection.Emit
{
    [DebuggerNonUserCode]
    public static class OverrideMethodHelper
    {
        public static MethodBuilder OverrideMethod(this TypeBuilder source, MethodBase parentMethod)
        {
            return OverrideMethod(source, parentMethod, null);
        }

        public static MethodBuilder OverrideMethod(this TypeBuilder source, MethodBase parentMethod, Func<ILGenerator, ILGenerator> body)
        {
            return OverrideMethod(source, parentMethod, body, null);
        }

        public static MethodBuilder OverrideMethod(this TypeBuilder source, MethodBase parentMethod, Func<ILGenerator, ILGenerator> body, IDictionary<MethodBase, MethodBuilder> map)
        {
            // don't defer this check since default error message doesn't say
            // what exact method is the reason of the failure to compile the class
            parentMethod.IsVirtual.AssertTrue();
            parentMethod.IsFinal.AssertFalse();

            var attrs = parentMethod.Attributes;
            attrs &= ~MethodAttributes.NewSlot;
            attrs &= ~MethodAttributes.Abstract;

            var derived = source.DefineMethod(
                // that's an awesome idea but it hurts reflector and debuggability
//                String.Format("{0}_{1}", parentMethod.Name, parentMethod.DeclaringType.ToShortString()),
                parentMethod.Name,
                attrs,
                parentMethod.Ret(),
                parentMethod.Params());
            parentMethod.GetParameters().ForEach((pi, i) => derived.DefineParameter(i + 1, ParmA.None, pi.Name));

            // the stuff below ain't necessary at all since we don't change the name
//            // note. the checks are very important since otherwise we get an exception:
//            // System.TypeLoadException: Signature of the body and declaration in a method implementation do not match.
//            if (!parentMethod.IsAbstract && !parentMethod.IsGenericMethod)
//            {
//                source.DefineMethodOverride(derived, parentMethod);
//            }

            if (body != null) body(derived.il());
            if (map != null) map[parentMethod] = derived;
            return derived;
        }
    }
}
