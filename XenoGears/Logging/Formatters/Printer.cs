using System;
using System.Diagnostics;
using System.IO;
using XenoGears.Strings;

namespace XenoGears.Logging.Formatters
{
    [DebuggerNonUserCode]
    public static class Printer
    {
        public static String ToLog(this Object o)
        {
            if (o == null) return "<null>";
            if (o is String) return (o as String).ToLog();
            if (o is FileInfo) return (o as FileInfo).ToLog();
            if (o is DirectoryInfo) return (o as DirectoryInfo).ToLog();
            return o.ToString();
        }

        public static String ToLog(this String s)
        {
            if (s == null) return "<null>";
            if (s == String.Empty) return "<empty>";
            return s;
        }

        public static String ToLog(this FileInfo fi)
        {
            if (fi == null) return "<null>";
            return "file: " + fi.FullName.Uncapitalize().ToLog();
        }

        public static String ToLog(this DirectoryInfo di)
        {
            if (di == null) return "<null>";
            return "dir: " + di.FullName.Uncapitalize().ToLog();
        }
    }
}