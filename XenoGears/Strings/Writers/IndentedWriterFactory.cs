using System.Diagnostics;
using System.IO;

namespace XenoGears.Strings.Writers
{
    [DebuggerNonUserCode]
    public static class IndentedWriterFactory
    {
        public static IndentedWriter Indented(this TextWriter writer)
        {
            var indented = writer as IndentedWriter;
            return indented ?? new IndentedWriter(writer);
        }
    }
}