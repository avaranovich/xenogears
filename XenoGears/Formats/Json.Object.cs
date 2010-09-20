using System;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats
{
    public class JsonObject : Json
    {
        public JsonObject()
        {
            _my_state = State.Object;
        }

        public JsonObject(Object value) 
            : base(value)
        {
            (_my_state == State.Object).AssertTrue();
        }

        public JsonObject(Object value, Type descriptor)
            : base(value, descriptor)
        {
            (_my_state == State.Object).AssertTrue();
        }

        public JsonObject(Object value, PropertyInfo descriptor)
            : base(value, descriptor)
        {
            (_my_state == State.Object).AssertTrue();
        }

        public JsonObject(Object value, MemberInfo descriptor)
            : base(value, descriptor)
        {
            (_my_state == State.Object).AssertTrue();
        }
    }
}