using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Traits.Disposable;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public partial class Logger
    {
        public readonly Guid Id = Guid.NewGuid();
        public String Name { get; set; }

        public LogWriter Writer { get; private set; }
        public IDisposable OverrideWriter(StringBuilder new_out) { return OverrideWriter(new StringWriter(new_out)); }
        public IDisposable OverrideWriter(TextWriter new_out) { var new_writer = LogWriter.Get(Guid.NewGuid().ToString()); new_writer.Medium = new_out; return OverrideWriter(new_writer); }
        public IDisposable OverrideWriter(String new_out) { return OverrideWriter(LogWriter.Get(new_out)); }
        public IDisposable OverrideWriter(Type new_out) { return OverrideWriter(LogWriter.Get(new_out)); }
        public IDisposable OverrideWriter(LogWriter new_out)
        {
            var old_out = Writer;
            Writer = new_out;
            var this_override = new DisposableAction(() => Writer = old_out);

            var debug_override = Debug.OverrideWriter(new_out);
            var info_override = Info.OverrideWriter(new_out);
            var warn_override = Warn.OverrideWriter(new_out);
            var error_override = Error.OverrideWriter(new_out);
            var fatal_override = Fatal.OverrideWriter(new_out);

            return this_override + debug_override + info_override + warn_override + error_override + fatal_override;
        }

        public LevelLogger Debug { get; private set; }
        public LevelLogger Info { get; private set; }
        public LevelLogger Warn { get; private set; }
        public LevelLogger Error { get; private set; }
        public LevelLogger Fatal { get; private set; }
        public LevelLogger this[Level level] 
        { 
            get
            {
                switch (level)
                {
                    case Level.Debug:
                        return Debug;
                    case Level.Info:
                        return Info;
                    case Level.Warn:
                        return Warn;
                    case Level.Error:
                        return Error;
                    case Level.Fatal:
                        return Fatal;
                    default:
                        throw AssertionHelper.Fail();
                }
            }
        }

        internal Logger(String name)
            : this(name, null)
        {
        }

        internal Logger(String name, LogWriter writer)
        {
            Name = name;

            Debug = new LevelLogger(Level.Debug, this);
            Info = new LevelLogger(Level.Info, this);
            Warn = new LevelLogger(Level.Warn, this);
            Error = new LevelLogger(Level.Error, this);
            Fatal = new LevelLogger(Level.Fatal, this);
            Writer = writer ?? LogWriter.Adhoc;

#if DEBUG
            MinLevel = Level.Debug;
#else
            MinLevel = Level.Info;
#endif

            IsEnabled = true;
        }

        public LevelLogger Write(Level level, Object o)
        {
            return this[level].Write(o);
        }

        public LevelLogger Write(Level level, String message)
        {
            return this[level].Write(message);
        }

        public LevelLogger Write(Level level, String format, params Object[] args)
        {
            return this[level].Write(format, args);
        }

        public LevelLogger WriteLine(Level level)
        {
            return this[level].WriteLine();
        }

        public LevelLogger WriteLine(Level level, Object o)
        {
            return this[level].WriteLine(o);
        }

        public LevelLogger WriteLine(Level level, String message)
        {
            return this[level].WriteLine(message);
        }

        public LevelLogger WriteLine(Level level, String format, params Object[] args)
        {
            return this[level].WriteLine(format, args);
        }
    }
}
