using System;
using System.Collections.Generic;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Engines.Configuration
{
    public class JsonMetadata
    {
        internal bool Initialized { get; set; }
        public Type Type { get; private set; }
        public JsonMetadata(Type type) { Type = type.AssertNotNull(); }

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