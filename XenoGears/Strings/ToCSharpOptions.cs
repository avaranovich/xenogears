using System.Diagnostics;

namespace XenoGears.Strings
{
    [DebuggerNonUserCode]
    public class ToCSharpOptions
    {
        public bool EmitAttributes { get; set; }
        public bool EmitVisibilityQualifier { get; set; }
        public bool EmitStaticQualifier { get; set; }
        public bool EmitDeclaringType { get; set; }
        public NameQualifiers NameQualifiers { get; set; }
        public bool EmitCtorNameAsClassName { get; set; }
        public bool EmitTypeArgsCount { get; set; }
        public bool EmitTypeArgs { get; set; }
        public bool EmitTypeArgsConstraints { get; set; }
        public bool EmitSemicolon { get; set; }

        public ToCSharpOptions Clone()
        {
            return (ToCSharpOptions)MemberwiseClone();
        }

        public static ToCSharpOptions Terse { get { return new ToCSharpOptions {
            EmitAttributes = false,
            EmitVisibilityQualifier = false,
            EmitStaticQualifier = true,
            EmitDeclaringType = false,
            NameQualifiers = NameQualifiers.None,
            EmitCtorNameAsClassName = false,
            EmitTypeArgsCount = true,
            EmitTypeArgs = false,
            EmitTypeArgsConstraints = false,
            EmitSemicolon = false,
        }; } }
        public static ToCSharpOptions Informative { get { return new ToCSharpOptions {
            EmitAttributes = false,
            EmitVisibilityQualifier = false,
            EmitStaticQualifier = true,
            EmitDeclaringType = false,
            NameQualifiers = NameQualifiers.None,
            EmitCtorNameAsClassName = false,
            EmitTypeArgsCount = false,
            EmitTypeArgs = true,
            EmitTypeArgsConstraints = true,
            EmitSemicolon = false,
        }; } }
        public static ToCSharpOptions InformativeWithNamespaces { get { return new ToCSharpOptions {
            EmitAttributes = false,
            EmitVisibilityQualifier = false,
            EmitStaticQualifier = true,
            EmitDeclaringType = false,
            NameQualifiers = NameQualifiers.Namespace,
            EmitCtorNameAsClassName = false,
            EmitTypeArgsCount = false,
            EmitTypeArgs = true,
            EmitTypeArgsConstraints = true,
            EmitSemicolon = false,
        }; } }
        public static ToCSharpOptions InformativeWithDeclaringType { get { return new ToCSharpOptions {
            EmitAttributes = false,
            EmitVisibilityQualifier = false,
            EmitStaticQualifier = true,
            EmitDeclaringType = true,
            NameQualifiers = NameQualifiers.None,
            EmitCtorNameAsClassName = false,
            EmitTypeArgsCount = false,
            EmitTypeArgs = true,
            EmitTypeArgsConstraints = true,
            EmitSemicolon = false,
        }; } }
        public static ToCSharpOptions ForCodegen { get { return new ToCSharpOptions {
            EmitAttributes = true,
            EmitVisibilityQualifier = true,
            EmitStaticQualifier = true,
            EmitDeclaringType = false,
            NameQualifiers = NameQualifiers.GlobalAndNamespace,
            EmitCtorNameAsClassName = true,
            EmitTypeArgsCount = false,
            EmitTypeArgs = true,
            EmitTypeArgsConstraints = true,
            EmitSemicolon = true,
        }; } }
        public static ToCSharpOptions Verbose { get { return new ToCSharpOptions {
            EmitAttributes = true,
            EmitVisibilityQualifier = true,
            EmitStaticQualifier = true,
            EmitDeclaringType = true,
            NameQualifiers = NameQualifiers.Namespace,
            EmitCtorNameAsClassName = false,
            EmitTypeArgsCount = true,
            EmitTypeArgs = true,
            EmitTypeArgsConstraints = true,
            EmitSemicolon = false,
        }; } }
    }
}
