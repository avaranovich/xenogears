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

        public LevelLogger EnsureBlankLines(int BlankLines)
        {
            PendingEolns = Math.Max(PendingEolns, BlankLines + 1);
            return this;
        }

        public LevelLogger EnsureOneBlankLine()
        {
            return EnsureBlankLines(1);
        }

        public LevelLogger EnsureTwoBlankLines()
        {
            return EnsureBlankLines(2);
        }

        public LevelLogger EnsureThreeBlankLines()
        {
            return EnsureBlankLines(3);
        }

        public LevelLogger EnsureTenBlankLines()
        {
            return EnsureBlankLines(10);
        }

        public LevelLogger LeaveBlankLine()
        {
            return LeaveBlankLines(1);
        }

        public LevelLogger LeaveBlankLines(int BlankLines)
        {
            PendingEolns = Math.Min(PendingEolns, BlankLines + 1);
            return this;
        }

        public LevelLogger LeaveOneBlankLine()
        {
            return LeaveBlankLines(1);
        }

        public LevelLogger LeaveTwoBlankLines()
        {
            return LeaveBlankLines(2);
        }

        public LevelLogger LeaveThreeBlankLines()
        {
            return LeaveBlankLines(3);
        }

        public LevelLogger LeaveTenBlankLines()
        {
            return LeaveBlankLines(10);
        }

        public LevelLogger TrimBlankLine()
        {
            return TrimBlankLines(1);
        }

        public LevelLogger TrimBlankLines(int BlankLines)
        {
            PendingEolns = Math.Max(PendingEolns, PendingEolns - (BlankLines + 1));
            return this;
        }

        public LevelLogger TrimOneBlankLine()
        {
            return TrimBlankLines(1);
        }

        public LevelLogger TrimTwoBlankLines()
        {
            return TrimBlankLines(2);
        }

        public LevelLogger TrimThreeBlankLines()
        {
            return TrimBlankLines(3);
        }

        public LevelLogger TrimTenBlankLines()
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