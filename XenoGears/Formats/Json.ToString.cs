using System;
using System.IO;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Strings.Writers;

namespace XenoGears.Formats
{
    public partial class Json
    {
        public sealed override String ToString()
        {
            return ToCompactString();
        }

        public String ToDebugString()
        {
            var buffer = new StringBuilder();
            BuildDebugString(new StringWriter(buffer));
            return buffer.ToString();
        }

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

        private void BuildDebugString(TextWriter writer)
        {
            if (IsPrimitive)
            {
                Primitive_BuildDebugString(writer);
            }
            else if (IsObject)
            {
                Object_BuildDebugString(writer);
            }
            else if (IsArray)
            {
                Array_BuildDebugString(writer);
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        private void BuildCompactString(TextWriter writer)
        {
            if (IsPrimitive)
            {
                Primitive_BuildCompactString(writer);
            }
            else if (IsObject)
            {
                Object_BuildCompactString(writer);
            }
            else if (IsArray)
            {
                Array_BuildCompactString(writer);
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        private void BuildPrettyString(IndentedWriter writer)
        {
            if (IsPrimitive)
            {
                Primitive_BuildPrettyString(writer);
            }
            else if (IsObject)
            {
                Object_BuildPrettyString(writer);
            }
            else if (IsArray)
            {
                Array_BuildPrettyString(writer);
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }
    }
}
