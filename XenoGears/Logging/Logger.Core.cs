using System;
using System.Diagnostics;
using XenoGears.Assertions;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public partial class Logger
    {
        public String Name { get; set; }

        private LogWriter _writer;
        public LogWriter Writer
        {
            get { return _writer; }
            set
            {
                _writer = value;
                Debug.Writer = value;
                Info.Writer = value;
                Warn.Writer = value;
                Error.Writer = value;
                Fatal.Writer = value;
            }
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
            Writer = writer ?? LogWriter.Get(name == "Console" ? "Console" : "Adhoc");

            Debug = new LevelLogger(Level.Debug, this);
            Info = new LevelLogger(Level.Info, this);
            Warn = new LevelLogger(Level.Warn, this);
            Error = new LevelLogger(Level.Error, this);
            Fatal = new LevelLogger(Level.Fatal, this);

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
