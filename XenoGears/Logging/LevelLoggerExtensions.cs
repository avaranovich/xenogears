using System;
using System.Diagnostics;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public static class LevelLoggerExtensions
    {
        public static LevelLogger Debug(this LevelLogger logger, Object o)
        {
            return logger.Logger.Debug(o);
        }

        public static LevelLogger Debug(this LevelLogger logger, String message)
        {
            return logger.Logger.Debug(message);
        }

        public static LevelLogger Debug(this LevelLogger logger, String format, params Object[] args)
        {
            return logger.Logger.Debug(format, args);
        }

        public static LevelLogger Info(this LevelLogger logger, Object o)
        {
            return logger.Logger.Info(o);
        }

        public static LevelLogger Info(this LevelLogger logger, String message)
        {
            return logger.Logger.Info(message);
        }

        public static LevelLogger Info(this LevelLogger logger, String format, params Object[] args)
        {
            return logger.Logger.Info(format, args);
        }

        public static LevelLogger Warn(this LevelLogger logger, Object o)
        {
            return logger.Logger.Warn(o);
        }

        public static LevelLogger Warn(this LevelLogger logger, String message)
        {
            return logger.Logger.Warn(message);
        }

        public static LevelLogger Warn(this LevelLogger logger, String format, params Object[] args)
        {
            return logger.Logger.Warn(format, args);
        }

        public static LevelLogger Error(this LevelLogger logger, Object o)
        {
            return logger.Logger.Error(o);
        }

        public static LevelLogger Error(this LevelLogger logger, String message)
        {
            return logger.Logger.Error(message);
        }

        public static LevelLogger Error(this LevelLogger logger, String format, params Object[] args)
        {
            return logger.Logger.Error(format, args);
        }

        public static LevelLogger Fatal(this LevelLogger logger, Object o)
        {
            return logger.Logger.Fatal(o);
        }

        public static LevelLogger Fatal(this LevelLogger logger, String message)
        {
            return logger.Logger.Fatal(message);
        }

        public static LevelLogger Fatal(this LevelLogger logger, String format, params Object[] args)
        {
            return logger.Logger.Fatal(format, args);
        }
    }
}