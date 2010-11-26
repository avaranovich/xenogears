using System;
using System.Diagnostics;

namespace XenoGears.Formats.Configuration.Default.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class JsonIncludeAttribute : Attribute
    {
        public String Name { get; set; }

        public JsonIncludeAttribute()
        {
        }

        public JsonIncludeAttribute(String name)
        {
            Name = name;
        }
    }
}
