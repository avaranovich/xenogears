using System;
using System.Diagnostics;

namespace XenoGears.Timers
{
    public class Timer
    {
        private readonly Stopwatch _timer;
        private readonly bool _autoreset;

        public Timer(bool autoreset = true)
        {
            _autoreset = autoreset;
            _timer = new Stopwatch();
            _timer.Start();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Reset()
        {
            _timer.Reset();
            _timer.Start();
        }

        public TimeSpan Elapsed()
        {
            var elapsed = _timer.Elapsed;
            if (_autoreset) Reset();
            return elapsed;
        }
    }
}
