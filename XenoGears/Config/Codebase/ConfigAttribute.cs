using System;
using System.Diagnostics;
using XenoGears.Strings;

namespace XenoGears.Config.Codebase
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [DebuggerNonUserCode]
    public class ConfigAttribute : Attribute
    {
        public String Name { get; set; }
        public String Location { get; set; }

        public ConfigAttribute(String location)
            : this(null, location)
        {
        }

        public ConfigAttribute(String name, String location)
        {
            Name = name ?? this.GetType().GetCSharpRef(ToCSharpOptions.Terse);
            Location = location;
        }
    }
}