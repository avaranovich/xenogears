using System;
using System.Diagnostics;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public static class Log
    {
        private static readonly LevelLogger Impl = LogFactory.GetLogger("ad-hoc").Debug;

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

        public static LevelLogger EnsureBlankLines(int eolns)
        {
            return Impl.EnsureBlankLines(eolns);
        }

        public static LevelLogger EnsureOneEoln()
        {
            return EnsureBlankLines(1);
        }

        public static LevelLogger EnsureTwoEolns()
        {
            return EnsureBlankLines(2);
        }

        public static LevelLogger EnsureThreeEolns()
        {
            return EnsureBlankLines(3);
        }

        public static LevelLogger EnsureTenEolns()
        {
            return EnsureBlankLines(10);
        }

        public static LevelLogger LeaveBlankLine()
        {
            return LeaveBlankLines(1);
        }

        public static LevelLogger LeaveBlankLines(int eolns)
        {
            return Impl.LeaveBlankLines(eolns);
        }

        public static LevelLogger LeaveOneEoln()
        {
            return LeaveBlankLines(1);
        }

        public static LevelLogger LeaveTwoEolns()
        {
            return LeaveBlankLines(2);
        }

        public static LevelLogger LeaveThreeEolns()
        {
            return LeaveBlankLines(3);
        }

        public static LevelLogger LeaveTenEolns()
        {
            return LeaveBlankLines(10);
        }

        public static LevelLogger TrimBlankLine()
        {
            return TrimBlankLines(1);
        }

        public static LevelLogger TrimBlankLines(int eolns)
        {
            return Impl.TrimBlankLines(eolns);
        }

        public static LevelLogger TrimOneEoln()
        {
            return TrimBlankLines(1);
        }

        public static LevelLogger TrimTwoEolns()
        {
            return TrimBlankLines(2);
        }

        public static LevelLogger TrimThreeEolns()
        {
            return TrimBlankLines(3);
        }

        public static LevelLogger TrimTenEolns()
        {
            return TrimBlankLines(10);
        }

        public static LevelLogger TrimAllBlankLines()
        {
            return Impl.TrimAllBlankLines();
        }
    }
}