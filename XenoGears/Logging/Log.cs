using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using XenoGears.Logging.Writers;
using XenoGears.Traits.Disposable;

namespace XenoGears.Logging
{
    // note. for now I don't care about log4net and similar stuff

    [DebuggerNonUserCode]
    public static class Log
    {
        private static TextWriter _out = new LowlevelMedium();
        public static TextWriter Out
        {
            get { return _out; } 
            set { _out = value ?? new StringWriter(new StringBuilder()); }
        }

        public static IDisposable OverrideOut(StringBuilder new_out) { return OverrideOut(new StringWriter(new_out)); }
        public static IDisposable OverrideOut(TextWriter new_out)
        {
            var old_out = Out;
            Out = new_out;
            return new DisposableAction(() => Out = old_out);
        }

        public static IDisposable MultiplexOut(StringBuilder sink) { return MultiplexOut(new StringWriter(sink)); }
        public static IDisposable MultiplexOut(TextWriter sink)
        {
            var eavesdropper = new Eavesdropper(Out, sink);
            return OverrideOut(eavesdropper);
        }

        public static void Write(Object o)
        {
            Out.Write(o);
        }

        public static void Write(String source, Object o)
        {
            Write(o);
        }

        public static void Write(String message)
        {
            Out.Write(message);
        }

        public static void Write(String source, String message)
        {
            Write(message);
        }

        public static void Write(String message, params Object[] args)
        {
            Out.Write(String.Format(message, args));
        }

        public static void Write(String source, String message, params Object[] args)
        {
            Write(message, args);
        }

        public static void WriteLine(Object o)
        {
            Write(o);
            WriteLine();
        }

        public static void WriteLine(String source, Object o)
        {
            WriteLine(o);
        }

        public static void WriteLine(String message)
        {
            Write(message);
            WriteLine();
        }

        public static void WriteLine(String source, String message)
        {
            WriteLine(message);
        }

        public static void WriteLine(String message, params Object[] args)
        {
            Write(message, args);
            WriteLine();
        }

        public static void WriteLine(String source, String message, params Object[] args)
        {
            WriteLine(message, args);
        }

        // todo. how do we introduce the WriteLine(eventSource) signature?! =)
        public static void WriteLine()
        {
            Write(Environment.NewLine);
        }
    }
}
