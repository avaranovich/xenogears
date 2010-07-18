using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Reflection.Simple;
using XenoGears.Reflection.Typed;

namespace XenoGears.Reflection.Emit.Hackarounds
{
    [DebuggerNonUserCode]
    public static class ILTrait_WithHackaround
    {
        public static Type CreateType(this TypeBuilder t, bool emitDebugInfo)
        {
            var module = t.Get("m_module").AssertCast<ModuleBuilder>();
            var moduleIsCapableOfEmittingDebugInfo = module.GetSymWriter() != null;
            emitDebugInfo.AssertImplies(moduleIsCapableOfEmittingDebugInfo);

            if (emitDebugInfo)
            {
                return t.CreateType();
            }
            else
            {
                var slot = module.GetSlot<ISymbolWriter>("m_iSymWriter");
                var bak = slot.Value;

                try
                {
                    slot.Value = null;
                    return t.CreateType();
                }
                finally 
                {
                    slot.Value = bak;
                }
            }
        }

        public static ILGenerator EmitCall_Hackaround(this ILGenerator il, OpCode opcode, MethodBase mb)
        {
            var mscorlib_ver = typeof(String).Assembly.GetName().Version;
            if (mscorlib_ver.Major < 4)
            {
                if (mb.IsSafeForEmit())
                {
                    il.EmitCall(opcode, mb.AssertCast<MethodInfo>(), null);
                    return il;
                }
                else
                {
                    if (il.GetType().Name == "DynamicILGenerator")
                    {
                        il.EmitCall(opcode, mb.AssertCast<MethodInfo>(), null);
                        return il;
                    }
                    else
                    {
                        // todo #1. also take into account that we need to update stacksize
                        // todo #2. what is RecordTokenFixup()?

                        var token = il.GetMethodToken_Hackaround(mb);
                        var operand = BitConverter.GetBytes(token);
                        return il.raw(opcode, operand);
                    }
                }
            }
            else
            {
                il.EmitCall(opcode, mb.AssertCast<MethodInfo>(), null);
                return il;
            }
        }
        public static MethodBuilder OverrideMethod_WithHackaround(this TypeBuilder source, MethodBase parentMethod)
        {
            return OverrideMethod_WithHackaround(source, parentMethod, null);
        }

        public static MethodBuilder OverrideMethod_WithHackaround(this TypeBuilder source, MethodBase parentMethod, Func<ILGenerator, ILGenerator> body)
        {
            return OverrideMethod_WithHackaround(source, parentMethod, body, null);
        }

        public static MethodBuilder OverrideMethod_WithHackaround(this TypeBuilder source, MethodBase parentMethod, Func<ILGenerator, ILGenerator> body, IDictionary<MethodBase, MethodBuilder> map)
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
                parentMethod.Ret().SafeTypeForEmit(),
                parentMethod.Params().Select(t_arg => t_arg.SafeTypeForEmit()).ToArray());
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
