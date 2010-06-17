using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Reflection.Shortcuts
{
    [DebuggerNonUserCode]
    public class MA
    {
        public const MethodAttributes NonVirtual = MethodAttributes.HideBySig;
        public const MethodAttributes Virtual = MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.HideBySig;
        public const MethodAttributes Abstract = Virtual | MethodAttributes.Abstract;
        public const MethodAttributes Override = MethodAttributes.Virtual | MethodAttributes.HideBySig;
        public const MethodAttributes SealedOverride = Override | MethodAttributes.Final;

        public const MethodAttributes Private = NonVirtual | MethodAttributes.Private;
        public const MethodAttributes PrivateProp = NonVirtual | MethodAttributes.SpecialName | MethodAttributes.Private;
        public const MethodAttributes PrivateIndexer = PrivateProp;
        public const MethodAttributes PrivateCtor = NonVirtual | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Private;
        public const MethodAttributes PrivateStatic = NonVirtual | MethodAttributes.Static | MethodAttributes.Private;
        public const MethodAttributes PrivateStaticProp = NonVirtual | MethodAttributes.SpecialName | MethodAttributes.Static | MethodAttributes.Private;

        public const MethodAttributes Protected = Virtual | MethodAttributes.Family;
        public const MethodAttributes ProtectedProp = Virtual | MethodAttributes.SpecialName | MethodAttributes.Family;
        public const MethodAttributes ProtectedIndexer = ProtectedProp;
        public const MethodAttributes ProtectedCtor = NonVirtual | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Family;
        public const MethodAttributes ProtectedStatic = NonVirtual | MethodAttributes.Static | MethodAttributes.Family;
        public const MethodAttributes ProtectedStaticProp = NonVirtual | MethodAttributes.SpecialName | MethodAttributes.Static | MethodAttributes.Family;

        public const MethodAttributes Public = Virtual | MethodAttributes.Public;
        public const MethodAttributes PublicProp = Virtual | MethodAttributes.SpecialName | MethodAttributes.Public;
        public const MethodAttributes PublicIndexer = PublicProp;
        public const MethodAttributes PublicCtor = NonVirtual | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Public;
        public const MethodAttributes PublicStatic = NonVirtual | MethodAttributes.Static | MethodAttributes.Public;
        public const MethodAttributes PublicStaticProp = NonVirtual | MethodAttributes.SpecialName | MethodAttributes.Static | MethodAttributes.Public;
        public const MethodAttributes Operator = PublicStaticProp;
        public const MethodAttributes Cast = PublicStaticProp;
    }
}