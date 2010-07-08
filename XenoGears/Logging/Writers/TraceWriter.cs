using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using XenoGears.Logging.Formatters;

namespace XenoGears.Logging.Writers
{
    [DebuggerNonUserCode]
    internal class TraceWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8;}
        }

        public override void Close()
        {
            Trace.Close();
        }

        public override void Flush()
        {
            Trace.Flush();
        }

        public override void Write(bool value)
        {
            Trace.Write(value.ToLog());
        }

        public override void Write(char value)
        {
            Trace.Write(value.ToLog());
        }

        public override void Write(char[] buffer)
        {
            Trace.Write(buffer.ToLog());
        }

        public override void Write(double value)
        {
            Trace.Write(value.ToLog());
        }

        public override void Write(int value)
        {
            Trace.Write(value.ToLog());
        }

        public override void Write(long value)
        {
            Trace.Write(value.ToLog());
        }

        public override void Write(Object value)
        {
            Trace.Write(value.ToLog());
        }

        public override void Write(float value)
        {
            Trace.Write(value.ToLog());
        }

        public override void Write(String s)
        {
            Trace.Write(s.ToLog());
        }

        public override void Write(String format, Object arg0)
        {
            // todo. this will mess up custom format specifiers!
            Trace.Write(String.Format(format, arg0.ToLog()));
        }

        public override void Write(String format, params Object[] arg)
        {
            // todo. this will mess up custom format specifiers!
            Trace.Write(String.Format(format, arg.Select(a => a.ToLog()).ToArray()));
        }

        public override void Write(String format, Object arg0, Object arg1)
        {
            // todo. this will mess up custom format specifiers!
            Trace.Write(String.Format(format, arg0.ToLog(), arg1.ToLog()));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Trace.Write(new String(buffer, index, count));
        }

        public override void WriteLine()
        {
            Trace.WriteLine(String.Empty);
        }

        public override void WriteLine(bool value)
        {
            Trace.WriteLine(value.ToLog());
        }

        public override void WriteLine(char value)
        {
            Trace.WriteLine(value.ToLog());
        }

        public override void WriteLine(double value)
        {
            Trace.WriteLine(value.ToLog());
        }

        public override void WriteLine(char[] buffer)
        {
            Trace.WriteLine(buffer.ToLog());
        }

        public override void WriteLine(int value)
        {
            Trace.WriteLine(value.ToLog());
        }

        public override void WriteLine(long value)
        {
            Trace.WriteLine(value.ToLog());
        }

        public override void WriteLine(Object value)
        {
            Trace.WriteLine(value.ToLog());
        }

        public override void WriteLine(float value)
        {
            Trace.WriteLine(value.ToLog());
        }

        public override void WriteLine(String s)
        {
            Trace.WriteLine(s.ToLog());
        }

        public override void WriteLine(uint value)
        {
            Trace.WriteLine(value.ToLog());
        }

        public override void WriteLine(String format, Object arg0)
        {
            // todo. this will mess up custom format specifiers!
            Trace.WriteLine(String.Format(format, arg0.ToLog()));
        }

        public override void WriteLine(String format, params Object[] arg)
        {
            // todo. this will mess up custom format specifiers!
            Trace.WriteLine(String.Format(format, arg.ToLog()));
        }

        public override void WriteLine(String format, Object arg0, Object arg1)
        {
            // todo. this will mess up custom format specifiers!
            Trace.WriteLine(String.Format(format, arg0.ToLog(), arg1.ToLog()));
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            Trace.WriteLine(new String(buffer, index, count));
        }
    }
}