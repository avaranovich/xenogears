using System;
using System.IO;
using System.Linq;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Strings.Writers;
using XenoGears.Strings;

namespace XenoGears.Formats
{
    public partial class Json
    {
        public String ToCompactString()
        {
            var buffer = new StringBuilder();
            BuildCompactString(new StringWriter(buffer));
            return buffer.ToString();
        }

        public String ToPrettyString()
        {
            var buffer = new StringBuilder();
            BuildPrettyString(buffer.Indented());
            return buffer.ToString();
        }

        public sealed override String ToString()
        {
            if (this.IsPrimitive)
            {
                return String.Format("primitive: {0}", _primitive.ToInvariantString());
            }
            else if (this.IsArray)
            {
                return String.Format("array: {0} {1}", Keys.Count(), Keys.Count() == 1 ? "element" : "elements");
            }
            else if (this.IsObject)
            {
                return String.Format("object: {0}", Keys.StringJoin());
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        public void BuildCompactString(TextWriter writer)
        {
            if (this.IsPrimitive)
            {
                if (_primitive == null)
                {
                    writer.Write("null");
                    return;
                }

                var s = _primitive as String;
                if (s != null)
                {
                    writer.Write(s.ToJsonString());
                    return;
                }

                var is_sbyte = _primitive is sbyte;
                if (is_sbyte)
                {
                    var i = (sbyte)_primitive;
                    writer.Write(i.ToInvariantString());
                    return;
                }

                var is_byte = _primitive is byte;
                if (is_byte)
                {
                    var ui = (byte)_primitive;
                    writer.Write(ui.ToInvariantString());
                    return;
                }

                var is_short = _primitive is short;
                if (is_short)
                {
                    var i = (short)_primitive;
                    writer.Write(i.ToInvariantString());
                    return;
                }

                var is_ushort = _primitive is ushort;
                if (is_ushort)
                {
                    var ui = (ushort)_primitive;
                    writer.Write(ui.ToInvariantString());
                    return;
                }

                var is_int = _primitive is int;
                if (is_int)
                {
                    var i = (int)_primitive;
                    writer.Write(i.ToInvariantString());
                    return;
                }

                var is_uint = _primitive is uint;
                if (is_uint)
                {
                    var ui = (uint)_primitive;
                    writer.Write(ui.ToInvariantString());
                    return;
                }

                var is_long = _primitive is long;
                if (is_long)
                {
                    var i = (long)_primitive;
                    writer.Write(i.ToInvariantString());
                    return;
                }

                var is_ulong = _primitive is ulong;
                if (is_ulong)
                {
                    var ui = (ulong)_primitive;
                    writer.Write(ui.ToInvariantString());
                    return;
                }

                var is_float = _primitive is float;
                if (is_float)
                {
                    var f = (float)_primitive;
                    writer.Write(f.ToInvariantString());
                    return;
                }

                var is_double = _primitive is double;
                if (is_double)
                {
                    var d = (double)_primitive;
                    writer.Write(d.ToInvariantString());
                    return;
                }

                var is_bool = _primitive is bool;
                if (is_bool)
                {
                    var b = (bool)_primitive;
                    writer.Write(b.ToInvariantString().ToLower());
                    return;
                }

                throw AssertionHelper.Fail();
            }
            else if (this.IsArray)
            {
                var keys = Keys.AssertCast<int>();
                writer.Write(String.Format("[{0}]", keys.Select((key, i) =>
                {
                    (key == i).AssertTrue();
                    return this[key].ToCompactString();
                }).StringJoin(",")));
            }
            else if (this.IsObject)
            {
                var keys = Keys.AssertCast<String>();
                writer.Write(String.Format("{{{0}}}", keys.Select((key, i) => String.Format("\"{0}\":{1}", key, this[key].ToCompactString())).StringJoin(",")));
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        public void BuildPrettyString(IndentedWriter writer)
        {
            if (this.IsPrimitive)
            {
                writer.Write(this.ToCompactString());
            }
            else if (this.IsArray)
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
            else if (this.IsObject)
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
                        if (this[key].IsComplex) writer.WriteLine();
                        this[key].BuildPrettyString(writer);
                        if (i < this.Count() - 1) writer.Write(",");
                    });

                    writer.WriteLine();
                }

                writer.Indent--;
                writer.Write("}");
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }
    }
}
