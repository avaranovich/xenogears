using System;
using System.Diagnostics;

namespace XenoGears.CommandLine.Exceptions
{
    [DebuggerNonUserCode]
    public class ConfigException : Exception
    {
        public ConfigException()
        {
        }

        public ConfigException(String message)
            : base(message)
        {
        }

        public ConfigException(String format, params String[] args)
            : base(String.Format(format, args))
        {
        }
    }
}