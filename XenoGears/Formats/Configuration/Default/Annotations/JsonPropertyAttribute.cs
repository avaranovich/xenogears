using System;
using System.Diagnostics;

namespace XenoGears.Formats.Configuration.Default.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class JsonPropertyAttribute : JsonIncludeAttribute
    {
        public JsonPropertyAttribute()
        {
        }

        public JsonPropertyAttribute(String name) 
            : base(name)
        {
        }
    }
}