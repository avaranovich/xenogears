using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Strings.Writers
{
    [DebuggerNonUserCode]
    public abstract class BaseWriter : TextWriter
    {
        public TextWriter InnerWriter { get; private set; }
        public override Encoding Encoding { get { return InnerWriter.Encoding; } }
        public override String NewLine { get { return InnerWriter.NewLine; } set { InnerWriter.NewLine = value; } }

        protected BaseWriter(TextWriter writer)
            : base(writer.FormatProvider)
        {
            InnerWriter = writer;
        }

        protected virtual void BeforeWrite(Object o, Type t)
        {
        }

        protected virtual void BeforeWriteLine()
        {
        }

        protected virtual void CoreWrite(char c)
        {
            InnerWriter.Write(c);
        }

        protected virtual void AfterWriteLine()
        {
        }

        protected virtual void AfterWrite(Object o, Type t)
        {
        }

        #region Eoln tracking

        private int pos = 0, cnt = 0;
        private readonly char[] lb = new char[10];
        private void push(char c) { lb[pos] = c; pos = (pos + 1) % 10; cnt++; }
        private bool is_eoln()
        {
            if (NewLine == null) return false;
            var len = NewLine.Length.AssertThat(i => i <= 10);
            if (cnt < len) return false;

            var nl_indices = 0.UpTo(len - 1);
            var lb_indices = pos.Unfolde(i => ((i - 1) + 10) % 10).Take(len).Reverse();
            return nl_indices.Zip(lb_indices, (nl_i, lb_i) => NewLine[nl_i] == lb[lb_i]).All();
        }

        private void WriteProxy(char c)
        {
            push(c);

            if (is_eoln()) BeforeWriteLine();
            CoreWrite(c);
            if (is_eoln()) AfterWriteLine();
        }

        #endregion

        #region Boilerplate stuff

        private int _pendingWrites = 0;

        public override void Close()
        {
            InnerWriter.Close();
        }

        public override void Flush()
        {
            InnerWriter.Flush();
        }

        public override void Write(bool value)
        {
            _pendingWrites++;

            try
            {
                if (_pendingWrites == 1) BeforeWrite(value, typeof(bool));
                Write(value ? "True" : "False");
                if (_pendingWrites == 1) AfterWrite(value, typeof(bool));
            }
            finally
            {
                _pendingWrites--;
            }
        }

        public override void Write(char value)
        {
            _pendingWrites++;

            try
            {
                if (_pendingWrites == 1) BeforeWrite(value, typeof(char));
                WriteProxy(value);
                if (_pendingWrites == 1) AfterWrite(value, typeof(char));
            }
            finally
            {
                _pendingWrites--;
            }
        }

        public override void Write(char[] buffer)
        {
            _pendingWrites++;

            try
            {
                if (_pendingWrites == 1) BeforeWrite(buffer, typeof(char[]));
                if (buffer != null)
                {
                    Write(buffer, 0, buffer.Length);
                }
                if (_pendingWrites == 1) AfterWrite(buffer, typeof(char[]));
            }
            finally
            {
                _pendingWrites--;
            }
        }

        public override void Write(double value)
        {
            _pendingWrites++;

            try
            {
                if (_pendingWrites == 1) BeforeWrite(value, typeof(double));
                Write(value.ToString(FormatProvider));
                if (_pendingWrites == 1) AfterWrite(value, typeof(double));
            }
            finally
            {
                _pendingWrites--;
            }
        }

        public override void Write(int value)
        {
            _pendingWrites++;

            try
            {
                if (_pendingWrites == 1) BeforeWrite(value, typeof(int));
                Write(value.ToString(FormatProvider));
                if (_pendingWrites == 1) AfterWrite(value, typeof(int));
            }
            finally
            {
                _pendingWrites--;
            }
        }

        public override void Write(long value)
        {
            _pendingWrites++;

            try
            {
                if (_pendingWrites == 1) BeforeWrite(value, typeof(long));
                Write(value.ToString(FormatProvider));
                if (_pendingWrites == 1) AfterWrite(value, typeof(long));
            }
            finally
            {
                _pendingWrites--;
            }
        }

        public override void Write(Object value)
        {
            _pendingWrites++;

            try
            {
                if (_pendingWrites == 1) BeforeWrite(value, typeof(Object));
                if (value != null)
                {
                    var formattable = value as IFormattable;
                    if (formattable != null)
                    {
                        Write(formattable.ToString(null, FormatProvider));
                    }
                    else
                    {
                        Write(value.ToString());
                    }
                }
                if (_pendingWrites == 1) AfterWrite(value, typeof(Object));
            }
            finally
            {
                _pendingWrites--;
            }
        }

        public override void Write(float value)
        {
            try
            {
                _pendingWrites++;

                if (_pendingWrites == 1) BeforeWrite(value, typeof(float));
                Write(value.ToString(FormatProvider));
                if (_pendingWrites == 1) AfterWrite(value, typeof(float));
            }
            finally
            {
                _pendingWrites--;
            }
        }

        public override void Write(String s)
        {
            try
            {
                _pendingWrites++;

                if (_pendingWrites == 1) BeforeWrite(s, typeof(String));
                if (s != null)
                {
                    Write(s.ToCharArray());
                }
                if (_pendingWrites == 1) AfterWrite(s, typeof(String));
            }
            finally
            {
                _pendingWrites--;
            }
        }

        public override void Write(String format, Object arg0)
        {
            Write(String.Format(FormatProvider, format, new[] { arg0 }));
        }

        public override void Write(String format, params Object[] arg)
        {
            Write(String.Format(FormatProvider, format, arg));
        }

        public override void Write(String format, Object arg0, Object arg1)
        {
            Write(String.Format(FormatProvider, format, new[] { arg0, arg1 }));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            _pendingWrites++;

            if (_pendingWrites == 1) BeforeWrite(buffer, typeof(char[]));

            buffer.AssertNotNull();
            (index >= 0).AssertTrue();
            (count >= 0).AssertTrue();
            (buffer.Length >= index + count).AssertTrue();

            for (var i = 0; i < count; i++)
            {
                Write(buffer[index + i]);
            }

            if (_pendingWrites == 1) AfterWrite(buffer, typeof(char[]));
        }

        public override void WriteLine()
        {
            Write(NewLine);
        }

        public override void WriteLine(bool value)
        {
            Write(value);
            WriteLine();
        }

        public override void WriteLine(char value)
        {
            Write(value);
            WriteLine();
        }

        public override void WriteLine(double value)
        {
            Write(value);
            WriteLine();
        }

        public override void WriteLine(char[] buffer)
        {
            Write(buffer);
            WriteLine();
        }

        public override void WriteLine(int value)
        {
            Write(value);
            WriteLine();
        }

        public override void WriteLine(long value)
        {
            Write(value);
            WriteLine();
        }

        public override void WriteLine(Object value)
        {
            Write(value);
            WriteLine();
        }

        public override void WriteLine(float value)
        {
            Write(value);
            WriteLine();
        }

        public override void WriteLine(String s)
        {
            Write(s);
            WriteLine();
        }

        public override void WriteLine(uint value)
        {
            Write(value);
            WriteLine();
        }

        public override void WriteLine(String format, Object arg0)
        {
            Write(format, arg0);
            WriteLine();
        }

        public override void WriteLine(String format, params Object[] arg)
        {
            Write(format, arg);
            WriteLine();
        }

        public override void WriteLine(String format, Object arg0, Object arg1)
        {
            Write(format, arg0, arg1);
            WriteLine();
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            Write(buffer, index, count);
            WriteLine();
        }

        #endregion
    }
}