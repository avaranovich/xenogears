using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Reflection.Shortcuts
{
    [DebuggerNonUserCode]
    public class TA
    {
        public const TypeAttributes Public = TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoClass | TypeAttributes.AnsiClass;
        public const TypeAttributes PublicAbstract = TA.Public | TypeAttributes.Abstract;
        public const TypeAttributes PublicStatic = TA.Public | TypeAttributes.Abstract | TypeAttributes.Sealed;
        public const TypeAttributes PublicInterface = TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Interface | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoClass | TypeAttributes.AnsiClass;
    }
}