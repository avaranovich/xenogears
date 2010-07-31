using System.Diagnostics;
using System.Text;
using XenoGears.Assertions;

namespace XenoGears.Strings.Writers
{
    [DebuggerNonUserCode]
    public static class DelayedWriterFactory
    {
        public static DelayedWriter Delayed(this StringBuilder buf)
        {
            buf.AssertNotNull();
            return new DelayedWriter(buf);
        }
    }
}