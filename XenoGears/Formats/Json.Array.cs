using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Strings.Writers;

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

    public partial class Json
    {
        private void Array_BuildDebugString(TextWriter writer)
        {
            writer.Write("array");
        }

        private void Array_BuildCompactString(TextWriter writer)
        {
            var keys = Keys.AssertCast<int>();
            writer.Write(String.Format("[{0}]", keys.Select((key, i) =>
            {
                (key == i).AssertTrue();
                return this[key].ToCompactString();
            }).StringJoin(",")));
        }

        private void Array_BuildPrettyString(IndentedWriter writer)
        {
            writer.Write("[");
            writer.Indent++;

            var keys = Keys.AssertCast<int>();
            if (keys.IsNotEmpty())
            {
                keys.ForEach((key, i) =>
                {
                    (key == i).AssertTrue();
                    if (this[key].IsComplex) writer.WriteLine();
                    this[key].BuildPrettyString(writer);
                    if (i < this.Count() - 1) writer.Write(",");
                });

                writer.WriteLine();
            }

            writer.Indent--;
            writer.Write("]");
        }
    }
}