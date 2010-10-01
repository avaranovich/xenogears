using System;
using System.Collections;
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

    public partial class Json
    {
        private void Object_BuildDebugString(TextWriter writer)
        {
            writer.Write("object");
        }

        private void Object_BuildCompactString(TextWriter writer)
        {
            var keys = Keys.AssertCast<String>();
            writer.Write(String.Format("{{{0}}}", keys.Select((key, i) =>
            {
                return String.Format("\"{0}\":{1}", key, this[key].ToCompactString());
            }).StringJoin(",")));
        }

        private void Object_BuildPrettyString(IndentedWriter writer)
        {
            writer.Write("{");
            writer.Indent++;

            var keys = Keys.AssertCast<String>();
            if (keys.IsNotEmpty())
            {
                keys.ForEach((key, i) =>
                {
                    writer.WriteLine();
                    writer.Write("\"{0}\" : ", key);
                    if (((IEnumerable)this[key].Keys).Cast<Object>().Count() > 0) writer.WriteLine();
                    this[key].BuildPrettyString(writer);
                    if (i < this.Count() - 1) writer.Write(",");
                });

                writer.WriteLine();
            }

            writer.Indent--;
            writer.Write("}");
        }
    }
}