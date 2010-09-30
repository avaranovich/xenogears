using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Reflection;
using XenoGears.Strings;
using XenoGears.Formats.Configuration.Default.Annotations;
using XenoGears.Functional;
using XenoGears.Reflection.Attributes;

namespace XenoGears.Formats.Configuration.Default
{
    [DebuggerNonUserCode]
    public class TypeConfig
    {
        public bool DefaultCtor { get; set; }
        public bool IsPrimitive { get; set; }
        public bool IsObject { get; set; }
        public List<MemberInfo> Slots { get; set; }
        public bool IsList { get; set; }
        public Type ListElement { get; set; }
        public bool IsHash { get; set; }
        public Type HashElement { get; set; }

        public Type Type { get; private set; }
        public TypeConfig(Type type)
        {
            Type = type;

            if (type != null)
            {
                var a_json = type.AttrOrNull<JsonAttribute>() ?? new JsonAttribute();
                DefaultCtor = a_json.DefaultCtor && type.HasDefaultCtor();

                IsPrimitive = type == typeof(string) || type == typeof(bool) || type == typeof(float) || type == typeof(double) ||
                    type == typeof(sbyte) || type == typeof(short) || type == typeof(int) || type == typeof(long) ||
                    type == typeof(byte) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong);
                IsPrimitive |= type.SupportsSerializationToString();
                IsPrimitive &= a_json.Shape.HasFlag(JsonShape.Primitive);

                Slots = type.JsonSlots(JsonSlots.Default).ToList();
                IsObject = Slots.IsNotEmpty();
                IsObject &= a_json.Shape.HasFlag(JsonShape.Object);

                ListElement = type.ListElement();
                IsList = ListElement != null;
                IsList &= a_json.Shape.HasFlag(JsonShape.List);

                HashElement = type.HashElement();
                IsHash = HashElement != null;
                IsHash &= a_json.Shape.HasFlag(JsonShape.Hash);

                if (IsPrimitive) { IsObject = false; IsList = false; IsHash = false; }
                if (IsHash) { IsObject = false; IsList = false; }
                if (IsList) { IsObject = false; }
            }
        }
    }
}