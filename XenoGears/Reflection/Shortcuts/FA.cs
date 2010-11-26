using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Reflection.Shortcuts
{
    [DebuggerNonUserCode]
    public class FA
    {
        public const FieldAttributes Public = FieldAttributes.Public;
        public const FieldAttributes Private = FieldAttributes.Private;

        public const FieldAttributes PublicStatic = Public | FieldAttributes.Static;
        public const FieldAttributes PrivateStatic = Private | FieldAttributes.Static;
    }
}