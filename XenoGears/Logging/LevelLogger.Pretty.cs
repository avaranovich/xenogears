namespace XenoGears.Logging
{
    public partial class LevelLogger
    {
        public LevelLogger Eoln()
        {
            Writer.Eoln();
            return this;
        }

        public LevelLogger Eolns(int eolns)
        {
            Writer.Eolns(eolns);
            return this;
        }

        public LevelLogger OneEoln()
        {
            Writer.OneEoln();
            return this;
        }

        public LevelLogger TwoEolns()
        {
            Writer.TwoEolns();
            return this;
        }

        public LevelLogger ThreeEolns()
        {
            Writer.ThreeEolns();
            return this;
        }

        public LevelLogger TenEolns()
        {
            Writer.TenEolns();
            return this;
        }

        public LevelLogger EnsureBlankLine()
        {
            Writer.EnsureBlankLine();
            return this;
        }

        public LevelLogger EnsureBlankLines(int blankLines)
        {
            Writer.EnsureBlankLines(blankLines);
            return this;
        }

        public LevelLogger EnsureOneBlankLine()
        {
            Writer.EnsureOneBlankLine();
            return this;
        }

        public LevelLogger EnsureTwoBlankLines()
        {
            Writer.EnsureTwoBlankLines();
            return this;
        }

        public LevelLogger EnsureThreeBlankLines()
        {
            Writer.EnsureThreeBlankLines();
            return this;
        }

        public LevelLogger EnsureTenBlankLines()
        {
            Writer.EnsureTenBlankLines();
            return this;
        }
    }
}