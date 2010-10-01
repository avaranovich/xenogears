using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Reflection.Emit;

namespace XenoGears.Reflection
{
    [DebuggerNonUserCode]
    public static class ReflectionHelper
    {
        public static Type GetFieldOrPropertyType(this Type t, String name)
        {
            return t.GetFieldOrProperty(name).GetFieldOrPropertyType();
        }

        public static Type GetFieldOrPropertyType(this MemberInfo mi)
        {
            if (mi == null)
            {
                return null;
            }
            else
            {
                return mi is FieldInfo ? ((FieldInfo)mi).FieldType : ((PropertyInfo)mi).PropertyType;
            }
        }

        public static MemberInfo GetFieldOrProperty(this Type t, String name)
        {
            if (t == null)
            {
                return null;
            }

            // only public for now
            var fi = t.GetField(name, BF.PublicInstance);
            if (fi != null)
            {
                return fi;
            }

            var pi = t.GetProperty(name, BF.PublicInstance);
            if (pi != null)
            {
                return pi;
            }

            return null;
        }

        // Has a multitude of uses, but primary of those is  finding out 
        // whether some type is a closed generic for some other type
        // e.g. SameMetadataToken(List<int>, List<>) will return true
        // for comparison, typeof(List<int>) == typeof(List<>) will return false
        public static bool SameMetadataToken(this MemberInfo t1, MemberInfo t2)
        {
            return t1.Module.Assembly == t2.Module.Assembly &&
                t1.Module == t2.Module &&
                    t1.MetadataToken == t2.MetadataToken;
        }

        public static bool SameType(this Type t1, Type t2)
        {
            if (!t1.SameBasisType(t2))
            {
                return false;
            }
            else
            {
                var t1args = t1.XGetGenericArguments();
                var t2args = t2.XGetGenericArguments();

                if (t1args.Length != t2args.Length)
                {
                    throw new NotSupportedException(String.Format("Something was overlooked: " +
                        "The type '{0}' and '{1}' claimed that they share basis type", t1, t2));
                }
                else
                {
                    return t1args.AllMatch(t2args,
                        (t1argsi, t2argsi) => t1argsi.SameType(t2argsi));
                }
            }
        }

        public static Type GetBasisType(this Type t)
        {
            return t.XGetGenericDefinition();
        }

        public static bool SameBasisType(this Type t1, Type t2)
        {
            if (t1 == null || t2 == null)
            {
                return t1 == t2;
            }
            else
            {
                return t1.SameMetadataToken(t2);
            }
        }

        public static Type[] GetBaseTypes(this Type t)
        {
            if (t.IsInterface)
            {
                return t.GetInterfaces().Concat(typeof(object)).ToArray();
            }
            else
            {
                var baseTypes = new List<Type>();
                for (var current = t.BaseType; current != null; current = current.BaseType)
                    baseTypes.Add(current);
                return baseTypes.ToArray();
            }
        }

        public static IEnumerable<Type> Hierarchy(this Type t)
        {
            if (t == null)
            {
                yield break;
            }
            else
            {
                for (var current = t; current != null; current = current.BaseType)
                    yield return current;

                foreach (var baseIface in t.GetInterfaces())
                    yield return baseIface;
            }
        }

        public static IEnumerable<MethodBase> Hierarchy(this MethodBase mb)
        {
            if (mb == null)
            {
                return Enumerable.Empty<MethodBase>();
            }
            else
            {
                if (mb is MethodInfo)
                {
                    return ((MethodInfo)mb).Hierarchy().Cast<MethodBase>();
                }
                else if (mb is ConstructorInfo)
                {
                    return ((ConstructorInfo)mb).Hierarchy().Cast<MethodBase>();
                }
                else
                {
                    throw AssertionHelper.Fail();
                }
            }
        }

        public static IEnumerable<MethodInfo> Hierarchy(this MethodInfo mi)
        {
            if (mi == null)
            {
                yield break;
            }
            else
            {
                for (var current = mi; current != null; current = 
                    current != current.GetBaseDefinition() ? current.GetBaseDefinition() : null)
                {
                    yield return current;

                    foreach (var declaration in current.Declarations()) 
                        yield return (MethodInfo)declaration;
                }
            }
        }

        public static IEnumerable<PropertyInfo> Hierarchy(this PropertyInfo pi)
        {
            var acc = pi == null ? null : 
                pi.CanRead ? pi.GetGetMethod(true) :
                pi.CanWrite ? pi.GetSetMethod(true) : 
                ((Func<MethodInfo>)(() => { throw AssertionHelper.Fail();}))();
            return acc.Hierarchy().Select(m => m.EnclosingProperty().AssertNotNull());
        }

        public static IEnumerable<ConstructorInfo> Hierarchy(this ConstructorInfo ci)
        {
            // todo. implement with IL analysis
            throw new NotImplementedException();
        }

        public static PropertyInfo EnclosingProperty(this MethodBase source)
        {
            if (source == null) return null;
            if (source.DeclaringType == null) return null;

            var props = source.DeclaringType.GetProperties(BF.All);
            return props.SingleOrDefault(prop => prop.GetAccessors(true).Any(mi => mi.MethodHandle == source.MethodHandle));
        }

        public static bool Overrides(this MemberInfo wb_child, MemberInfo wb_parent)
        {
            if (wb_child == null || wb_parent == null) return false;

            if (wb_child is MethodInfo || wb_parent is MethodInfo)
            {
                var mi_child = wb_child as MethodInfo;
                var mi_parent = wb_parent as MethodInfo;
                if (mi_child == null || mi_parent == null) return false;
                return mi_child.Hierarchy().Contains(mi_parent);
            }
            else if (wb_child is PropertyInfo || wb_parent is PropertyInfo)
            {
                var pi_child = wb_child as PropertyInfo;
                var pi_parent = wb_parent as PropertyInfo;
                if (pi_child == null || pi_parent == null) return false;
                return pi_child.Hierarchy().Contains(pi_parent);
            }
            else
            {
                return false;
            }
        }

        public static bool CanRead(this MemberInfo mi)
        {
            if (mi is FieldInfo)
            {
                return true;
            }
            else if (mi is PropertyInfo)
            {
                var pi = mi.AssertCast<PropertyInfo>();
                return pi.CanRead;
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        public static Object GetValue(this MemberInfo mi, Object target)
        {
            if (mi is FieldInfo)
            {
                var fi = mi.AssertCast<FieldInfo>();
                return fi.GetValue(target);
            }
            else if (mi is PropertyInfo)
            {
                var pi = mi.AssertCast<PropertyInfo>();
                return pi.GetValue(target, null);
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        public static bool CanWrite(this MemberInfo mi)
        {
            if (mi is FieldInfo)
            {
                return true;
            }
            else if (mi is PropertyInfo)
            {
                var pi = mi.AssertCast<PropertyInfo>();
                return pi.CanWrite;
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        public static void SetValue(this MemberInfo mi, Object target, Object value)
        {
            if (mi is FieldInfo)
            {
                var fi = mi.AssertCast<FieldInfo>();
                fi.SetValue(target, value);
            }
            else if (mi is PropertyInfo)
            {
                var pi = mi.AssertCast<PropertyInfo>();
                pi.SetValue(target, value, null);
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        public static Type Type(this MemberInfo mi)
        {
            if (mi is FieldInfo)
            {
                var fi = mi.AssertCast<FieldInfo>();
                return fi.FieldType;
            }
            else if (mi is PropertyInfo)
            {
                var pi = mi.AssertCast<PropertyInfo>();
                return pi.PropertyType;
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }
    }
}