using System;
using XenoGears.Functional;

namespace XenoGears.Logging
{
    public partial class LogWriter
    {
        public LogWriter Eoln()
        {
            return Eolns(1);
        }

        public LogWriter Eolns(int eolns)
        {
            eolns.TimesDo(_ => Medium.WriteLine());
            PreviouslyWrittenEolns += eolns;
            return this;
        }

        public LogWriter OneEoln()
        {
            return Eolns(1);
        }

        public LogWriter TwoEolns()
        {
            return Eolns(2);
        }

        public LogWriter ThreeEolns()
        {
            return Eolns(3);
        }

        public LogWriter TenEolns()
        {
            return Eolns(10);
        }

        public LogWriter EnsureBlankLine()
        {
            return EnsureBlankLines(1);
        }

        public LogWriter EnsureBlankLines(int blankLines)
        {
            // if we haven't yet written anything, there's no point in inserting blank lines
            if (HasWrittenAnything)
            {
                var eolns = blankLines + 1;
                var to_write = Math.Max(eolns - PreviouslyWrittenEolns, 0);
                to_write.TimesDo(_ => Medium.WriteLine());
                PreviouslyWrittenEolns = Math.Max(eolns, PreviouslyWrittenEolns);
            }

            return this;
        }

        public LogWriter EnsureOneBlankLine()
        {
            return EnsureBlankLines(1);
        }

        public LogWriter EnsureTwoBlankLines()
        {
            return EnsureBlankLines(2);
        }

        public LogWriter EnsureThreeBlankLines()
        {
            return EnsureBlankLines(3);
        }

        public LogWriter EnsureTenBlankLines()
        {
            return EnsureBlankLines(10);
        }
    }
}
