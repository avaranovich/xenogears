using System;
using System.Diagnostics;
using XenoGears.Assertions;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public class Logger
    {
        public String Name { get; set; }
        public Level Level { get; set; }

        public bool IsEnabled { get; set; }
        public void Enable() { IsEnabled = true; }
        public void Disable() { IsEnabled = false; }

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
        {
            Name = name;

            Debug = new LevelLogger(this, Level.Debug);
            Info = new LevelLogger(this, Level.Info);
            Warn = new LevelLogger(this, Level.Warn);
            Error = new LevelLogger(this, Level.Error);
            Fatal = new LevelLogger(this, Level.Fatal);

#if DEBUG
            Level = Level.Debug;
#else
            Level = Level.Info;
#endif

            IsEnabled = true;
        }

        public LevelLogger Write(Level level, Object o)
        {
            return this[level].Write(o);
        }

        public LevelLogger Write(Level level, String format, params Object[] args)
        {
            return this[level].Write(format, args);
        }

        public LevelLogger WriteLine(Level level, Object o)
        {
            return this[level].WriteLine(o);
        }

        public LevelLogger WriteLine(Level level, String format, params Object[] args)
        {
            return this[level].WriteLine(format, args);
        }
    }
}
