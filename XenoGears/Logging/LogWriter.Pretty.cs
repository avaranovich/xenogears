using System;

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
            throw new NotImplementedException();
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

        public LogWriter EnsureBlankLines(int BlankLines)
        {
            throw new NotImplementedException();
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
