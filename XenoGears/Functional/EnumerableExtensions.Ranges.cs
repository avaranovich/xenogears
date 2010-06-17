using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static LinearIntegerRange UpTo(this int start, int stop)
        {
            return new LinearIntegerRange(start, stop);
        }

        public static ReverseLinearIntegerRange DownTo(this int start, int stop)
        {
            return new ReverseLinearIntegerRange(start, stop);
        }
    }

    [DebuggerNonUserCode]
    public class LinearIntegerRange : IEnumerable<int>
    {
        private readonly int _start;
        private readonly int _stop;
        private readonly int _step;

        public LinearIntegerRange(int start, int stop)
            : this(start, stop, 1)
        {
        }

        public LinearIntegerRange(int start, int stop, int step)
        {
            _start = start;
            _stop = stop;
            _step = step;
        }

        public LinearIntegerRange Step(int step)
        {
            return new LinearIntegerRange(_start, _stop, step);
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator<int> GetEnumerator()
        {
            return _start.Unfold(i => i + _step, i => i <= _stop).GetEnumerator();
        }
    }

    [DebuggerNonUserCode]
    public class ReverseLinearIntegerRange : IEnumerable<int>
    {
        private readonly int _start;
        private readonly int _stop;
        private readonly int _step;

        public ReverseLinearIntegerRange(int start, int stop)
            : this(start, stop, 1)
        {
        }

        public ReverseLinearIntegerRange(int start, int stop, int step)
        {
            _start = start;
            _stop = stop;
            _step = step;
        }

        public ReverseLinearIntegerRange Step(int step)
        {
            return new ReverseLinearIntegerRange(_start, _stop, step);
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator<int> GetEnumerator()
        {
            return _start.Unfold(i => i - _step, i => i >= _stop).GetEnumerator();
        }
    }
}
