using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using XenoGears.Assertions;

namespace XenoGears.Reflection.Emit
{
    [DebuggerNonUserCode]
    public static partial class ILTrait
    {
        public static ILGenerator DefineLocal(this ILGenerator il, Type type, out LocalBuilder variableInfo)
        {
            variableInfo = il.DeclareLocal(type);
            return il;
        }

        public static ILGenerator DefineLabel(this ILGenerator il, out Label label)
        {
            label = il.DefineLabel();
            return il;
        }

        public static ILGenerator Apply(this ILGenerator il, Func<ILGenerator, ILGenerator> coder)
        {
            if (coder != null) coder(il);
            return il;
        }

        public static ILGenerator add(this ILGenerator il)
        {
            il.Emit(OpCodes.Add);
            return il;
        }

        public static ILGenerator and(this ILGenerator il)
        {
            il.Emit(OpCodes.And);
            return il;
        }

        public static ILGenerator box(this ILGenerator il, Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            // the line below forbids usage of .box in expressions creation
            // if (!type.IsValueType) throw new ArgumentException("Value type expected", "type");

            il.Emit(OpCodes.Box, type);
            return il;
        }

        public static ILGenerator br(this ILGenerator il, Label label)
        {
            il.Emit(OpCodes.Br, label);
            return il;
        }

        public static ILGenerator br_s(this ILGenerator il, Label label)
        {
            il.Emit(OpCodes.Br_S, label);
            return il;
        }


        public static ILGenerator brfalse(this ILGenerator il, Label label)
        {
            il.Emit(OpCodes.Brfalse, label);
            return il;
        }

        public static ILGenerator brfalse_s(this ILGenerator il, Label label)
        {
            il.Emit(OpCodes.Brfalse_S, label);
            return il;
        }

        public static ILGenerator brtrue(this ILGenerator il, Label label)
        {
            il.Emit(OpCodes.Brtrue, label);
            return il;
        }

        public static ILGenerator brtrue_s(this ILGenerator il, Label label)
        {
            il.Emit(OpCodes.Brtrue_S, label);
            return il;
        }

        public static ILGenerator call(this ILGenerator il, MethodBase method)
        {
            il.EmitCall(OpCodes.Call, (MethodInfo)method, null);
            return il;
        }

        public static ILGenerator callvirt(this ILGenerator il, MethodBase method)
        {
            // WHL-382 - it isn't convenitent to check out whether a method is virtual, thus we're doing it here
            il.EmitCall(method.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, (MethodInfo)method, null);
            return il;
        }

        public static ILGenerator ceq(this ILGenerator il)
        {
            il.Emit(OpCodes.Ceq);
            return il;
        }

        public static ILGenerator cgt(this ILGenerator il)
        {
            il.Emit(OpCodes.Cgt);
            return il;
        }

        public static ILGenerator clt(this ILGenerator il)
        {
            il.Emit(OpCodes.Clt);
            return il;
        }

        public static ILGenerator cne(this ILGenerator il)
        {
            return il.ceq().cfalse();
        }

        public static ILGenerator cge(this ILGenerator il)
        {
            return il.clt().cfalse();
        }

        public static ILGenerator cle(this ILGenerator il)
        {
            return il.cgt().cfalse();
        }

        public static ILGenerator cfalse(this ILGenerator il)
        {
            return il.ldc_i4(0).ceq();
        }

        public static ILGenerator ctrue(this ILGenerator il)
        {
            return il.ldc_i4(1).ceq();
        }

        public static ILGenerator cmp(this ILGenerator il, PredicateType pred)
        {
            switch (pred)
            {
                case PredicateType.Equal:
                    return il.ceq();
                case PredicateType.GreaterThan:
                    return il.cgt();
                case PredicateType.GreaterThanOrEqual:
                    return il.cge();
                case PredicateType.LessThan:
                    return il.clt();
                case PredicateType.LessThanOrEqual:
                    return il.cle();
                case PredicateType.NotEqual:
                    return il.cne();
                case PredicateType.IsFalse:
                    return il.cne();
                case PredicateType.IsTrue:
                    return il.cne();
                default:
                    throw AssertionHelper.Fail();
            }
        }

        public static ILGenerator castclass(this ILGenerator il, Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            il.Emit(OpCodes.Castclass, type);
            return il;
        }

        public static ILGenerator constrained(this ILGenerator il, Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            il.Emit(OpCodes.Constrained, type);
            return il;
        }

        public static ILGenerator convert(this ILGenerator il, Type source, Type destination)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (destination == null) throw new ArgumentNullException("destination");

            if (source == destination) return il;

            if (source == typeof(Object) && destination.IsValueType) return il.unbox_any(destination);
            if (source.IsValueType && destination == typeof(Object)) return il.box(destination);

            // if (source.IsAssignableFrom(destination)) return this;
            // --> it doesn't work for int? -> int, cause int is assignable from int?

            var converter = LookUpConverter(source, destination);
            if (converter != null) // not so beauty, but it's enough for internal code
            {
                // todo. implement invariant culture here
                if (converter is ConstructorInfo) return il.newobj((ConstructorInfo) converter);
                // note the ClassCastException expected below in near future :)
                return converter.IsVirtual ? il.callvirt((MethodInfo) converter) : il.call((MethodInfo) converter);
            }

            Func<ILGenerator, ILGenerator> emitter;
            if (CanGenerateConverter(source, destination, out emitter)) return emitter(il);

            return il.castclass(destination);
        }

        public static ILGenerator debugger(this ILGenerator il)
        {
            il.Emit(OpCodes.Break);
            return il;
        }

        public static ILGenerator div(this ILGenerator il)
        {
            il.Emit(OpCodes.Div);
            return il;
        }

        public static ILGenerator dup(this ILGenerator il)
        {
            il.Emit(OpCodes.Dup);
            return il;
        }

        public static ILGenerator initobj(this ILGenerator il, Type valueType)
        {
            il.Emit(OpCodes.Initobj, valueType);
            return il;
        }

        public static ILGenerator isinst(this ILGenerator il, Type valueType)
        {
            il.Emit(OpCodes.Isinst, valueType);
            return il;
        }

        public static ILGenerator label(this ILGenerator il, Label label)
        {
            il.MarkLabel(label);
            return il;
        }

        public static ILGenerator ld_args(this ILGenerator il, int count)
        {
            for (var i = 0; i < count; i++)
                ldarg(il, i);
            return il;
        }

        public static ILGenerator ldarg(this ILGenerator il, int index)
        {
            if (index < 4)
                switch (index)
                {
                    case 0:
                        il.Emit(OpCodes.Ldarg_0);
                        return il;
                    case 1:
                        il.Emit(OpCodes.Ldarg_1);
                        return il;
                    case 2:
                        il.Emit(OpCodes.Ldarg_2);
                        return il;
                    case 3:
                        il.Emit(OpCodes.Ldarg_3);
                        return il;
                    default:
                        throw new ArgumentOutOfRangeException("index", "Index should not be negative");
                }

            if (index > byte.MaxValue) il.Emit(OpCodes.Ldarg, index);
            else il.Emit(OpCodes.Ldarg_S, (byte) index);

            return il;
        }

        public static ILGenerator ldftn(this ILGenerator il, MethodBase mb)
        {
            il.Emit(OpCodes.Ldftn, mb.AssertCast<MethodInfo>());
            return il;
        }

        public static ILGenerator ldtrue(this ILGenerator il)
        {
            return il.ldc_i4(1);
        }

        public static ILGenerator ldfalse(this ILGenerator il)
        {
            return il.ldc_i4(0);
        }

        public static ILGenerator ldc_i4(this ILGenerator il, int constant)
        {
            if (constant < 9)
                if (constant > -2)
                    switch (constant)
                    {
                        case 0:
                            il.Emit(OpCodes.Ldc_I4_0);
                            return il;
                        case 1:
                            il.Emit(OpCodes.Ldc_I4_1);
                            return il;
                        case 2:
                            il.Emit(OpCodes.Ldc_I4_2);
                            return il;
                        case 3:
                            il.Emit(OpCodes.Ldc_I4_3);
                            return il;
                        case 4:
                            il.Emit(OpCodes.Ldc_I4_4);
                            return il;
                        case 5:
                            il.Emit(OpCodes.Ldc_I4_5);
                            return il;
                        case 6:
                            il.Emit(OpCodes.Ldc_I4_6);
                            return il;
                        case 7:
                            il.Emit(OpCodes.Ldc_I4_7);
                            return il;
                        case 8:
                            il.Emit(OpCodes.Ldc_I4_8);
                            return il;
                        case -1:
                            il.Emit(OpCodes.Ldc_I4_M1);
                            return il;
                    }
                else
                {
                    il.Emit(OpCodes.Ldc_I4, constant);
                    return il;
                }

            if (constant > sbyte.MaxValue || constant < sbyte.MinValue) il.Emit(OpCodes.Ldc_I4, constant);
            else il.Emit(OpCodes.Ldc_I4_S, (sbyte) constant);

            return il;
        }

        public static ILGenerator ldc_i8(this ILGenerator il, long constant)
        {
            il.Emit(OpCodes.Ldc_I8, constant);
            return il;
        }

        public static ILGenerator ldc_r4(this ILGenerator il, float constant)
        {
            il.Emit(OpCodes.Ldc_R4, constant);
            return il;
        }

        public static ILGenerator ldc_r8(this ILGenerator il, double constant)
        {
            il.Emit(OpCodes.Ldc_R8, constant);
            return il;
        }

        public static ILGenerator lddefault(this ILGenerator il, Type type)
        {
            if (type == typeof(void)) return il; // usualy used in props autogen

            if (type.IsPrimitive)
            {
                if (typeof(bool) == type || typeof(byte) == type || typeof(sbyte) == type || typeof(short) == type ||
                    typeof(ushort) == type || typeof(int) == type || typeof(uint) == type || typeof(char) == type)
                    return il.ldc_i4(0);

                if (typeof(float) == type) return il.ldc_r4(0);
                if (typeof(double) == type) return il.ldc_r8(0.0);

                if (typeof(long) == type || typeof(ulong) == type) return il.ldc_i8(0);

                throw new ArgumentOutOfRangeException("type", "Unexpected primitive type: " + type);
            }

            if (type.IsValueType)
            {
                var variable = il.DeclareLocal(type);
                return il.ldloca(variable).initobj(type).ldloc(variable);
            }

            return il.ldnull();
        }

        public static ILGenerator ldelem(this ILGenerator il, Type t)
        {
            il.Emit(OpCodes.Ldelem, t);
            return il;
        }

        public static ILGenerator ldelem_ref(this ILGenerator il)
        {
            il.Emit(OpCodes.Ldelem_Ref);
            return il;
        }

        public static ILGenerator ldfld(this ILGenerator il, FieldInfo field)
        {
            // todo. use hackarounds (see the Hackarounds namespace nearby)!
            il.Emit(field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, field);
            return il;
        }

        public static ILGenerator ldloc(this ILGenerator il, LocalVariableInfo variable)
        {
            if (variable.LocalIndex < 4)
                switch (variable.LocalIndex)
                {
                    case (0):
                        il.Emit(OpCodes.Ldloc_0);
                        return il;
                    case (1):
                        il.Emit(OpCodes.Ldloc_1);
                        return il;
                    case (2):
                        il.Emit(OpCodes.Ldloc_2);
                        return il;
                    case (3):
                        il.Emit(OpCodes.Ldloc_3);
                        return il;
                    default:
                        throw new ArgumentOutOfRangeException("variable", "Variable index should be positive");
                }

            if (variable.LocalIndex > byte.MaxValue)
                il.Emit(OpCodes.Ldloc, variable.LocalIndex);
            else
                il.Emit(OpCodes.Ldloc_S, (byte) variable.LocalIndex);
            return il;
        }

        public static ILGenerator ldloca(this ILGenerator il, LocalVariableInfo variable)
        {
            if (variable.LocalIndex > byte.MaxValue)
                il.Emit(OpCodes.Ldloca, variable.LocalIndex);
            else
                il.Emit(OpCodes.Ldloca_S, (byte) variable.LocalIndex);
            return il;
        }

        public static ILGenerator ldnull(this ILGenerator il)
        {
            il.Emit(OpCodes.Ldnull);
            return il;
        }

        public static ILGenerator ldtoken(this ILGenerator il, Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            // todo. use hackarounds (see the Hackarounds namespace nearby)!
            il.Emit(OpCodes.Ldtoken, type);
            return il;
        }

        public static ILGenerator ldtoken(this ILGenerator il, MethodBase methodBase)
        {
            if (methodBase == null) throw new ArgumentNullException("methodBase");

            // todo. use hackarounds (see the Hackarounds namespace nearby)!
            il.Emit(OpCodes.Ldtoken, methodBase.AssertCast<MethodInfo>());
            return il;
        }

        public static ILGenerator ldtoken(this ILGenerator il, FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException("fieldInfo");

            // todo. use hackarounds (see the Hackarounds namespace nearby)!
            il.Emit(OpCodes.Ldtoken, fieldInfo);
            return il;
        }

        public static ILGenerator ldstr(this ILGenerator il, String constant)
        {
            if (constant == null) return il.ldnull();

            il.Emit(OpCodes.Ldstr, constant);
            return il;
        }

        public static ILGenerator mul(this ILGenerator il)
        {
            il.Emit(OpCodes.Mul);
            return il;
        }

        public static ILGenerator neg(this ILGenerator il)
        {
            il.Emit(OpCodes.Neg);
            return il;
        }

        public static ILGenerator newarr(this ILGenerator il, Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            // todo. use hackarounds (see the Hackarounds namespace nearby)!
            il.Emit(OpCodes.Newarr, type);
            return il;
        }

        public static ILGenerator newobj(this ILGenerator il, MethodBase ctor)
        {
            if (ctor == null) throw new ArgumentNullException("ctor");

            // todo. use hackarounds (see the Hackarounds namespace nearby)!
            il.Emit(OpCodes.Newobj, ctor.AssertCast<ConstructorInfo>());
            return il;
        }

        public static ILGenerator newobj(this ILGenerator il, Type type, params Type[] ctorParams)
        {
            if (type == null) throw new ArgumentNullException("type");

            var ctor = type.GetConstructor(ctorParams);
            if (ctor != null) return il.newobj(ctor);

            ctor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Standard, ctorParams, null);
            if (ctor != null) return il.newobj(ctor);

            throw new ArgumentException(String.Format("No such .ctor({1}) for type {0}.", type, ctorParams));
        }

        public static ILGenerator nop(this ILGenerator il)
        {
            il.Emit(OpCodes.Nop);
            return il;
        }

        public static ILGenerator not(this ILGenerator il)
        {
            il.Emit(OpCodes.Not);
            return il;
        }

        public static ILGenerator or(this ILGenerator il)
        {
            il.Emit(OpCodes.Or);
            return il;
        }

        public static ILGenerator pop(this ILGenerator il)
        {
            il.Emit(OpCodes.Pop);
            return il;
        }

        public static ILGenerator rem(this ILGenerator il)
        {
            il.Emit(OpCodes.Rem);
            return il;
        }

        public static ILGenerator ret(this ILGenerator il)
        {
            il.Emit(OpCodes.Ret);
            return il;
        }

        public static ILGenerator shl(this ILGenerator il)
        {
            il.Emit(OpCodes.Shl);
            return il;
        }

        public static ILGenerator shr(this ILGenerator il)
        {
            il.Emit(OpCodes.Shr);
            return il;
        }

        public static ILGenerator @sizeof(this ILGenerator il, Type t)
        {
            il.Emit(OpCodes.Sizeof, t);
            return il;
        }

        public static ILGenerator starg(this ILGenerator il, ParameterInfo pi)
        {
            return il.starg(pi.Position);
        }

        public static ILGenerator starg(this ILGenerator il, int index)
        {
            if (index > byte.MaxValue)
                il.Emit(OpCodes.Starg, index);
            else
                il.Emit(OpCodes.Starg_S, index);

            return il;
        }

        public static ILGenerator stelem(this ILGenerator il, Type t)
        {
            il.Emit(OpCodes.Stelem, t);
            return il;
        }

        public static ILGenerator stelem_ref(this ILGenerator il)
        {
            il.Emit(OpCodes.Stelem_Ref);
            return il;
        }

        public static ILGenerator stfld(this ILGenerator il, FieldInfo field)
        {
            // todo. use hackarounds (see the Hackarounds namespace nearby)!
            il.Emit(field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field);
            return il;
        }

        public static ILGenerator stloc(this ILGenerator il, LocalBuilder variable)
        {
            if (variable == null) return il; // do nothing for void

            if (variable.LocalIndex < 4)
            {
                switch (variable.LocalIndex)
                {
                    case 0:
                        il.Emit(OpCodes.Stloc_0);
                        return il;
                    case 1:
                        il.Emit(OpCodes.Stloc_1);
                        return il;
                    case 2:
                        il.Emit(OpCodes.Stloc_2);
                        return il;
                    case 3:
                        il.Emit(OpCodes.Stloc_3);
                        return il;
                    default:
                        throw new NotImplementedException("An unexpected place :(");
                }
            }

            if (variable.LocalIndex > byte.MaxValue)
                il.Emit(OpCodes.Stloc, variable);
            else
                il.Emit(OpCodes.Stloc_S, variable);

            return il;
        }

        public static ILGenerator sub(this ILGenerator il)
        {
            il.Emit(OpCodes.Sub);
            return il;
        }

        public static ILGenerator @throw(this ILGenerator il)
        {
            il.Emit(OpCodes.Throw);
            return il;
        }

        public static ILGenerator @throw(this ILGenerator il, Type exceptionType, params Type[] ctorParamTypes)
        {
            if (exceptionType == null) throw new ArgumentNullException("exceptionType");
            if (!typeof(Exception).IsAssignableFrom(exceptionType)) throw new ArgumentException("Exception type expected.", "exceptionType");

            var constructor = exceptionType.GetConstructor(ctorParamTypes);
            if (constructor == null) throw new ArgumentNullException("exceptionType", "No ctor found");

            return il.newobj(constructor).@throw();
        }

        public static ILGenerator unbox(this ILGenerator il, Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!type.IsValueType) throw new ArgumentException("Value type expected", "type");

            il.Emit(OpCodes.Unbox, type);
            return il;
        }

        public static ILGenerator unbox_any(this ILGenerator il, Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            il.Emit(OpCodes.Unbox_Any, type);
            return il;
        }

        public static ILGenerator xor(this ILGenerator il)
        {
            il.Emit(OpCodes.Xor);
            return il;
        }
    }
}