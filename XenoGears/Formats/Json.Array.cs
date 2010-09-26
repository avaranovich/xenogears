using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats
{
    [DebuggerNonUserCode]
    public class JsonArray : Json
    {
        public JsonArray()
        {
            _my_state = State.Array;
        }

        public JsonArray(Object value) 
            : base(value)
        {
            (_my_state == State.Array).AssertTrue();
        }

        public JsonArray(Object value, Type descriptor)
            : base(value, descriptor)
        {
            (_my_state == State.Array).AssertTrue();
        }

        public JsonArray(Object value, PropertyInfo descriptor)
            : base(value, descriptor)
        {
            (_my_state == State.Array).AssertTrue();
        }

        public JsonArray(Object value, MemberInfo descriptor)
            : base(value, descriptor)
        {
            (_my_state == State.Array).AssertTrue();
        }
    }
}