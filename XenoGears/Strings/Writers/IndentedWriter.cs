using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace XenoGears.Strings.Writers
{
    [DebuggerNonUserCode]
    public class IndentedWriter : BaseWriterWrapper
    {
        public int Indent { get; set; }
        public String TabString { get; set; }
        private bool TabsPending { get; set; }

        public IndentedWriter(StringBuilder buf)
            : this(new StringWriter(buf))
        {
        }

        public IndentedWriter(StringBuilder buf, String tabString)
            : this(new StringWriter(buf), tabString)
        {
        }

        public IndentedWriter(TextWriter writer) 
            : this(writer, "    ")
        {
        }

        public IndentedWriter(TextWriter writer, String tabString) 
            : base(writer)
        {
            Indent = 0;
            TabString = tabString;
            TabsPending = true;
        }

        protected override void BeforeWrite(object o, Type t)
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

        protected override void AfterWriteLine()
        {
            TabsPending = true;
        }

        #region NoTabs versions of TextWriter API

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