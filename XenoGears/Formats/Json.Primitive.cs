using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Reflection;
using XenoGears.Strings;
using XenoGears.Strings.Writers;

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

    public partial class Json
    {
        private void Primitive_BuildDebugString(TextWriter writer)
        {
            Primitive_BuildCompactString(writer);
        }

        private void Primitive_BuildCompactString(TextWriter writer)
        {
            if (Value == null)
            {
                writer.Write("null");
                return;
            }

            var s = Value as String;
            if (s != null)
            {
                writer.Write(s.ToJsonString());
                return;
            }

            var is_sbyte = Value is sbyte;
            if (is_sbyte)
            {
                var i = (sbyte)Value;
                writer.Write(i.ToInvariantString());
                return;
            }

            var is_byte = Value is byte;
            if (is_byte)
            {
                var ui = (byte)Value;
                writer.Write(ui.ToInvariantString());
                return;
            }

            var is_short = Value is short;
            if (is_short)
            {
                var i = (short)Value;
                writer.Write(i.ToInvariantString());
                return;
            }

            var is_ushort = Value is ushort;
            if (is_ushort)
            {
                var ui = (ushort)Value;
                writer.Write(ui.ToInvariantString());
                return;
            }

            var is_int = Value is int;
            if (is_int)
            {
                var i = (int)Value;
                writer.Write(i.ToInvariantString());
                return;
            }

            var is_uint = Value is uint;
            if (is_uint)
            {
                var ui = (uint)Value;
                writer.Write(ui.ToInvariantString());
                return;
            }

            var is_long = Value is long;
            if (is_long)
            {
                var i = (long)Value;
                writer.Write(i.ToInvariantString());
                return;
            }

            var is_ulong = Value is ulong;
            if (is_ulong)
            {
                var ui = (ulong)Value;
                writer.Write(ui.ToInvariantString());
                return;
            }

            var is_float = Value is float;
            if (is_float)
            {
                var f = (float)Value;
                writer.Write(f.ToInvariantString());
                return;
            }

            var is_double = Value is double;
            if (is_double)
            {
                var d = (double)Value;
                writer.Write(d.ToInvariantString());
                return;
            }

            var is_bool = Value is bool;
            if (is_bool)
            {
                var b = (bool)Value;
                writer.Write(b.ToInvariantString().ToLower());
                return;
            }

            throw AssertionHelper.Fail();
        }

        private void Primitive_BuildPrettyString(IndentedWriter writer)
        {
            Primitive_BuildCompactString(writer);
        }
    }
}