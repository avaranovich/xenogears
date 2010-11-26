using System;
using System.Reflection;
using System.Reflection.Emit;
using XenoGears.Assertions;

namespace XenoGears.Reflection.Emit
{
    public static partial class ILTrait
    {
        public static ILGenerator @if(this ILGenerator il, bool condition, Func<ILGenerator, ILGenerator> @true)
        {
            var endBlock = il.DefineLabel();

            // it's possible to calculate length of the "@true(il)" block by visiting delegate body
            // but i'm too lazy to do that :) thus short jumps aren't used
            il.Emit(condition ? OpCodes.Brfalse : OpCodes.Brtrue, endBlock);
            @true(il);
            il.MarkLabel(endBlock);

            return il;
        }

        public static ILGenerator @if(this ILGenerator il, bool condition, Func<ILGenerator, ILGenerator> @true, Func<ILGenerator, ILGenerator> @false)
        {
            var elseMarker = il.DefineLabel();
            var endifMarker = il.DefineLabel();

            il.Emit(condition ? OpCodes.Brfalse : OpCodes.Brtrue, elseMarker);
            @true(il);
            il.Emit(OpCodes.Br, endifMarker);
            il.MarkLabel(elseMarker);
            @false(il);
            il.MarkLabel(endifMarker);

            return il;
        }

        public static ILGenerator ld_args(this ILGenerator il, int start, int end)
        {
            for (var i = start; i <= end; i++)
                ldarg(il, i);
            return il;
        }

        public static ILGenerator ld_field_info(this ILGenerator il, FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException("fieldInfo");

            // todo. use hackarounds (see the Hackarounds namespace nearby)!
            il.Emit(OpCodes.Ldtoken, fieldInfo);
            il.Emit(OpCodes.Ldtoken, fieldInfo.DeclaringType);
            il.EmitCall(OpCodes.Call, typeof(FieldInfo).GetMethod("GetFieldFromHandle", new[] { typeof(RuntimeFieldHandle), typeof(RuntimeTypeHandle) }), null);
            il.Emit(OpCodes.Castclass, typeof(FieldInfo));

            return il;
        }

        public static ILGenerator ld_method_info(this ILGenerator il, MethodBase methodBase)
        {
            if (methodBase == null) throw new ArgumentNullException("methodBase");

            // todo. use hackarounds (see the Hackarounds namespace nearby)!
            il.Emit(OpCodes.Ldtoken, methodBase.AssertCast<MethodInfo>());
            il.Emit(OpCodes.Ldtoken, methodBase.DeclaringType);
            il.EmitCall(OpCodes.Call, typeof(MethodBase).GetMethod("GetMethodFromHandle", new[] { typeof(RuntimeMethodHandle), typeof(RuntimeTypeHandle) }), null);

            return il;
        }

        public static ILGenerator ld_type_info(this ILGenerator il, Type typeInfo)
        {
            if (typeInfo == null) throw new ArgumentNullException("typeInfo");

            // todo. use hackarounds (see the Hackarounds namespace nearby)!
            il.Emit(OpCodes.Ldtoken, typeInfo);
            il.EmitCall(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new[] { typeof(RuntimeTypeHandle) }), null);

            return il;
        }

        public static ILGenerator macro_call_varargs(this ILGenerator il, MethodBase method, int varargc)
        {
            return il.macro_mkarr(varargc).call(method);
        }

        public static ILGenerator macro_callvirt_varargs(this ILGenerator il, MethodBase method, int varargc)
        {
            return il.macro_mkarr(varargc).callvirt(method);
        }

        public static ILGenerator macro_mkarr(this ILGenerator il, int length)
        {
            return il.macro_mkarr(typeof(Object), length);
        }

        public static ILGenerator macro_mkarr(this ILGenerator il, Type type, int length)
        {
            throw new NotImplementedException();
        }

        public static ILGenerator macro_invoke(this ILGenerator il, String name, int argc)
        {
            throw new NotImplementedException();
        }

        public static ILGenerator macro_getslot(this ILGenerator il, String name)
        {
            throw new NotImplementedException();
        }

        public static ILGenerator macro_getslot(this ILGenerator il, String name, Type declaringType)
        {
            throw new NotImplementedException();
        }

        public static ILGenerator macro_setslot(this ILGenerator il, String name)
        {
            throw new NotImplementedException();
        }

        public static ILGenerator macro_setslot(this ILGenerator il, String name, Type declaringType)
        {
            throw new NotImplementedException();
        }
    }
}
