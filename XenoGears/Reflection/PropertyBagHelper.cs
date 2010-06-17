using System;
using System.Diagnostics;
using System.Linq;
using XenoGears.Collections;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Reflection
{
    [DebuggerNonUserCode]
    public static class PropertyBagHelper
    {
        public static ReadOnlyDictionary<String, Object> ToPropertyBag(this Object obj)
        {
            var fields = obj.GetType().GetFields(BF.PublicInstance)
                .ToDictionary(f => f.Name, f => f.GetValue(obj));
            var props = obj.GetType().GetProperties(BF.PublicInstance)
                .Where(p => p.CanRead && p.GetGetMethod(true).GetParameters().IsEmpty())
                .ToDictionary(p => p.Name, f => f.GetValue(obj, null));
            return fields.Concat(props).ToDictionary().ToReadOnly();
        }
    }
}
