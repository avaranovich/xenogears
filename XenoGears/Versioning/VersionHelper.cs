using System;
using System.Diagnostics;
using System.IO;
using XenoGears.Assertions;

namespace XenoGears.Versioning
{
    [DebuggerNonUserCode]
    public static class VersionHelper
    {
        public static Version Version(this FileInfo fi)
        {
            fi.AssertNotNull();
            if (!fi.Exists) throw new FileNotFoundException(null, fi.FullName);

            var fvi = FileVersionInfo.GetVersionInfo(fi.FullName);
            return new Version(fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart, fvi.FilePrivatePart);
        }
    }
}