using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Reflection.Attributes;

namespace XenoGears.Formats.Configuration.Default.Annotations
{
    [DebuggerNonUserCode]
    public static class JsonIncludeHelper
    {
        public static String JsonName(this MemberInfo mi)
        {
            if (mi == null) return null;
            var a_ji = mi.AttrOrNull<JsonIncludeAttribute>() ?? new JsonIncludeAttribute(mi.Name);
            return a_ji.Name;
        }
    }
}
