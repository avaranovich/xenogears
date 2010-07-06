using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using XenoGears.Assertions;

namespace XenoGears.Playground.Framework
{
    [DebuggerNonUserCode]
    public class Eavesdropper : TextWriter
    {
        public TextWriter Channel { get; private set; }
        public TextWriter Sink { get; private set; }

        public Eavesdropper(TextWriter channel, StringBuilder sink)
            : this(channel, new StringWriter(sink))
        {
        }

        public Eavesdropper(TextWriter channel, TextWriter sink)
            : base(CultureInfo.InvariantCulture)
        {
            Channel = channel.AssertNotNull();
            Sink = sink.AssertNotNull();
        }

        public override Encoding Encoding
        {
            get { return Channel.Encoding; }
        }

        public override void Close()
        {
            Channel.Close();
        }

        public override void Flush()
        {
            Channel.Flush();
        }

        public override void Write(bool value)
        {
            Sink.Write(value);
            Channel.Write(value);
        }

        public override void Write(char value)
        {
            Sink.Write(value);
            Channel.Write(value);
        }

        public override void Write(char[] buffer)
        {
            Sink.Write(buffer);
            Channel.Write(buffer);
        }

        public override void Write(double value)
        {
            Sink.Write(value);
            Channel.Write(value);
        }

        public override void Write(int value)
        {
            Sink.Write(value);
            Channel.Write(value);
        }

        public override void Write(long value)
        {
            Sink.Write(value);
            Channel.Write(value);
        }

        public override void Write(Object value)
        {
            Sink.Write(value);
            Channel.Write(value);
        }

        public override void Write(float value)
        {
            Sink.Write(value);
            Channel.Write(value);
        }

        public override void Write(String s)
        {
            Sink.Write(s);
            Channel.Write(s);
        }

        public override void Write(String format, Object arg0)
        {
            Sink.Write(format, arg0);
            Channel.Write(format, arg0);
        }

        public override void Write(String format, params Object[] arg)
        {
            Sink.Write(format, arg);
            Channel.Write(format, arg);
        }

        public override void Write(String format, Object arg0, Object arg1)
        {
            Sink.Write(format, arg0, arg1);
            Channel.Write(format, arg0, arg1);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Sink.Write(buffer, index, count);
            Channel.Write(buffer, index, count);
        }

        public override void WriteLine()
        {
            Sink.WriteLine();
            Channel.WriteLine();
        }

        public override void WriteLine(bool value)
        {
            Sink.WriteLine(value);
            Channel.WriteLine(value);
        }

        public override void WriteLine(char value)
        {
            Sink.WriteLine(value);
            Channel.WriteLine(value);
        }

        public override void WriteLine(double value)
        {
            Sink.WriteLine(value);
            Channel.WriteLine(value);
        }

        public override void WriteLine(char[] buffer)
        {
            Sink.WriteLine(buffer);
            Channel.WriteLine(buffer);
        }

        public override void WriteLine(int value)
        {
            Sink.WriteLine(value);
            Channel.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            Sink.WriteLine(value);
            Channel.WriteLine(value);
        }

        public override void WriteLine(Object value)
        {
            Sink.WriteLine(value);
            Channel.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            Sink.WriteLine(value);
            Channel.WriteLine(value);
        }

        public override void WriteLine(String s)
        {
            Sink.WriteLine(s);
            Channel.WriteLine(s);
        }

        public override void WriteLine(uint value)
        {
            Sink.WriteLine(value);
            Channel.WriteLine(value);
        }

        public override void WriteLine(String format, Object arg0)
        {
            Sink.WriteLine(format, arg0);
            Channel.WriteLine(format, arg0);
        }

        public override void WriteLine(String format, params Object[] arg)
        {
            Sink.WriteLine(format, arg);
            Channel.WriteLine(format, arg);
        }

        public override void WriteLine(String format, Object arg0, Object arg1)
        {
            Sink.WriteLine(format, arg0, arg1);
            Channel.WriteLine(format, arg0, arg1);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            Sink.WriteLine(buffer, index, count);
            Channel.WriteLine(buffer, index, count);
        }
    }
}
