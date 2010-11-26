using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Functional;

namespace XenoGears.Ogre.Reflection
{
    public static class Tools
    {
        public static bool IsDotfuscated(this String fname)
        {
            return fname == null || fname.Any(c => c > 255);
        }

        public static IEnumerable<Type> GetExplorableContracts(this Type type)
        {
            var hierarchy = type.Unfold(t => t.BaseType, t => t != null);
            foreach(var htype in hierarchy)
            {
                if (!htype.Name.IsDotfuscated())
                {
                    yield return htype;
                }

                var myIfaces = htype.GetInterfaces();
                var baseIfaces = htype.BaseType == null ? Enumerable.Empty<Type>() : htype.BaseType.GetInterfaces();
                foreach(var iface in myIfaces.Except(baseIfaces))
                {
                    yield return iface;
                }
            }
        }

        public static String NormalizeFieldName(this String fname)
        {
            // autoprop: <Name>k__BackingField
            return fname.Contains(">") ? fname.Substring(1, fname.IndexOf(">") - 1) : fname;
        }

        public static IEnumerable<FieldInfo> GetExplorableFields(this Type type, BindingFlags flags)
        {
            // todo. smart but not 100% correct approach
            var dict = new Dictionary<String, FieldInfo>();
            var hfields = type.GetExplorableContracts().Reverse().SelectMany(
                t => t.GetFields(flags)).Where(f => !f.Name.IsDotfuscated());
            hfields.ForEach(f => dict[f.Name.NormalizeFieldName()] = f);
            return dict.Values.OrderBy(f => f.Name);
        }

        public static IEnumerable<PropertyInfo> GetExplorableProperties(this Type type, BindingFlags flags)
        {
            // todo. smart but not 100% correct approach
            var dict = new Dictionary<String, PropertyInfo>();
            var hprops = type.GetExplorableContracts().Reverse().SelectMany(
                t => t.GetProperties(flags)).Where(p => !p.Name.IsDotfuscated()) // don't care about obfuscated stuff
                .Where(p => p.CanRead && p.GetGetMethod() != null && p.GetGetMethod().GetParameters().IsEmpty());
            hprops.ForEach(f => dict[f.Name] = f);
            return dict.Values.OrderBy(p => p.Name);
        }

        public static String GetExplorableTypeInfo(this Object obj)
        {
            var hierarchy = obj.GetType().Unfold(t => t.BaseType, t => t != null);
            var explorable = hierarchy.Where(t => !t.Name.IsDotfuscated());
            var obfuscated = hierarchy.Where(t => t.Name.IsDotfuscated());

            var top = explorable.First();
            var ifaces = obfuscated.SelectMany(t => t.GetInterfaces()).Distinct().Except(top.GetInterfaces());
            var nonRedundant = top.MkArray().Concat(ifaces.OrderBy(t => t.Name));

            return nonRedundant.Select(t => t.AssemblyQualifiedName).StringJoin("," + Environment.NewLine);
        }
    }
}
