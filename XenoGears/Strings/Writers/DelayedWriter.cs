using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Strings.Writers
{
    [DebuggerNonUserCode]
    public class DelayedWriter : BaseWriterWrapper
    {
        public bool IsDelayed { get; set; }
        private StringBuilder _buf = null;
        private Queue<Action> _eagerWrites = new Queue<Action>();
        private Queue<Action> _delayedWrites = new Queue<Action>();

        public DelayedWriter(StringBuilder buf)
            : base(buf)
        {
        }

        public void Delay(Action action)
        {
            action.AssertNotNull();
            IsDelayed = true;

            _delayedWrites.Enqueue(action);
            var buf = _buf = new StringBuilder();
            _eagerWrites.Enqueue(() => this.Write(buf.ToString()));
        }

        public void Delay(Action<DelayedWriter> action)
        {
            Delay(() => action(this));
        }

        public void Commit()
        {
            try
            {
                IsDelayed = false;

                while (_delayedWrites.IsNotEmpty())
                {
                    var delayed = _delayedWrites.Dequeue();
                    delayed();

                    var eager = _eagerWrites.Dequeue();
                    eager();
                }

                _delayedWrites.AssertEmpty();
                _eagerWrites.AssertEmpty();
            }
            finally
            {
                IsDelayed = true;
            }

            _buf = null;
            IsDelayed = false;
        }

        protected override void CoreWrite(char c)
        {
            if (IsDelayed)
            {
                _buf.Append(c);
            }
            else
            {
                InnerWriter.Write(c);
            }
        }
    }
}