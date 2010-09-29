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
        }

        public JsonAttribute(JsonShape shape)
        {
            Shape = shape;
        }

        public JsonAttribute(JsonSlots slots)
        {
            Slots = slots;
        }

        public JsonAttribute(JsonShape shape, JsonSlots slots)
        {
            Shape = shape;
            Slots = slots;
        }

        public JsonAttribute(JsonSlots slots, JsonShape shape)
        {
            Shape = shape;
            Slots = slots;
        }
    }
}
