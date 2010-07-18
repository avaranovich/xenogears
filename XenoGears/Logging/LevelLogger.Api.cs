using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using XenoGears.Traits.Disposable;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public partial class LevelLogger
    {
        public readonly Guid Id = Guid.NewGuid();
        public Level Level { get; private set; }
        public Logger Logger { get; private set; }

        public LogWriter Writer { get; private set; }
        public IDisposable OverrideWriter(StringBuilder new_out) { return OverrideWriter(new StringWriter(new_out)); }
        public IDisposable OverrideWriter(TextWriter new_out) { var new_writer = LogWriter.Get(Guid.NewGuid().ToString()); new_writer.Medium = new_out; return OverrideWriter(new_writer); }
        public IDisposable OverrideWriter(String new_out) { return OverrideWriter(LogWriter.Get(new_out)); }
        public IDisposable OverrideWriter(Type new_out) { return OverrideWriter(LogWriter.Get(new_out)); }
        public IDisposable OverrideWriter(LogWriter new_out)
        {
            var old_out = Writer;
            Writer = new_out;
            return new DisposableAction(() => Writer = old_out);
        }

        public LevelLogger(Level level, Logger logger)
        {
            Level = level;
            Logger = logger;
            Writer = logger.Writer ?? LogWriter.Adhoc;

#if TRACE
            IsEnabled = true;
#endif
        }

        public LevelLogger Write(Object o)
        {
            RawWrite(o);
            return this;
        }

        public LevelLogger Write(String message)
        {
            RawWrite(message);
            return this;
        }

        public LevelLogger Write(String format, params Object[] args)
        {
            RawWrite(String.Format(format, args));
            return this;
        }

        public LevelLogger WriteLine(Object o)
        {
            return Write(o).Eoln();
        }

        public LevelLogger WriteLine(String message)
        {
            return Write(message).Eoln();
        }

        public LevelLogger WriteLine(String format, params Object[] args)
        {
            return Write(format, args).Eoln();
        }

        private void RawWrite(Object o)
        {
            if (IsMuted()) return;
            Writer.Write(Logger, Level, o);
        }
    }
}