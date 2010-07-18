namespace XenoGears.Logging
{
    public partial class LevelLogger
    {
        public LevelLogger Eoln()
        {
            if (!IsMuted()) Writer.Eoln();
            return this;
        }

        public LevelLogger Eolns(int eolns)
        {
            if (!IsMuted()) Writer.Eolns(eolns);
            return this;
        }

        public LevelLogger OneEoln()
        {
            if (!IsMuted()) Writer.OneEoln();
            return this;
        }

        public LevelLogger TwoEolns()
        {
            if (!IsMuted()) Writer.TwoEolns();
            return this;
        }

        public LevelLogger ThreeEolns()
        {
            if (!IsMuted()) Writer.ThreeEolns();
            return this;
        }

        public LevelLogger TenEolns()
        {
            if (!IsMuted()) Writer.TenEolns();
            return this;
        }

        public LevelLogger EnsureBlankLine()
        {
            if (!IsMuted()) Writer.EnsureBlankLine();
            return this;
        }

        public LevelLogger EnsureBlankLines(int blankLines)
        {
            if (!IsMuted()) Writer.EnsureBlankLines(blankLines);
            return this;
        }

        public LevelLogger EnsureOneBlankLine()
        {
            if (!IsMuted()) Writer.EnsureOneBlankLine();
            return this;
        }

        public LevelLogger EnsureTwoBlankLines()
        {
            if (!IsMuted()) Writer.EnsureTwoBlankLines();
            return this;
        }

        public LevelLogger EnsureThreeBlankLines()
        {
            if (!IsMuted()) Writer.EnsureThreeBlankLines();
            return this;
        }

        public LevelLogger EnsureTenBlankLines()
        {
            if (!IsMuted()) Writer.EnsureTenBlankLines();
            return this;
        }
    }
}