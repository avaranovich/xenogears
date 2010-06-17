using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Reflection
{
    [DebuggerNonUserCode]
    public static class VisibilityHelper
    {
        public static Visibility Visibility(this MemberInfo mi)
        {
            mi.AssertNotNull();
            if (mi is Type)
            {
                var t = mi.AssertCast<Type>();
                if (t.IsNested)
                {
                    if (t.IsNestedPublic) return Reflection.Visibility.Public;
                    if (t.IsNestedFamORAssem) return Reflection.Visibility.FamilyOrAssembly;
                    if (t.IsNestedFamily) return Reflection.Visibility.Family;
                    if (t.IsNestedAssembly) return Reflection.Visibility.Assembly;
                    if (t.IsNestedFamANDAssem) return Reflection.Visibility.FamilyAndAssembly;
                    if (t.IsNestedPrivate) return Reflection.Visibility.Private;
                    throw AssertionHelper.Fail();
                }
                else
                {
                    if (t.IsPublic) return Reflection.Visibility.Public;
                    return Reflection.Visibility.Assembly;
                }
            }
            else if (mi is FieldInfo)
            {
                var fi = mi.AssertCast<FieldInfo>();
                if (fi.IsPublic) return Reflection.Visibility.Public;
                if (fi.IsFamilyOrAssembly) return Reflection.Visibility.FamilyOrAssembly;
                if (fi.IsFamily) return Reflection.Visibility.Family;
                if (fi.IsAssembly) return Reflection.Visibility.Assembly;
                if (fi.IsFamilyAndAssembly) return Reflection.Visibility.FamilyAndAssembly;
                if (fi.IsPrivate) return Reflection.Visibility.Private;
                throw AssertionHelper.Fail();
            }
            else if (mi is MethodBase)
            {
                var mb = mi.AssertCast<MethodBase>();
                if (mb.IsPublic) return Reflection.Visibility.Public;
                if (mb.IsFamilyOrAssembly) return Reflection.Visibility.FamilyOrAssembly;
                if (mb.IsFamily) return Reflection.Visibility.Family;
                if (mb.IsAssembly) return Reflection.Visibility.Assembly;
                if (mb.IsFamilyAndAssembly) return Reflection.Visibility.FamilyAndAssembly;
                if (mb.IsPrivate) return Reflection.Visibility.Private;
                throw AssertionHelper.Fail();
            }
            else if (mi is PropertyInfo)
            {
                var pi = mi.AssertCast<PropertyInfo>();
                var getter = pi.CanRead ? null : pi.GetGetMethod(true);
                var setter = pi.CanRead ? null : pi.GetGetMethod(true);

                if (getter == null || setter == null)
                {
                    throw AssertionHelper.Fail();
                }
                else if (getter == null || setter == null)
                {
                    return (getter ?? setter).Visibility();
                }
                else
                {
                    var g_vis = getter.Visibility();
                    var s_vis = setter.Visibility();
                    return (Visibility)Math.Max((int)g_vis, (int)s_vis);
                }
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }
    }
}