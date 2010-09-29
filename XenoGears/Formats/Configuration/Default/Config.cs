using System;
using System.Collections.Generic;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Configuration.Default
{
    public class Config
    {
        internal bool Initialized { get; set; }
        public Type Type { get; private set; }
        public Config(Type type) { Type = type.AssertNotNull(); }

        public bool DefaultCtor { get; set; }

        public bool IsPrimitive { get; set; }
        public Func<Object, Object> DynamicPrimitive { get; set; }

        public bool IsObject { get; set; }
        public List<MemberInfo> Slots { get; set; }
        public Func<Object, Dictionary<String, Object>> DynamicObject { get; set; }

        public bool IsList { get; set; }
        public Type ListElement { get; set; }
        public Func<Object, IEnumerable<Object>> DynamicList { get; set; }

        public bool IsHash { get; set; }
        public Type HashElement { get; set; }
        public Func<Object, Dictionary<Object, Object>> DynamicHash { get; set; }
    }
}