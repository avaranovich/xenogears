using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;

namespace XenoGears.Logging.Media
{
    [DebuggerNonUserCode]
    public class AdhocMedium : TextWriter
    {
        public override Encoding Encoding { get { return Medium.Encoding; } }

        private TextWriter Console { get { return System.Console.Out; } }
        private TextWriter Trace = new TraceMedium();
        private TextWriter Medium
        {
            get
            {
                var unit_test = UnitTest.CurrentTest;
//                return unit_test != null ? Trace : Console;
                return Console;
            }
        }

        public override void Close()
        {
            Medium.Close();
        }

        public override void Flush()
        {
            Medium.Flush();
        }

        public override void Write(bool value)
        {
            Medium.Write(value);
        }

        public override void Write(char value)
        {
            Medium.Write(value);
        }

        public override void Write(char[] buffer)
        {
            Medium.Write(buffer);
        }

        public override void Write(double value)
        {
            Medium.Write(value);
        }

        public override void Write(int value)
        {
            Medium.Write(value);
        }

        public override void Write(long value)
        {
            Medium.Write(value);
        }

        public override void Write(Object value)
        {
            Medium.Write(value);
        }

        public override void Write(float value)
        {
            Medium.Write(value);
        }

        public override void Write(String s)
        {
            Medium.Write(s);
        }

        public override void Write(String format, Object arg0)
        {
            Medium.Write(format, arg0);
        }

        public override void Write(String format, params Object[] arg)
        {
            Medium.Write(format, arg.Select(a => a).ToArray());
        }

        public override void Write(String format, Object arg0, Object arg1)
        {
            Medium.Write(format, arg0, arg1);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Medium.Write(buffer, index, count);
        }

        public override void WriteLine()
        {
            Medium.WriteLine();
        }

        public override void WriteLine(bool value)
        {
            Medium.WriteLine(value);
        }

        public override void WriteLine(char value)
        {
            Medium.WriteLine(value);
        }

        public override void WriteLine(double value)
        {
            Medium.WriteLine(value);
        }

        public override void WriteLine(char[] buffer)
        {
            Medium.WriteLine(buffer);
        }

        public override void WriteLine(int value)
        {
            Medium.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            Medium.WriteLine(value);
        }

        public override void WriteLine(Object value)
        {
            Medium.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            Medium.WriteLine(value);
        }

        public override void WriteLine(String s)
        {
            Medium.WriteLine(s);
        }

        public override void WriteLine(uint value)
        {
            Medium.WriteLine(value);
        }

        public override void WriteLine(String format, Object arg0)
        {
            Medium.WriteLine(format, arg0);
        }

        public override void WriteLine(String format, params Object[] arg)
        {
            Medium.Write(format, arg.Select(a => a).ToArray());
        }

        public override void WriteLine(String format, Object arg0, Object arg1)
        {
            Medium.WriteLine(format, arg0, arg1);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            Medium.WriteLine(buffer, index, count);
        }
    }
}