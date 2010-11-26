using System.Diagnostics;
using System.IO;
using System.Text;
using XenoGears.Assertions;

namespace XenoGears.Strings.Writers
{
    [DebuggerNonUserCode]
    public static class IndentedWriterFactory
    {
        public static IndentedWriter Indented(this StringBuilder buf)
        {
            buf.AssertNotNull();
            return new IndentedWriter(buf);
        }

        public static IndentedWriter Indented(this TextWriter writer)
        {
            writer.AssertNotNull();
            var indented = writer as IndentedWriter;
            return indented ?? new IndentedWriter(writer);
        }
    }
}