using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace XenoGears.Logging.Media
{
    [DebuggerNonUserCode]
    public class ConsoleMedium : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Console.Out.Encoding; }
        }

        public override void Close()
        {
            Console.Out.Close();
        }

        public override void Flush()
        {
            Console.Out.Flush();
        }

        public override void Write(bool value)
        {
            Console.Out.Write(value);
        }

        public override void Write(char value)
        {
            Console.Out.Write(value);
        }

        public override void Write(char[] buffer)
        {
            Console.Out.Write(buffer);
        }

        public override void Write(double value)
        {
            Console.Out.Write(value);
        }

        public override void Write(int value)
        {
            Console.Out.Write(value);
        }

        public override void Write(long value)
        {
            Console.Out.Write(value);
        }

        public override void Write(Object value)
        {
            Console.Out.Write(value);
        }

        public override void Write(float value)
        {
            Console.Out.Write(value);
        }

        public override void Write(String s)
        {
            Console.Out.Write(s);
        }

        public override void Write(String format, Object arg0)
        {
            Console.Out.Write(String.Format(format, arg0));
        }

        public override void Write(String format, params Object[] arg)
        {
            Console.Out.Write(String.Format(format, arg.Select(a => a).ToArray()));
        }

        public override void Write(String format, Object arg0, Object arg1)
        {
            Console.Out.Write(String.Format(format, arg0, arg1));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Console.Out.Write(new String(buffer, index, count));
        }

        public override void WriteLine()
        {
            Console.Out.WriteLine(String.Empty);
        }

        public override void WriteLine(bool value)
        {
            Console.Out.WriteLine(value);
        }

        public override void WriteLine(char value)
        {
            Console.Out.WriteLine(value);
        }

        public override void WriteLine(double value)
        {
            Console.Out.WriteLine(value);
        }

        public override void WriteLine(char[] buffer)
        {
            Console.Out.WriteLine(buffer);
        }

        public override void WriteLine(int value)
        {
            Console.Out.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            Console.Out.WriteLine(value);
        }

        public override void WriteLine(Object value)
        {
            Console.Out.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            Console.Out.WriteLine(value);
        }

        public override void WriteLine(String s)
        {
            Console.Out.WriteLine(s);
        }

        public override void WriteLine(uint value)
        {
            Console.Out.WriteLine(value);
        }

        public override void WriteLine(String format, Object arg0)
        {
            Console.Out.WriteLine(String.Format(format, arg0));
        }

        public override void WriteLine(String format, params Object[] arg)
        {
            Console.Out.WriteLine(String.Format(format, arg));
        }

        public override void WriteLine(String format, Object arg0, Object arg1)
        {
            Console.Out.WriteLine(String.Format(format, arg0, arg1));
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            Console.Out.WriteLine(new String(buffer, index, count));
        }
    }
}