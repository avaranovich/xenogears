using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Strings.Writers
{
    [DebuggerNonUserCode]
    public class DelayedWriter : BaseWriter
    {
        private bool _isDelayed = false;
        private StringBuilder _buf = null;
        private Queue<Action> _eagerWrites = new Queue<Action>();
        private Queue<Action> _delayedWrites = new Queue<Action>();

        public DelayedWriter(TextWriter writer)
            : base(writer)
        {
        }

        public void Delay(Action action)
        {
            action.AssertNotNull();
            _isDelayed = true;

            _delayedWrites.Enqueue(action);
            _buf = new StringBuilder();
            _eagerWrites.Enqueue(() => this.Write(_buf.ToString()));
        }

        public void Delay(Action<DelayedWriter> action)
        {
            Delay(() => action(this));
        }

        public void Commit()
        {
            try
            {
                _isDelayed = true;

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
                _isDelayed = true;
            }

            _buf = null;
            _isDelayed = false;
        }

        protected override void CoreWrite(char c)
        {
            if (_isDelayed)
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