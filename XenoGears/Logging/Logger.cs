using System;
using System.Diagnostics;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public class Logger
    {
        public String Name { get; set; }

        public bool Enabled { get; set; }
        public void Enable() { Enabled = true; }
        public void Disable() { Enabled = false; }

        public Logger(String name)
        {
            Name = name;

#if TRACE
            Enabled = true;
#endif
        }

        public void Write(Object o)
        {
            if (Enabled) Log.Write(Name, o);
        }

        public void Write(String message)
        {
            if (Enabled) Log.Write(Name, message);
        }

        public void Write(String message, params Object[] args)
        {
            if (Enabled) Log.Write(Name, String.Format(message, args));
        }

        public void WriteLine(Object o)
        {
            Write(o);
            WriteLine();
        }

        public void WriteLine(String message)
        {
            Write(message);
            WriteLine();
        }

        public void WriteLine(String message, params Object[] args)
        {
            Write(message, args);
            WriteLine();
        }

        public void WriteLine()
        {
            Write(Environment.NewLine);
        }
    }
}