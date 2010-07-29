using System.Diagnostics;
using System.IO;

namespace XenoGears.Strings.Writers
{
    [DebuggerNonUserCode]
    public static class DelayedWriterFactory
    {
        public static DelayedWriter Delayed(this TextWriter writer)
        {
            var delayed = writer as DelayedWriter;
            return delayed ?? new DelayedWriter(writer);
        }
    }
}