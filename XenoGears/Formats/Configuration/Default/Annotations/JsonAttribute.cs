using System;
using System.Diagnostics;

namespace XenoGears.Formats.Configuration.Default.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class JsonAttribute : Attribute
    {
        public JsonShape Shape { get; set; }
        public JsonSlots Slots { get; set; }
        public bool DefaultCtor { get; set; }

        public JsonAttribute()
        {
            Shape = JsonShape.Primitive | JsonShape.Object | JsonShape.List | JsonShape.Hash;
            Slots = JsonSlots.Default;
            DefaultCtor = true;
        }

        public JsonAttribute(JsonShape shape)
            : this()
        {
            Shape = shape;
        }

        public JsonAttribute(JsonSlots slots)
            : this()
        {
            Slots = slots;
        }

        public JsonAttribute(JsonShape shape, JsonSlots slots)
            : this()
        {
            Shape = shape;
            Slots = slots;
        }

        public JsonAttribute(JsonSlots slots, JsonShape shape)
            : this()
        {
            Shape = shape;
            Slots = slots;
        }
    }
}
