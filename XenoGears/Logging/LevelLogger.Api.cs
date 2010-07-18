using System;
using System.Diagnostics;
using XenoGears.Functional;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public partial class LevelLogger
    {
        public Logger Logger { get; private set; }
        public Level Level { get; private set; }

        public LevelLogger(Logger logger, Level level)
        {
            Logger = logger;
            Level = level;

#if TRACE
            IsEnabled = true;
#endif
        }

        public LevelLogger Write(Object o)
        {
            RawWrite(o);
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

        public LevelLogger WriteLine(String format, params Object[] args)
        {
            return Write(format, args).Eoln();
        }

        private void RawWrite(Object o)
        {
            if (IsMuted()) return;
            PendingEolns.TimesDo(_ => LogWriter.Write(Logger, Level, Environment.NewLine));
            PendingEolns = 0;
            LogWriter.Write(Logger, Level, o);
        }
    }
}