using System;
using System.Diagnostics;
using XenoGears.Reflection;

namespace XenoGears.Formats.Configuration.Default.Annotations
{
    [DebuggerNonUserCode]
    public static class JsonShapeHelper
    {
        public static Type ListElement(this Type t)
        {
            return t.GetListElement();
        }

        public static Type HashElement(this Type t)
        {
            var key = t.GetDictionaryKey();
            var value = t.GetDictionaryValue();
            return key == typeof(String) ? value : null;
        }
    }
}