using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using XenoGears.Traits.Disposable;

namespace XenoGears.Logging
{
    // note. for now I don't care about log4net and similar stuff

    [DebuggerNonUserCode]
    public static class Log
    {
        private static TextWriter _out = Console.Out;
        public static TextWriter Out
        {
            get { return _out; } 
            set { _out = value ?? new StringWriter(new StringBuilder()); }
        }

        public static IDisposable SetOut(StringBuilder new_out) { return SetOut(new StringWriter(new_out)); }
        public static IDisposable SetOut(TextWriter new_out)
        {
            var old_out = Out;
            Out = new_out;
            return new DisposableAction(() => Out = old_out);
        }

        public static void Write(Object o)
        {
            Out.Write(o);
        }

        public static void Write(String message)
        {
            Out.Write(message);
        }

        public static void Write(String message, params Object[] args)
        {
            Out.Write(String.Format(message, args));
        }

        public static void WriteLine(Object o)
        {
            Out.WriteLine(o);
        }

        public static void WriteLine(String message)
        {
            Write(message);
            Write(Environment.NewLine);
        }

        public static void WriteLine(String message, params Object[] args)
        {
            Write(message, args);
            Write(Environment.NewLine);
        }

        public static void WriteLine()
        {
            WriteLine(String.Empty);
        }

        public static void Trace(Object o)
        {
            System.Diagnostics.Trace.Write(o);
        }

        [Conditional("TRACE")]
        public static void Trace(String message)
        {
            System.Diagnostics.Trace.Write(message);
        }

        [Conditional("TRACE")]
        public static void Trace(String message, params Object[] args)
        {
            System.Diagnostics.Trace.Write(String.Format(message, args));
        }

        [Conditional("TRACE")]
        public static void TraceLine(Object o)
        {
            System.Diagnostics.Trace.WriteLine(o);
        }

        [Conditional("TRACE")]
        public static void TraceLine(String message)
        {
            Trace(message);
            Trace(Environment.NewLine);
        }

        [Conditional("TRACE")]
        public static void TraceLine(String message, params Object[] args)
        {
            Trace(message, args);
            Trace(Environment.NewLine);
        }

        [Conditional("TRACE")]
        public static void TraceLine()
        {
            TraceLine(String.Empty);
        }
    }
}
