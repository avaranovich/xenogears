using System;
using System.Diagnostics;

namespace XenoGears.CommandLine.Exceptions
{
    [DebuggerNonUserCode]
    public class ConfigException : Exception
    {
        public ConfigException()
            : this(null as Exception)
        {
        }

        public ConfigException(String message)
            : this(null as Exception, message)
        {
        }

        public ConfigException(String format, params String[] args)
            : this(null as Exception, format, args)
        {
        }

        public ConfigException(Exception innerException)
            : base(null, innerException)
        {
        }

        public ConfigException(Exception innerException, String message)
            : base(message, innerException)
        {
        }

        public ConfigException(Exception innerException, String format, params String[] args)
            : base(String.Format(format, args), innerException)
        {
        }
    }
}