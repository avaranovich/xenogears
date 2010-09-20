using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
            return ToCompactString();
        }

        public void BuildCompactString(TextWriter writer)
        {
            if (this.IsPrimitive)
            {
                if (_primitive == null) writer.Write("null");

                var s = _primitive as String;
                if (s != null) writer.Write(s.ToJsonString());

                var is_int = _primitive is int;
                if (is_int) writer.Write(_primitive.ToInvariantString());

                var is_double = _primitive is double;
                if (is_double) writer.Write(_primitive.ToInvariantString());

                var is_bool = _primitive is bool;
                if (is_bool) writer.Write(is_bool.ToInvariantString().ToLower());

                // todo. maybe add custom converters, e.g. for datetime
                // todo. maybe even return clientcontext-bound code, but for what?
                throw AssertionHelper.Fail();
            }
            else if (this.IsArray)
            {
                var keys = ((IDynamicMetaObjectProvider)this).GetMetaObject(Expression.Constant(this)).GetDynamicMemberNames();
                writer.Write(String.Format("[{0}]", keys.Select((key, i) =>
                {
                    (key == i.ToString()).AssertTrue();
                    return this[key].ToCompactString();
                }).StringJoin(",")));
            }
            else if (this.IsObject)
            {
                var keys = ((IDynamicMetaObjectProvider)this).GetMetaObject(Expression.Constant(this)).GetDynamicMemberNames();
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

                var keys = ((IDynamicMetaObjectProvider)this).GetMetaObject(Expression.Constant(this)).GetDynamicMemberNames();
                if (keys.IsNotEmpty())
                {
                    keys.ForEach((key, i) =>
                    {
                        (key == i.ToString()).AssertTrue();
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

                var keys = ((IDynamicMetaObjectProvider)this).GetMetaObject(Expression.Constant(this)).GetDynamicMemberNames();
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
