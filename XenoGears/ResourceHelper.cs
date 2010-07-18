using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using XenoGears.Functional;

namespace XenoGears
{
    [DebuggerNonUserCode]
    public static class ResourceHelper
    {
        public static ReadOnlyCollection<String> Resources(this Assembly asm)
        {
            return asm.GetManifestResourceNames().ToReadOnly();
        }

        public static String ReadResource(this Assembly asm, String resource_name)
        {
            if (asm == null) return null;
            if (resource_name == null) return null;

            using (var stream = asm.GetManifestResourceStream(resource_name))
            {
                return stream.AsString();
            }
        }

        public static byte[] ReadResourceBytes(this Assembly asm, String resource_name)
        {
            if (asm == null) return null;
            if (resource_name == null) return null;

            using (var stream = asm.GetManifestResourceStream(resource_name))
            {
                return stream.AsByteArray();
            }
        }

        public static Stream ReadResourceStream(this Assembly asm, String resource_name)
        {
            if (asm == null) return null;
            if (resource_name == null) return null;

            return asm.GetManifestResourceStream(resource_name);
        }
    }
}