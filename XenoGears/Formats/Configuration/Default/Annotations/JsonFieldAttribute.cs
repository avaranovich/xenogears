using System;
using System.Diagnostics;

namespace XenoGears.Formats.Configuration.Default.Annotations
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class JsonFieldAttribute : JsonIncludeAttribute
    {
        public JsonFieldAttribute()
        {
        }

        public JsonFieldAttribute(String name)
            : base(name)
        {
        }
    }
}