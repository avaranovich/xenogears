using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Reflection;

namespace XenoGears.Formats
{
    [DebuggerNonUserCode]
    public class JsonPrimitive : Json
    {
        public JsonPrimitive(Object value) 
            : base(value.IsJsonPrimitive() ? null : value)
        {
            if (value.IsJsonPrimitive()) _my_primitive = value;
            (_my_state == State.Primitive).AssertTrue();
        }

        public JsonPrimitive(Object value, Type descriptor)
            : base(value, descriptor)
        {
            (_my_state == State.Primitive).AssertTrue();
        }

        public JsonPrimitive(Object value, PropertyInfo descriptor)
            : base(value, descriptor)
        {
            (_my_state == State.Primitive).AssertTrue();
        }

        public JsonPrimitive(Object value, MemberInfo descriptor)
            : base(value, descriptor)
        {
            (_my_state == State.Primitive).AssertTrue();
        }
    }
}