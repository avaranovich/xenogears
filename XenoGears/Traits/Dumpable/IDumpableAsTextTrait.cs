using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using XenoGears.Reflection.Attributes;

namespace XenoGears.Traits.Dumpable
{
    [DebuggerNonUserCode]
    public static class IDumpableAsTextTrait
    {
        public static String DumpAsText<T>(this T dumpable)
            where T : IDumpableAsText
        {
            var buffer = new StringBuilder();
            DumpAsText(buffer, dumpable);
            return buffer.ToString();
        }

        public static StringBuilder DumpAsText<T>(this T dumpable, StringBuilder buffer)
            where T : IDumpableAsText
        {
            return DumpAsText(buffer, dumpable);
        }

        public static StringBuilder DumpAsText<T>(this StringBuilder buffer, T dumpable)
            where T : IDumpableAsText
        {
            DumpAsText(dumpable, new StringWriter(buffer));
            return buffer;
        }

        public static TextWriter DumpAsText<T>(this T dumpable, TextWriter buffer)
            where T : IDumpableAsText
        {
            return DumpAsText(buffer, dumpable);
        }

        public static TextWriter DumpAsText<T>(this TextWriter writer, T dumpable)
            where T : IDumpableAsText
        {
            if (dumpable == null)
            {
                var formatter = typeof(T).Attr<DumpFormatAttribute>(true);
                writer.Write(formatter.NullObjectFormat);
            }
            else
            {
                dumpable.DumpAsText(writer);
            }

            return writer;
        }
    }
}