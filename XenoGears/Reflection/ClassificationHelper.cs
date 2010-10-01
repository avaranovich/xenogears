using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection.Attributes;
using XenoGears.Reflection.Emit;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Reflection
{
    [DebuggerNonUserCode]
    public static class ClassificationHelper
    {
        public static bool IsArray(this Object o)
        {
            return o != null && o.GetType().IsArray;
        }

        public static bool IsArray(this Type t)
        {
            if (t == null) return false;
            return t != null && t.IsArray;
        }

        public static bool Is1dArray(this Object o)
        {
            return o != null && o.GetType().Is1dArray();
        }

        public static bool Is1dArray(this Type t)
        {
            if (t == null) return false;
            return t.IsArray && t.GetArrayRank() == 1 && !t.GetElementType().IsArray;
        }

        public static bool IsRectMdArray(this Object o)
        {
            return o != null && o.GetType().IsRectMdArray();
        }

        public static bool IsRectMdArray(this Type t)
        {
            if (t == null) return false;
            return t.IsArray && t.GetArrayRank() > 1;
        }

        public static bool IsJaggedMdArray(this Object o)
        {
            return o != null && o.GetType().IsJaggedMdArray();
        }

        public static bool IsJaggedMdArray(this Type t)
        {
            if (t == null) return false;
            return t.IsArray && t.GetArrayRank() == 1 && t.GetElementType().IsArray;
        }

        public static bool IsEnumerableType(this Type t)
        {
            if (t == null) return false;
            return t.GetEnumerableElement() != null;
        }

        public static bool IsListType(this Type t)
        {
            if (t == null) return false;
            return t.GetListElement() != null;
        }

        public static bool IsDictionaryType(this Type t)
        {
            if (t == null) return false;
            return t.GetDictionaryElement() != null;
        }

        public static bool IsEnumerableOf<T>(this Type t)
        {
            if (t == null) return false;
            return t.IsEnumerableOf(typeof(T));
        }

        public static bool IsListOf<T>(this Type t)
        {
            if (t == null) return false;
            return t.IsListOf(typeof(T));
        }

        public static bool IsDictionaryOf<K, V>(this Type t)
        {
            if (t == null) return false;
            return t.IsDictionaryOf(typeof(K), typeof(V));
        }

        public static bool IsEnumerableOf(this Type t, Type elType)
        {
            if (t == null || elType == null) return false;
            return t.IsEnumerableType() && t.GetEnumerableElement() == elType;
        }

        public static bool IsListOf(this Type t, Type elType)
        {
            if (t == null || elType == null) return false;
            return t.IsListType() && t.GetListElement() == elType;
        }

        public static bool IsDictionaryOf(this Type t, Type keyType, Type valueType)
        {
            if (t == null || keyType == null || valueType == null) return false;
            return t.IsDictionaryType() && t.GetDictionaryElement().GetType() ==
                typeof(KeyValuePair<,>).XMakeGenericType(keyType, valueType);
        }

        public static Type GetEnumerableElement(this Type t)
        {
            if (t == null) return null;
            var unambiguousEnum =
                t.SameMetadataToken(typeof(IEnumerable<>)) ? t :
                t.GetInterfaces().Where(iface => iface.SameMetadataToken(typeof(IEnumerable<>))).SingleOrDefault2();
            return unambiguousEnum == null ? null : unambiguousEnum.XGetGenericArguments().Single();
        }

        public static Type GetListElement(this Type t)
        {
            var adder = t.GetListAdder();
            return adder == null ? null : adder.Params().AssertSingle();
        }

        public static MethodInfo GetListAdder(this Type t)
        {
            if (t == null) return null;
            var enumOfT = t.GetEnumerableElement();
            if (enumOfT == null)
            {
                return null;
            }
            else
            {
                return t.Hierarchy().SelectMany(ht => ht.GetMethods(BF.All))
                    .FirstOrDefault(m => m.Name == "Add" && Seq.Equal(m.Params(), enumOfT.MkArray()));
            }
        }

        public static KeyValuePair<Type, Type>? GetDictionaryElement(this Type t)
        {
            if (t == null) return null;
            var unambiguousDic =
                t.SameMetadataToken(typeof(IDictionary<,>)) ? t :
                t.GetInterfaces().Where(iface => iface.SameMetadataToken(typeof(IDictionary<,>))).SingleOrDefault2();
            return unambiguousDic == null ? null : (KeyValuePair<Type, Type>?)
                new KeyValuePair<Type, Type>(
                    unambiguousDic.XGetGenericArguments()[0],
                    unambiguousDic.XGetGenericArguments()[1]);
        }

        public static Type GetDictionaryKey(this Type t)
        {
            var kvp_el = t.GetDictionaryElement();
            return kvp_el == null ? null : kvp_el.Value.Key;
        }

        public static Type GetDictionaryValue(this Type t)
        {
            var kvp_el = t.GetDictionaryElement();
            return kvp_el == null ? null : kvp_el.Value.Value;
        }

        public static MethodInfo GetDictionaryAdder(this Type t)
        {
            if (t == null) return null;
            var key = t.GetDictionaryKey();
            var value = t.GetDictionaryValue();
            if (key == null || value == null)
            {
                return null;
            }
            else
            {
                return t.Hierarchy().SelectMany(ht => ht.GetMethods(BF.All))
                    .FirstOrDefault(m => m.Name == "Add" && Seq.Equal(m.Params(), new []{key, value}));
            }
        }
        public static bool IsReferenceType(this Type t)
        {
            if (t == null) return false;
            return t.IsClass || t.IsInterface;
        }

        public static bool IsNonNullableValueType(this Type t)
        {
            if (t == null) return false;
            return t.IsValueType && !t.IsNullable() && t != typeof(void);
        }

        public static bool IsNullable(this Type t)
        {
            if (t == null) return false;
            return t.SameMetadataToken(typeof(Nullable<>));
        }

        public static bool IsNullable(this Object o)
        {
            if (o == null) return false;
            return o.GetType().IsNullable();
        }

        public static Type UndecorateNullable(this Type t)
        {
            if (t == null) return null;
            return t.IsNullable() ? Nullable.GetUnderlyingType(t) : t;
        }

        public static Object UndecorateNullable(this Object o)
        {
            if (o == null) return null;
            return o.GetType().IsNullable() ? o.GetType().GetProperty("Value").GetValue(o, null) : o;
        }

        public static bool IsInteger(this Type t)
        {
            if (t == null) return false;
            return t == typeof(sbyte) || t == typeof(byte) ||
                t == typeof(short) || t == typeof(ushort) ||
                t == typeof(int) || t == typeof(uint) ||
                t == typeof(long) || t == typeof(ulong);
        }

        public static bool IsFloatingPoint(this Type t)
        {
            if (t == null) return false;
            return t == typeof(float) || t == typeof(double);
        }

        public static bool IsNumeric(this Type t)
        {
            if (t == null) return false;
            return t.IsInteger() || t.IsFloatingPoint() || t == typeof(decimal);
        }

        public static bool IsTOrNullableT<T>(this Type t)
            where T : struct
        {
            if (t == null) return false;
            if (t.IsNullable())
            {
                return IsTOrNullableT<T>(t.UndecorateNullable());
            }
            else
            {
                return t == typeof(T);
            }
        }

        public static bool IsEnumOrNullable(this Type t)
        {
            if (t == null) return false;
            if (t.IsNullable())
            {
                return IsEnumOrNullable(t.UndecorateNullable());
            }
            else
            {
                return t.IsEnum;
            }
        }

        public static bool IsOpenGeneric(this Type t)
        {
            if (t == null) return false;
            if (t.IsFType())
            {
                return t.Ret().IsOpenGeneric() || t.Params().Any(arg => arg.IsOpenGeneric());
            }
            else if (t.IsArray)
            {
                return t.GetElementType().IsOpenGeneric();
            }
            else
            {
                return t.IsGenericParameter || t.XGetGenericArguments().Any(arg => arg.IsOpenGeneric());
            }
        }

        public static bool IsOpenGeneric(this MethodBase m)
        {
            if (m == null) return false;
            return m is MethodInfo ? ((MethodInfo)m).IsOpenGeneric() : false;
        }

        public static bool IsOpenGeneric(this MethodInfo m)
        {
            if (m == null) return false;
            return m.ReturnType.IsOpenGeneric() ||
                m.Params().Any(pt => pt.IsOpenGeneric()) ||
                m.ContainsGenericParameters; // example: bool Meth<T>(int x);
        }

        public static bool IsCompilerGenerated(this MemberInfo mi)
        {
            if (mi == null) return false;
            return mi.HasAttr<CompilerGeneratedAttribute>();
        }

        public static bool IsAnonymous(this Type t)
        {
            if (t == null) return false;
            return
                t.HasAttr<CompilerGeneratedAttribute>() &&
                (Regex.IsMatch(t.Name, @"\<\>.*AnonymousType.*") || // C# anonymous types
                t.Name.StartsWith("XenoGearsAnonymousType<")); // XenoGears anonymous types
        }

        public static bool IsTransparentIdentifier(this String s)
        {
            if (s == null) return false;
            return Regex.IsMatch(s, @"\<\>.*TransparentIdentifier.*");
        }

        public static bool IsExtension(this MethodBase mb)
        {
            if (mb == null) return false;
            return mb.HasAttr<ExtensionAttribute>();
        }

        public static bool IsExtension(this PropertyInfo pi)
        {
            if (pi == null) return false;
            // todo. cya in c# 5.0 or when we decompile f#
            return false;
        }

        public static bool IsStatic(this MemberInfo mi)
        {
            if (mi == null) return false;
            if (mi is PropertyInfo)
            {
                return ((PropertyInfo)mi).GetGetMethod(true).IsStatic;
            }
            else if (mi is EventInfo)
            {
                return ((EventInfo)mi).GetAddMethod(true).IsStatic;
            }
            else if (mi is Type)
            {
                var t = (Type) mi;
                return t.IsStatic();
            }
            else
            {
                var pi = mi.GetType().GetProperty("IsStatic", typeof(bool));
                if (pi != null)
                {
                    return (bool)pi.GetValue(mi, null);
                }
                else
                {
                    throw new NotSupportedException(mi.ToString());
                }
            }
        }

        public static bool IsInstance(this MemberInfo mi)
        {
            if (mi == null) return false;
            (mi is Type).AssertFalse();
            return !mi.IsStatic();
        }

        public static bool IsInstance(this MethodBase mi)
        {
            if (mi == null) return false;
            return !mi.IsStatic() && !mi.IsConstructor;
        }

        public static bool IsConstructor(this MethodBase mi)
        {
            if (mi == null) return false;
            return mi.IsConstructor;
        }

        public static Type SlotType(this MemberInfo mi)
        {
            if (mi == null) return null;
            if (mi is PropertyInfo)
            {
                return ((PropertyInfo)mi).PropertyType;
            }
            else if (mi is FieldInfo)
            {
                return ((FieldInfo)mi).FieldType;
            }
            else if (mi is EventInfo)
            {
                return ((EventInfo)mi).EventHandlerType;
            }
            else
            {
                throw new NotSupportedException(mi.ToString());
            }
        }

        public static bool IsStatic(this Type t)
        {
            if (t == null) return false;
            return t.IsSealed && t.IsAbstract;
        }

        public static bool IsVarargs(this MethodBase mi)
        {
            if (mi == null) return false;
            var p_last = mi.GetParameters().LastOrDefault();
            return p_last != null && p_last.IsVarargs();
        }

        public static bool IsVarargs(this ParameterInfo pi)
        {
            if (pi == null) return false;
            return pi.HasAttr<ParamArrayAttribute>();
        }

        public static bool IsIndexer(this PropertyInfo pi)
        {
            if (pi == null) return false;
            if (pi.IsDefaultIndexer()) return true;
            return pi.HasAttr<IndexerNameAttribute>();
        }

        public static bool IsDefaultIndexer(this PropertyInfo pi)
        {
            if (pi == null) return false;
            var defaultMember = pi.DeclaringType.AttrOrNull<DefaultMemberAttribute>();
            return defaultMember != null && defaultMember.MemberName == pi.Name;
        }

        public static bool IsIndexer(this MethodBase mb)
        {
            return mb.EnclosingProperty().IsIndexer();
        }

        public static bool IsDefaultIndexer(this MethodBase mb)
        {
            return mb.EnclosingProperty().IsDefaultIndexer();
        }

        public static bool IsAccessor(this MethodBase mb)
        {
            return mb.IsGetter() || mb.IsSetter();
        }

        public static bool IsGetter(this MethodBase mb)
        {
            if (mb == null) return false;
            var p = mb.EnclosingProperty();
            return p != null && p.GetGetMethod(true) == mb;
        }

        public static bool IsSetter(this MethodBase mb)
        {
            if (mb == null) return false;
            var p = mb.EnclosingProperty();
            return p != null && p.GetSetMethod(true) == mb;
        }

        public static bool IsArrayIndexer(this MethodBase mb)
        {
            return mb.IsArrayGetter() || mb.IsArraySetter();
        }

        public static bool IsArrayGetter(this MethodBase mb)
        {
            // todo. for dimensions higher than 3 arrays of objects define
            // GetValue(params) and SetValue(params) methods => this won't work correctly
            if (mb == null) return false;
            return (mb.DeclaringType.IsArray && mb.Name == "Get") ||
                (mb.DeclaringType == typeof(Array) && mb.Name == "GetValue");
        }

        public static MethodInfo ArrayGetter(this Type t)
        {
            if (t == null || !t.IsArray) return null;

            var t_el = t.GetElementType();
            if (t_el != typeof(Object)) return t.GetMethod("Get", BF.All | BF.DeclOnly);

            // todo. for dimensions higher than 3 arrays of objects define
            // GetValue(params) and SetValue(params) methods => this won't work correctly
            var @params = t.GetArrayRank().Times(typeof(int)).ToArray();
            return typeof(Array).GetMethod("GetValue", @params);
        }

        public static bool IsArraySetter(this MethodBase mb)
        {
            // todo. for dimensions higher than 3 arrays of objects define
            // GetValue(params) and SetValue(params) methods => this won't work correctly
            if (mb == null) return false;
            return (mb.DeclaringType.IsArray && mb.Name == "Set") ||
                (mb.DeclaringType == typeof(Array) && mb.Name == "SetValue");
        }

        public static MethodInfo ArraySetter(this Type t)
        {
            if (t == null || !t.IsArray) return null;

            var t_el = t.GetElementType();
            if (t_el != typeof(Object)) return t.GetMethod("Set", BF.All | BF.DeclOnly);

            // todo. for dimensions higher than 3 arrays of objects define
            // GetValue(params) and SetValue(params) methods => this won't work correctly
            var @params = typeof(Object).Concat(t.GetArrayRank().Times(typeof(int))).ToArray();
            return typeof(Array).GetMethod("SetValue", @params);
        }

        public static Object RuntimeHandle(this MemberInfo mi)
        {
            if (mi == null) return null;
            if (mi is Type)
            {
                return ((Type)mi).TypeHandle;
            }
            else if (mi is MethodBase)
            {
                return ((MethodBase)mi).MethodHandle;
            }
            else if (mi is FieldInfo)
            {
                return ((FieldInfo)mi).FieldHandle;
            }
            else
            {
                throw new NotSupportedException(mi.ToString());
            }
        }

        public static bool HasBody(this MethodBase mb)
        {
            return mb.GetMethodBody() != null;
        }

        public static MethodBase ImplOf(this MethodBase mb)
        {
            var mi = mb as MethodInfo;
            if (mi == null) return null;

            var implOf = mi.Declarations();
            return implOf.FirstOrDefault();
        }

        // note. this is C#-specific
        public static bool IsExplicitImpl(this MethodBase mb)
        {
            var implOf = mb.ImplOf();
            if (implOf == null) return false;

            var t_iface = implOf.DeclaringType.AssertThat(t => t.IsInterface);
            var cs_explicitImplName = t_iface.FullName + "." + implOf.Name;
            return cs_explicitImplName == mb.Name;
        }

        public static MethodBase ExplicitImplOf(this MethodBase mb)
        {
            return mb.IsExplicitImpl() ? mb.ImplOf() : null;
        }

        public static bool IsImplicitImpl(this MethodBase mb)
        {
            var implOf = mb.ImplOf();
            if (implOf == null) return false;
            return !mb.IsExplicitImpl();
        }

        public static MethodBase ImplicitImplOf(this MethodBase mb)
        {
            return mb.IsImplicitImpl() ? mb.ImplOf() : null;
        }

        public static Object IsVirtual(this MemberInfo mi)
        {
            if (mi == null) return null;
            if (mi is MethodBase)
            {
                return ((MethodBase)mi).IsVirtual();
            }
            else if (mi is PropertyInfo)
            {
                return ((PropertyInfo)mi).IsVirtual();
            }
            else
            {
                throw new NotSupportedException(mi.ToString());
            }
        }

        public static bool IsVirtual(this MethodBase mb)
        {
            return mb.IsVirtual;
        }

        public static bool IsVirtual(this PropertyInfo pi)
        {
            var acc = pi.GetAccessors(true) ?? new MethodInfo[0];
            return acc.Any(m => m.IsVirtual());
        }

        public static bool HasDefaultCtor(this Type t)
        {
            if (t == null) return false;
            return t.GetConstructors(BF.AllInstance).Any(ci => ci.Paramc() == 0);
        }

        public static bool IsJsonPrimitive(this Type t)
        {
            return t == typeof(string) || t == typeof(bool) || t == typeof(float) || t == typeof(double) ||
                t == typeof(sbyte) || t == typeof(short) || t == typeof(int) || t == typeof(long) ||
                t == typeof(byte) || t == typeof(ushort) || t == typeof(uint) || t == typeof(ulong);
        }

        public static bool IsJsonPrimitive(this Object obj)
        {
            if (obj == null) return true;
            if (obj is Type) return (obj as Type).IsJsonPrimitive();
            return obj.GetType().IsJsonPrimitive();
        }

        public static bool IsAbstract(this PropertyInfo pi)
        {
            return pi.GetAccessors(true).Any(mi => mi.IsAbstract);
        }
    }
}