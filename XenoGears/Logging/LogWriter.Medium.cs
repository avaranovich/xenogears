using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using XenoGears.Logging.Media;
using XenoGears.Traits.Disposable;
using XenoGears.Logging.Formatters;

namespace XenoGears.Logging
{
    // note. for now I don't care about log4net and similar stuff
    // those are backends, and as of now I need to flesh out the frontend

    [DebuggerNonUserCode]
    public partial class LogWriter
    {
        public TextWriter Medium { get; set; }

        public IDisposable Override(StringBuilder new_out) { return Override(new StringWriter(new_out)); }
        public IDisposable Override(TextWriter new_out)
        {
            var old_out = Medium;
            Medium = new_out ?? new StringWriter(new StringBuilder());
            return new DisposableAction(() => Medium = old_out);
        }

        public IDisposable Multiplex(StringBuilder sink) { return Multiplex(new StringWriter(sink)); }
        public IDisposable Multiplex(TextWriter sink)
        {
            var eavesdropper = new Eavesdropper(Medium, sink);
            return Override(eavesdropper);
        }

        public LogWriter Write(Logger logger, Level level, Object o)
        {
            if (!IsMuted(logger, level)) Medium.Write(o.ToLog());
            return this;
        }

        public LogWriter Write(Logger logger, Level level, String message)
        {
            if (!IsMuted(logger, level)) Medium.Write(message.ToLog());
            return this;
        }

        public LogWriter Write(Logger logger, Level level, String format, Object[] args)
        {
            // todo. this will mess up custom format specifiers!
            if (!IsMuted(logger, level)) Medium.Write(String.Format(format, args.Select(a => a.ToLog()).ToArray()));
            return this;
        }
    }
}
