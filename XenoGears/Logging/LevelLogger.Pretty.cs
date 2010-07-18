using System;

namespace XenoGears.Logging
{
    public partial class LevelLogger
    {
        public int PendingEolns { get; set; }

        public LevelLogger Eoln()
        {
            return Eolns(1);
        }

        public LevelLogger Eolns(int eolns)
        {
            PendingEolns += eolns;
            return this;
        }

        public LevelLogger OneEoln()
        {
            return Eolns(1);
        }

        public LevelLogger TwoEolns()
        {
            return Eolns(2);
        }

        public LevelLogger ThreeEolns()
        {
            return Eolns(3);
        }

        public LevelLogger TenEolns()
        {
            return Eolns(10);
        }

        public LevelLogger EnsureBlankLine()
        {
            return EnsureBlankLines(1);
        }

        public LevelLogger EnsureBlankLines(int eolns)
        {
            PendingEolns = Math.Max(PendingEolns, eolns + 1);
            return this;
        }

        public LevelLogger EnsureOneEoln()
        {
            return EnsureBlankLines(1);
        }

        public LevelLogger EnsureTwoEolns()
        {
            return EnsureBlankLines(2);
        }

        public LevelLogger EnsureThreeEolns()
        {
            return EnsureBlankLines(3);
        }

        public LevelLogger EnsureTenEolns()
        {
            return EnsureBlankLines(10);
        }

        public LevelLogger LeaveBlankLine()
        {
            return LeaveBlankLines(1);
        }

        public LevelLogger LeaveBlankLines(int eolns)
        {
            PendingEolns = Math.Min(PendingEolns, eolns + 1);
            return this;
        }

        public LevelLogger LeaveOneEoln()
        {
            return LeaveBlankLines(1);
        }

        public LevelLogger LeaveTwoEolns()
        {
            return LeaveBlankLines(2);
        }

        public LevelLogger LeaveThreeEolns()
        {
            return LeaveBlankLines(3);
        }

        public LevelLogger LeaveTenEolns()
        {
            return LeaveBlankLines(10);
        }

        public LevelLogger TrimBlankLine()
        {
            return TrimBlankLines(1);
        }

        public LevelLogger TrimBlankLines(int eolns)
        {
            PendingEolns = Math.Max(PendingEolns, PendingEolns - (eolns + 1));
            return this;
        }

        public LevelLogger TrimOneEoln()
        {
            return TrimBlankLines(1);
        }

        public LevelLogger TrimTwoEolns()
        {
            return TrimBlankLines(2);
        }

        public LevelLogger TrimThreeEolns()
        {
            return TrimBlankLines(3);
        }

        public LevelLogger TrimTenEolns()
        {
            return TrimBlankLines(10);
        }

        public LevelLogger TrimAllBlankLines()
        {
            PendingEolns = Math.Max(PendingEolns, 1);
            return this;
        }
    }
}