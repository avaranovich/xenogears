using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using XenoGears.Functional;

namespace XenoGears.Streams
{
    [DebuggerNonUserCode]
    public static class ResourceHelper
    {
        public static ReadOnlyCollection<String> Resources(this Assembly asm)
        {
            return asm.GetManifestResourceNames().ToReadOnly();
        }

        public static String ReadText(this Assembly asm, String resource_name)
        {
            if (asm == null) return null;
            if (resource_name == null) return null;

            using (var stream = asm.GetManifestResourceStream(resource_name))
            {
                return stream.DumpToString();
            }
        }

        public static byte[] ReadBinary(this Assembly asm, String resource_name)
        {
            if (asm == null) return null;
            if (resource_name == null) return null;

            using (var stream = asm.GetManifestResourceStream(resource_name))
            {
                return stream.DumpToByteArray();
            }
        }

        public static StrongNameKeyPair ReadKey(this Assembly asm, String resource_name)
        {
            var bytes = asm.ReadBinary(resource_name);
            return bytes == null ? null : new StrongNameKeyPair(bytes);
        }

        public static Stream OpenStream(this Assembly asm, String resource_name)
        {
            if (asm == null) return null;
            if (resource_name == null) return null;

            return asm.GetManifestResourceStream(resource_name);
        }
    }
}