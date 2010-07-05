using System;

namespace XenoGears.CommandLine.Exceptions
{
    public class ConfigException : Exception
    {
        public ConfigException()
        {
        }

        public ConfigException(String message)
            : base(message)
        {
        }

        public ConfigException(String format, String arg0)
            : base(String.Format(format, arg0))
        {
        }

        public ConfigException(String format, String arg0, String arg1)
            : base(String.Format(format, arg0, arg1))
        {
        }

        public ConfigException(String format, String arg0, String arg1, String arg2)
            : base(String.Format(format, arg0, arg1, arg2))
        {
        }
    }
}