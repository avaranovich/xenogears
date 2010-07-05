using System;
using System.Diagnostics;
using System.IO;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public static class Printer
    {
        public static String ToTrace(this Object o)
        {
            if (o == null) return "<null>";
            if (o is String) return (o as String).ToTrace();
            if (o is FileInfo) return (o as FileInfo).ToTrace();
            if (o is DirectoryInfo) return (o as DirectoryInfo).ToTrace();
            return o.ToString();
        }

        public static String ToTrace(this String s)
        {
            if (s == null) return "<null>";
            if (s == String.Empty) return "<empty>";
            return s;
        }

        public static String ToTrace(this FileInfo fi)
        {
            if (fi == null) return "<null>";
            return "file: " + fi.FullName.ToTrace();
        }

        public static String ToTrace(this DirectoryInfo di)
        {
            if (di == null) return "<null>";
            return "dir: " + di.FullName.ToTrace();
        }
    }
}