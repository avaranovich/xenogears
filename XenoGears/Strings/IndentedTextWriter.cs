using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Strings
{
    [DebuggerNonUserCode]
    public class IndentedTextWriter : TextWriter
    {
        public TextWriter InnerWriter { get; private set; }
        public override Encoding Encoding { get { return InnerWriter.Encoding; } }

        public override String NewLine { get { return InnerWriter.NewLine; } set { InnerWriter.NewLine = value; } }
        public String TabString { get; set; }

        public int Indent { get; set; }
        private bool TabsPending { get; set; }

        public IndentedTextWriter(TextWriter writer) 
            : this(writer, "    ")
        {
        }

        public IndentedTextWriter(TextWriter writer, String tabString) 
            : base(CultureInfo.InvariantCulture)
        {
            InnerWriter = writer;
            TabString = tabString;
            Indent = 0;
            TabsPending = true;
        }

        private void OutputTabs()
        {
            if (TabsPending)
            {
                for (var i = 0; i < Indent; i++)
                {
                    InnerWriter.Write(TabString);
                }

                TabsPending = false;
            }
        }

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

        private void ProxiedWrite(char c)
        {
            InnerWriter.Write(c);

            push(c);
            TabsPending |= is_eoln();
        }

        #region Boilerplate implementation of TextWriter API

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
            OutputTabs();
            Write(value ? "True" : "False");
        }

        public override void Write(char value)
        {
            OutputTabs();
            ProxiedWrite(value);
        }

        public override void Write(char[] buffer)
        {
            OutputTabs();
            if (buffer != null)
            {
                Write(buffer, 0, buffer.Length);
            }
        }

        public override void Write(double value)
        {
            OutputTabs();
            Write(value.ToString(FormatProvider));
        }

        public override void Write(int value)
        {
            OutputTabs();
            Write(value.ToString(FormatProvider));
        }

        public override void Write(long value)
        {
            OutputTabs();
            Write(value.ToString(FormatProvider));
        }

        public override void Write(Object value)
        {
            OutputTabs();
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
        }

        public override void Write(float value)
        {
            OutputTabs();
            Write(value.ToString(FormatProvider));
        }

        public override void Write(String s)
        {
            OutputTabs();
            if (s != null)
            {
                Write(s.ToCharArray());
            }
        }

        public override void Write(String format, Object arg0)
        {
            OutputTabs();
            Write(String.Format(FormatProvider, format, new []{arg0}));
        }

        public override void Write(String format, params Object[] arg)
        {
            OutputTabs();
            Write(String.Format(FormatProvider, format, arg));
        }

        public override void Write(String format, Object arg0, Object arg1)
        {
            OutputTabs();
            Write(String.Format(FormatProvider, format, new []{arg0, arg1}));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            OutputTabs();

            buffer.AssertNotNull();
            (index >= 0).AssertTrue();
            (count >= 0).AssertTrue();
            (buffer.Length >= index + count).AssertTrue();

            for (var i = 0; i < count; i++)
            {
                Write(buffer[index + i]);
            }
        }

        public void WriteNoTabs(bool value)
        {
            TabsPending = false;
            Write(value);
        }

        public void WriteNoTabs(char value)
        {
            TabsPending = false;
            Write(value);
        }

        public void WriteNoTabs(double value)
        {
            TabsPending = false;
            Write(value);
        }

        public void WriteNoTabs(char[] buffer)
        {
            TabsPending = false;
            Write(buffer);
        }

        public void WriteNoTabs(int value)
        {
            TabsPending = false;
            Write(value);
        }

        public void WriteNoTabs(long value)
        {
            TabsPending = false;
            Write(value);
        }

        public void WriteNoTabs(Object value)
        {
            TabsPending = false;
            Write(value);
        }

        public void WriteNoTabs(float value)
        {
            TabsPending = false;
            Write(value);
        }

        public void WriteNoTabs(String s)
        {
            TabsPending = false;
            Write(s);
        }

        public void WriteNoTabs(uint value)
        {
            TabsPending = false;
            Write(value);
        }

        public void WriteNoTabs(String format, Object arg0)
        {
            TabsPending = false;
            Write(arg0);
        }

        public void WriteNoTabs(String format, params Object[] arg)
        {
            TabsPending = false;
            Write(format, arg);
        }

        public void WriteNoTabs(String format, Object arg0, Object arg1)
        {
            TabsPending = false;
            Write(format, arg0, arg1);
        }

        public void WriteNoTabs(char[] buffer, int index, int count)
        {
            TabsPending = false;
            Write(buffer, index, count);
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

        public void WriteLineNoTabs()
        {
            TabsPending = false;
            WriteLine();
        }

        public void WriteLineNoTabs(bool value)
        {
            TabsPending = false;
            WriteLine(value);
        }

        public void WriteLineNoTabs(char value)
        {
            TabsPending = false;
            WriteLine(value);
        }

        public void WriteLineNoTabs(double value)
        {
            TabsPending = false;
            WriteLine(value);
        }

        public void WriteLineNoTabs(char[] buffer)
        {
            TabsPending = false;
            WriteLine(buffer);
        }

        public void WriteLineNoTabs(int value)
        {
            TabsPending = false;
            WriteLine(value);
        }

        public void WriteLineNoTabs(long value)
        {
            TabsPending = false;
            WriteLine(value);
        }

        public void WriteLineNoTabs(Object value)
        {
            TabsPending = false;
            WriteLine(value);
        }

        public void WriteLineNoTabs(float value)
        {
            TabsPending = false;
            WriteLine(value);
        }

        public void WriteLineNoTabs(String s)
        {
            TabsPending = false;
            WriteLine(s);
        }

        public void WriteLineNoTabs(uint value)
        {
            TabsPending = false;
            WriteLine(value);
        }

        public void WriteLineNoTabs(String format, Object arg0)
        {
            TabsPending = false;
            WriteLine(arg0);
        }

        public void WriteLineNoTabs(String format, params Object[] arg)
        {
            TabsPending = false;
            WriteLine(format, arg);
        }

        public void WriteLineNoTabs(String format, Object arg0, Object arg1)
        {
            TabsPending = false;
            WriteLine(format, arg0, arg1);
        }

        public void WriteLineNoTabs(char[] buffer, int index, int count)
        {
            TabsPending = false;
            WriteLine(buffer, index, count);
        }

        #endregion
    }
}
