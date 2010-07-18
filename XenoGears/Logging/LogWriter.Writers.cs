using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using XenoGears.Logging.Writers;
using XenoGears.Traits.Disposable;

namespace XenoGears.Logging
{
    // note. for now I don't care about log4net and similar stuff

    [DebuggerNonUserCode]
    public static partial class LogWriter
    {
        private static TextWriter _medium = new LowlevelMedium();
        private static TextWriter Medium
        {
            get { return _medium; }
            set { _medium = value ?? new StringWriter(new StringBuilder()); }
        }

        public static IDisposable Override(StringBuilder new_out) { return Override(new StringWriter(new_out)); }
        public static IDisposable Override(TextWriter new_out)
        {
            var old_out = Medium;
            Medium = new_out;
            return new DisposableAction(() => Medium = old_out);
        }

        public static IDisposable Multiplex(StringBuilder sink) { return Multiplex(new StringWriter(sink)); }
        public static IDisposable Multiplex(TextWriter sink)
        {
            var eavesdropper = new Eavesdropper(Medium, sink);
            return Override(eavesdropper);
        }

        internal static void Write(Logger logger, Level level, Object o)
        {
            if (IsMuted(logger, level)) return;
            Medium.Write(o);
        }
    }
}
