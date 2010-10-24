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
            // note. I'm commenting this line because we might want to support serialization-only scenarios
//            return t.GetListElement();
            return t.GetEnumerableElement();
        }

        public static Type HashElement(this Type t)
        {
            var key = t.GetDictionaryKey();
            var value = t.GetDictionaryValue();
            return key == typeof(String) ? value : null;
        }
    }
}