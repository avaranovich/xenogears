using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace XenoGears.Strings.Writers
{
    [DebuggerNonUserCode]
    public abstract class BaseWriter : TextWriter
    {
        protected abstract void CoreWrite(String s);

        #region Boilerplate stuff

        public override void Write(bool value)
        {
            CoreWrite(value.ToString(FormatProvider));
        }

        public override void Write(char value)
        {
            CoreWrite(value.ToString(FormatProvider));
        }

        public override void Write(char[] buffer)
        {
            CoreWrite(new String(buffer));
        }

        public override void Write(double value)
        {
            CoreWrite(value.ToString(FormatProvider));
        }

        public override void Write(int value)
        {
            CoreWrite(value.ToString(FormatProvider));
        }

        public override void Write(long value)
        {
            CoreWrite(value.ToString(FormatProvider));
        }

        public override void Write(Object value)
        {
            CoreWrite(value.ToInvariantString());
        }

        public override void Write(float value)
        {
            CoreWrite(value.ToString(FormatProvider));
        }

        public override void Write(String s)
        {
            CoreWrite(s);
        }

        public override void Write(String format, Object arg0)
        {
            CoreWrite(String.Format(format, arg0));
        }

        public override void Write(String format, params Object[] arg)
        {
            CoreWrite(String.Format(format, arg.Select(a => a).ToArray()));
        }

        public override void Write(String format, Object arg0, Object arg1)
        {
            CoreWrite(String.Format(format, arg0, arg1));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            CoreWrite(new String(buffer, index, count));
        }

        public override void WriteLine()
        {
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(bool value)
        {
            CoreWrite(value.ToString(FormatProvider));
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(char value)
        {
            CoreWrite(value.ToString(FormatProvider));
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(double value)
        {
            CoreWrite(value.ToString(FormatProvider));
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(char[] buffer)
        {
            CoreWrite(new String(buffer));
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(int value)
        {
            CoreWrite(value.ToString(FormatProvider));
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(long value)
        {
            CoreWrite(value.ToString(FormatProvider));
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(Object value)
        {
            CoreWrite(value.ToString());
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(float value)
        {
            CoreWrite(value.ToString(FormatProvider));
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(String s)
        {
            CoreWrite(s);
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(uint value)
        {
            CoreWrite(value.ToString(FormatProvider));
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(String format, Object arg0)
        {
            CoreWrite(String.Format(format, arg0));
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(String format, params Object[] arg)
        {
            CoreWrite(String.Format(format, arg.Select(a => a).ToArray()));
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(String format, Object arg0, Object arg1)
        {
            CoreWrite(String.Format(format, arg0, arg1));
            CoreWrite(Environment.NewLine);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            CoreWrite(new String(buffer, index, count));
            CoreWrite(Environment.NewLine);
        }

        #endregion
    }
}