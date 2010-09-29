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
            return t.GetEnumerableElement();
        }

        public static Type HashElement(this Type t)
        {
            return t.GetDictionaryValue();
        }
    }
}