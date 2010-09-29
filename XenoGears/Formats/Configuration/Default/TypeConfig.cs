using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Reflection;
using XenoGears.Strings;
using XenoGears.Formats.Configuration.Default.Annotations;
using XenoGears.Functional;

namespace XenoGears.Formats.Configuration.Default
{
    [DebuggerNonUserCode]
    public class TypeConfig
    {
        public Type Type { get; private set; }
        public TypeConfig(Type type)
        {
            Type = type;

            if (type != null)
            {
                DefaultCtor = type.HasDefaultCtor();

                IsPrimitive = type == typeof(string) || type == typeof(bool) || type == typeof(float) || type == typeof(double) ||
                    type == typeof(sbyte) || type == typeof(short) || type == typeof(int) || type == typeof(long) ||
                    type == typeof(byte) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong);
                IsPrimitive |= type.SupportsSerializationToString();

                Slots = type.JsonSlots(JsonSlots.Default).ToList();
                IsObject = Slots.IsNotEmpty();

                ListElement = type.ListElement();
                IsList = ListElement != null;

                HashElement = type.HashElement();
                IsHash = HashElement != null;
            }
        }

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