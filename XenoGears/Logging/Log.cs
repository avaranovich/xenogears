using System;
using System.Diagnostics;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public static class Log
    {
        private static readonly LevelLogger Impl = Logger.Adhoc.Debug;

        public static LevelLogger Write(Object o)
        {
            return Impl.Write(o);
        }

        public static LevelLogger Write(String message)
        {
            return Impl.Write(message);
        }

        public static LevelLogger Write(String format, params Object[] args)
        {
            return Impl.Write(format, args);
        }

        public static LevelLogger WriteLine(Object o)
        {
            return Impl.WriteLine(o);
        }

        public static LevelLogger WriteLine(String message)
        {
            return Impl.WriteLine(message);
        }

        public static LevelLogger WriteLine(String format, params Object[] args)
        {
            return Impl.WriteLine(format, args);
        }

        public static LevelLogger Eoln()
        {
            return Impl.Eoln();
        }

        public static LevelLogger Eolns(int eolns)
        {
            return Impl.Eolns(eolns);
        }

        public static LevelLogger OneEoln()
        {
            return Impl.OneEoln();
        }

        public static LevelLogger TwoEolns()
        {
            return Impl.TwoEolns();
        }

        public static LevelLogger ThreeEolns()
        {
            return Impl.ThreeEolns();
        }

        public static LevelLogger TenEolns()
        {
            return Impl.TenEolns();
        }

        public static LevelLogger EnsureBlankLine()
        {
            return Impl.EnsureBlankLine();
        }

        public static LevelLogger EnsureBlankLines(int blankLines)
        {
            return Impl.EnsureBlankLines(blankLines);
        }

        public static LevelLogger EnsureOneBlankLine()
        {
            return Impl.EnsureOneBlankLine();
        }

        public static LevelLogger EnsureTwoBlankLines()
        {
            return Impl.EnsureTwoBlankLines();
        }

        public static LevelLogger EnsureThreeBlankLines()
        {
            return Impl.EnsureThreeBlankLines();
        }

        public static LevelLogger EnsureTenBlankLines()
        {
            return Impl.EnsureTenBlankLines();
        }
    }
}