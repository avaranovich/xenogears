using System;
using System.Diagnostics;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public static class LoggerExtensions
    {
        public static LevelLogger Debug(this Logger logger, Object o)
        {
            return logger.WriteLine(Level.Debug, o);
        }

        public static LevelLogger Debug(this Logger logger, String message)
        {
            return logger.WriteLine(Level.Debug, message);
        }

        public static LevelLogger Debug(this Logger logger, String format, params Object[] args)
        {
            return logger.WriteLine(Level.Debug, format, args);
        }

        public static LevelLogger Info(this Logger logger, Object o)
        {
            return logger.WriteLine(Level.Info, o);
        }

        public static LevelLogger Info(this Logger logger, String message)
        {
            return logger.WriteLine(Level.Info, message);
        }

        public static LevelLogger Info(this Logger logger, String format, params Object[] args)
        {
            return logger.WriteLine(Level.Info, format, args);
        }

        public static LevelLogger Warn(this Logger logger, Object o)
        {
            return logger.WriteLine(Level.Warn, o);
        }

        public static LevelLogger Warn(this Logger logger, String message)
        {
            return logger.WriteLine(Level.Warn, message);
        }

        public static LevelLogger Warn(this Logger logger, String format, params Object[] args)
        {
            return logger.WriteLine(Level.Warn, format, args);
        }

        public static LevelLogger Error(this Logger logger, Object o)
        {
            return logger.WriteLine(Level.Error, o);
        }

        public static LevelLogger Error(this Logger logger, String message)
        {
            return logger.WriteLine(Level.Error, message);
        }

        public static LevelLogger Error(this Logger logger, String format, params Object[] args)
        {
            return logger.WriteLine(Level.Error, format, args);
        }

        public static LevelLogger Fatal(this Logger logger, Object o)
        {
            return logger.WriteLine(Level.Fatal, o);
        }

        public static LevelLogger Fatal(this Logger logger, String message)
        {
            return logger.WriteLine(Level.Fatal, message);
        }

        public static LevelLogger Fatal(this Logger logger, String format, params Object[] args)
        {
            return logger.WriteLine(Level.Fatal, format, args);
        }
    }
}