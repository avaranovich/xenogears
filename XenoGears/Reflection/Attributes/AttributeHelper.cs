using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Reflection.Attributes
{
    [DebuggerNonUserCode]
    public static class AttributeHelper
    {
        public static bool HasAttr<T>(this ICustomAttributeProvider cap)
            where T : Attribute
        {
            return cap.HasAttr(typeof(T));
        }

        public static bool HasAttr(this ICustomAttributeProvider cap, Type t)
        {
            return cap.HasAttr(t, true);
        }

        public static bool HasAttr<T>(this ICustomAttributeProvider cap, bool inherit)
            where T : Attribute
        {
            return cap.HasAttr(typeof(T), inherit);
        }

        public static bool HasAttr(this ICustomAttributeProvider cap, Type t, bool inherit)
        {
            // a crude workaround
            if (cap is PropertyInfo && inherit)
            {
                var pi = (PropertyInfo)cap;
                var hierarchy = pi.DeclaringType.Hierarchy();
                return hierarchy.Any(t1 =>
                {
                    var basep = t1.GetProperties(BF.All).SingleOrDefault(p => p.Name == pi.Name && p.DeclaringType == t1);
                    return basep == null ? false : basep.IsDefined(t, true);
                });
            }
            else
            {
                return cap.IsDefined(t, inherit);
            }
        }

        public static IEnumerable<T> Attrs<T>(this ICustomAttributeProvider cap)
            where T : Attribute
        {
            return cap.Attrs(typeof(T)).Cast<T>();
        }

        public static IEnumerable<Attribute> Attrs(this ICustomAttributeProvider cap)
        {
            return cap.Attrs(typeof(Attribute));
        }

        public static IEnumerable<Attribute> Attrs(this ICustomAttributeProvider cap, Type t)
        {
            return cap.Attrs(t, true);
        }

        public static IEnumerable<T> Attrs<T>(this ICustomAttributeProvider cap, bool inherit)
            where T : Attribute
        {
            return cap.Attrs(typeof(T), inherit).Cast<T>();
        }

        public static IEnumerable<Attribute> Attrs(this ICustomAttributeProvider cap, Type t, bool inherit)
        {
            // a crude workaround
            if (cap is PropertyInfo && inherit)
            {
                var pi = (PropertyInfo)cap;
                var hierarchy = pi.DeclaringType.Hierarchy();
                return hierarchy.SelectMany(t1 =>
                {
                    var basep = t1.GetProperties(BF.All).SingleOrDefault(p => p.Name == pi.Name && p.DeclaringType == t1);
                    return basep == null ? Enumerable.Empty<Attribute>() : basep.GetCustomAttributes(t, true).Cast<Attribute>();
                })
                // this is necessary to shield us from a codegen bug
                // typeof(Gen'dNode).GetProperties("Name", any BF) returns 2x props: CompiledNode.Name and Gen'dNode.Name
                .TakeWhile((a, i) => i < 1);
            }
            else
            {
                return cap.GetCustomAttributes(t, inherit).Cast<Attribute>();
            }
        }

        public static T Attr<T>(this ICustomAttributeProvider cap)
            where T : Attribute
        {
            return (T)cap.Attr(typeof(T));
        }

        public static Attribute Attr(this ICustomAttributeProvider cap, Type t)
        {
            return cap.Attr(t, true);
        }

        public static Attribute Attr(this ICustomAttributeProvider cap)
        {
            return cap.Attr(typeof(Attribute));
        }

        public static T Attr<T>(this ICustomAttributeProvider cap, bool inherit)
            where T : Attribute
        {
            return (T)cap.Attr(typeof(T), inherit);
        }

        public static Attribute Attr(this ICustomAttributeProvider cap, Type t, bool inherit)
        {
            return cap.Attrs(t, inherit).Single();
        }

        public static T AttrOrNull<T>(this ICustomAttributeProvider cap)
            where T : Attribute
        {
            return (T)cap.AttrOrNull(typeof(T));
        }

        public static Attribute AttrOrNull(this ICustomAttributeProvider cap, Type t)
        {
            return cap.AttrOrNull(t, true);
        }

        public static Attribute AttrOrNull(this ICustomAttributeProvider cap)
        {
            return cap.Attr(typeof(Attribute));
        }

        public static T AttrOrNull<T>(this ICustomAttributeProvider cap, bool inherit)
            where T : Attribute
        {
            return (T)cap.AttrOrNull(typeof(T), inherit);
        }

        public static Attribute AttrOrNull(this ICustomAttributeProvider cap, Type t, bool inherit)
        {
            return cap.Attrs(t, inherit).SingleOrDefault();
        }
    }
}