using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Formats.Configuration.Default
{
    [DebuggerNonUserCode]
    public class Config
    {
        public Type Type { get; private set; }
        public Config(Type type) { Type = type; }

        public bool DefaultCtor { get; set; }

        public bool IsPrimitive { get; set; }

        public bool IsObject { get; set; }
        public List<MemberInfo> Slots { get; set; }

        public bool IsList { get; set; }
        public Type ListElement { get; set; }

        public bool IsHash { get; set; }
        public Type HashElement { get; set; }
    }
}