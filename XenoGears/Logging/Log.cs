using System;
using System.Diagnostics;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public static class Log
    {
        private static readonly LevelLogger Impl = Logger.Get("Adhoc").Debug;

        public static LevelLogger Write(Object o)
        {
            return Impl.Write(o);
        }

        public static LevelLogger Write(String format, params Object[] args)
        {
            return Impl.Write(format, args);
        }

        public static LevelLogger WriteLine(Object o)
        {
            return Impl.WriteLine(o);
        }

        public static LevelLogger WriteLine(String format, params Object[] args)
        {
            return Impl.WriteLine(format, args);
        }

        public static int PendingEolns
        {
            get { return Impl.PendingEolns; }
            set { Impl.PendingEolns = value; }
        }

        public static LevelLogger Eoln()
        {
            return Eolns(1);
        }

        public static LevelLogger Eolns(int eolns)
        {
            return Impl.Eolns(eolns);
        }

        public static LevelLogger OneEoln()
        {
            return Eolns(1);
        }

        public static LevelLogger TwoEolns()
        {
            return Eolns(2);
        }

        public static LevelLogger ThreeEolns()
        {
            return Eolns(3);
        }

        public static LevelLogger TenEolns()
        {
            return Eolns(10);
        }

        public static LevelLogger EnsureBlankLine()
        {
            return EnsureBlankLines(1);
        }

        public static LevelLogger EnsureBlankLines(int BlankLines)
        {
            return Impl.EnsureBlankLines(BlankLines);
        }

        public static LevelLogger EnsureOneBlankLine()
        {
            return EnsureBlankLines(1);
        }

        public static LevelLogger EnsureTwoBlankLines()
        {
            return EnsureBlankLines(2);
        }

        public static LevelLogger EnsureThreeBlankLines()
        {
            return EnsureBlankLines(3);
        }

        public static LevelLogger EnsureTenBlankLines()
        {
            return EnsureBlankLines(10);
        }

        public static LevelLogger LeaveBlankLine()
        {
            return LeaveBlankLines(1);
        }

        public static LevelLogger LeaveBlankLines(int BlankLines)
        {
            return Impl.LeaveBlankLines(BlankLines);
        }

        public static LevelLogger LeaveOneBlankLine()
        {
            return LeaveBlankLines(1);
        }

        public static LevelLogger LeaveTwoBlankLines()
        {
            return LeaveBlankLines(2);
        }

        public static LevelLogger LeaveThreeBlankLines()
        {
            return LeaveBlankLines(3);
        }

        public static LevelLogger LeaveTenBlankLines()
        {
            return LeaveBlankLines(10);
        }

        public static LevelLogger TrimBlankLine()
        {
            return TrimBlankLines(1);
        }

        public static LevelLogger TrimBlankLines(int BlankLines)
        {
            return Impl.TrimBlankLines(BlankLines);
        }

        public static LevelLogger TrimOneBlankLine()
        {
            return TrimBlankLines(1);
        }

        public static LevelLogger TrimTwoBlankLines()
        {
            return TrimBlankLines(2);
        }

        public static LevelLogger TrimThreeBlankLines()
        {
            return TrimBlankLines(3);
        }

        public static LevelLogger TrimTenBlankLines()
        {
            return TrimBlankLines(10);
        }

        public static LevelLogger TrimAllBlankLines()
        {
            return Impl.TrimAllBlankLines();
        }
    }
}