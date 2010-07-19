using System.Diagnostics;
using System.IO;

namespace XenoGears.Strings
{
    // use this to work around the following bug in BCL
    // http://blogs.msdn.com/kaelr/archive/2006/03/28/indentedtextwriter.aspx
    [DebuggerNonUserCode]
    public static class IndentedTextWriterFactory
    {
        public static IndentedTextWriter Indented(this TextWriter writer)
        {
            var indented = writer as IndentedTextWriter;
            return indented ?? new IndentedTextWriter(writer);
        }
    }
}