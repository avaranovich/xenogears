using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Formats.Configuration.Default.Annotations
{
    [DebuggerNonUserCode]
    internal static class JsonSlotsHelper
    {
        public static ReadOnlyCollection<MemberInfo> JsonSlots(this Type t, JsonSlots slots)
        {
            if (t == null) return null;
            throw new NotImplementedException();
        }
    }
}