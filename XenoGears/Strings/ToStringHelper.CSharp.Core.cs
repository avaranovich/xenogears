using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection;
using XenoGears.Reflection.Attributes;
using XenoGears.Reflection.Generics;

namespace XenoGears.Strings
{
    // todo. verify that decls ain't contain closed gargs
    // todo. c# doesn't provide syntax for member refs

    [DebuggerNonUserCode]
    public static partial class ToStringHelper
    {
        public static String GetCSharpRef(this MemberInfo mi, ToCSharpOptions opt)
        {
            if (mi is Type) return ((Type)mi).GetCSharpRef(opt);
            else if (mi is FieldInfo) return ((FieldInfo)mi).GetCSharpRef(opt);
            else if (mi is MethodBase) return ((MethodBase)mi).GetCSharpRef(opt);
            else if (mi is PropertyInfo) return ((PropertyInfo)mi).GetCSharpRef(opt);
            else throw AssertionHelper.Fail();
        }

        public static String GetCSharpDecl(this MemberInfo mi, ToCSharpOptions opt)
        {
            if (mi is Type) return ((Type)mi).GetCSharpDecl(opt);
            else if (mi is FieldInfo) return ((FieldInfo)mi).GetCSharpDecl(opt);
            else if (mi is MethodBase) return ((MethodBase)mi).GetCSharpDecl(opt);
            else if (mi is PropertyInfo) return ((PropertyInfo)mi).GetCSharpDecl(opt);
            else throw AssertionHelper.Fail();
        }

        public static String GetCSharpRef(this Type t, ToCSharpOptions opt)
        {
            // since this is only ref, we can ignore loads of stuff
            opt = (opt ?? ToCSharpOptions.Terse).Clone();
            opt.EmitAttributes = false;
            opt.EmitVisibilityQualifier = false;
            opt.EmitStaticQualifier = false;
            opt.EmitAttributes = false;
            opt.EmitDeclaringType = true;
            opt.EmitSemicolon = false;

            return t.GetCSharpDecl(opt);
        }

        public static String GetCSharpDecl(this Type t, ToCSharpOptions opt)
        {
            if (t == typeof(byte)) return "byte";
            if (t == typeof(sbyte)) return "sbyte";
            if (t == typeof(short)) return "short";
            if (t == typeof(ushort)) return "ushort";
            if (t == typeof(int)) return "int";
            if (t == typeof(uint)) return "uint";
            if (t == typeof(long)) return "long";
            if (t == typeof(ulong)) return "ulong";
            if (t == typeof(float)) return "float";
            if (t == typeof(double)) return "double";
            if (t == typeof(decimal)) return "decimal";
            if (t == typeof(bool)) return "bool";
            if (t == typeof(char)) return "char";
            if (t == typeof(void)) return "void";

            if (t.IsNullable())
            {
                return t.UndecorateNullable().GetCSharpRef(opt) + "?";
            }
            else
            {
                opt = opt ?? ToCSharpOptions.Terse;
                var buffer = new StringBuilder();

                if (opt.EmitAttributes && t.Attrs().IsNotEmpty())
                    buffer.Append(t.GetCSharpAttributesClause(opt)).Append(" ");
                if (opt.EmitStaticQualifier && t.IsStatic()) buffer.Append("static ");
                buffer.Append(t.GetCSharpTypeNameQualifier(opt));
                buffer.Append(t.Name.Slice(0, t.Name.IndexOf("`") == -1 ? t.Name.Length : t.Name.IndexOf("`")));
                if (opt.EmitTypeArgsCount && t.XGetGenericArguments().IsNotEmpty())
                    buffer.Append("`").Append(t.XGetGenericArguments().Count());
                if (opt.EmitTypeArgs && t.XGetGenericArguments().IsNotEmpty()) 
                    buffer.Append(t.GetCSharpTypeArgsClause(opt));

                if (opt.EmitSemicolon) buffer.Append(";");
                return buffer.ToString();
            }
        }

        public static String GetCSharpRef(this FieldInfo f, ToCSharpOptions opt)
        {
            // since this is only ref, we can ignore loads of stuff
            opt = (opt ?? ToCSharpOptions.Terse).Clone();
            opt.EmitAttributes = false;
            opt.EmitVisibilityQualifier = false;
            opt.EmitStaticQualifier = false;
            opt.EmitAttributes = false;
            opt.EmitDeclaringType = true;
            opt.EmitSemicolon = false;

            return f.GetCSharpDecl(opt);
        }

        public static String GetCSharpDecl(this FieldInfo f, ToCSharpOptions opt)
        {
            opt = opt ?? ToCSharpOptions.Terse;
            var buffer = new StringBuilder();

            if (opt.EmitAttributes && f.Attrs().IsNotEmpty()) 
                buffer.Append(f.GetCSharpAttributesClause(opt)).Append(" ");
            if (opt.EmitVisibilityQualifier) buffer.Append(f.GetCSharpVisibilityQualifier()).Append(" ");
            if (opt.EmitStaticQualifier && f.IsStatic) buffer.Append("static ");
            buffer.Append(f.FieldType.GetCSharpRef(opt)).Append(" ");
            if (opt.EmitDeclaringType) buffer.Append(f.DeclaringType.GetCSharpRef(ToCSharpOptions.Terse) + "::");
            buffer.Append(f.Name);

            if (opt.EmitSemicolon) buffer.Append(";");
            return buffer.ToString();
        }

        public static String GetCSharpRef(this MethodBase m, ToCSharpOptions opt)
        {
            // since this is only ref, we can ignore loads of stuff
            opt = (opt ?? ToCSharpOptions.Terse).Clone();
            opt.EmitAttributes = false;
            opt.EmitVisibilityQualifier = false;
            opt.EmitStaticQualifier = false;
            opt.EmitAttributes = false;
            opt.EmitDeclaringType = true;
            opt.EmitSemicolon = false;

            return m.GetCSharpDecl(opt);
        }

        public static String GetCSharpDecl(this MethodBase m, ToCSharpOptions opt)
        {
            opt = opt ?? ToCSharpOptions.Terse;
            var buffer = new StringBuilder();

            if (opt.EmitAttributes && m.Attrs().IsNotEmpty()) 
                buffer.Append(m.GetCSharpAttributesClause(opt)).Append(" ");
            if (opt.EmitAttributes && m is MethodInfo && (((MethodInfo)m).ReturnParameter).Attrs().IsNotEmpty())
                buffer.Append(((MethodInfo)m).ReturnParameter.GetCSharpAttributesClause(opt)).Append(" ");
            if (opt.EmitVisibilityQualifier) buffer.Append(m.GetCSharpVisibilityQualifier()).Append(" ");
            if (opt.EmitStaticQualifier && m.IsStatic) buffer.Append("static ");
            if (!(m is ConstructorInfo)) buffer.Append(m.Ret().GetCSharpRef(opt)).Append(" ");
            if (opt.EmitDeclaringType) buffer.Append(m.DeclaringType.GetCSharpRef(ToCSharpOptions.Terse) + "::");
            if (!(m is ConstructorInfo)) buffer.Append(m.Name.Slice(0, m.Name.IndexOf("`") == -1 ? m.Name.Length : m.Name.IndexOf("`")));
            else if (opt.EmitCtorNameAsClassName) buffer.Append(m.DeclaringType.Name.Slice(0, m.DeclaringType.Name.IndexOf("`") == -1 ? m.DeclaringType.Name.Length : m.DeclaringType.Name.IndexOf("`")));
            else buffer.Append(m.Name.Slice(0, m.Name.IndexOf("`") == -1 ? m.Name.Length : m.Name.IndexOf("`")));
            if (opt.EmitTypeArgs) buffer.Append(m.GetCSharpTypeArgsClause(opt));

            buffer.Append("(");
            m.GetParameters().ForEach((pi, i) =>
            {
                if (opt.EmitAttributes && pi.Attrs().IsNotEmpty())
                    buffer.Append(pi.GetCSharpAttributesClause(opt)).Append(" ");

                var mod = "";
                if (pi.ParameterType.IsByRef && pi.IsOut) mod = "out";
                if (pi.ParameterType.IsByRef && !pi.IsOut) mod = "ref";
                if (mod.IsNotEmpty()) buffer.Append(mod).Append(" ");

                var t_par = pi.ParameterType.IsByRef ? pi.ParameterType.GetElementType() : pi.ParameterType;
                buffer.Append(t_par.GetCSharpRef(opt)).Append(" ");
                buffer.Append(pi.Name);

                if (i != m.GetParameters().Count() - 1) buffer.Append(", ");
            });
            buffer.Append(")");

            if (opt.EmitTypeArgsConstraints) buffer.Append(m.GetCSharpTypeConstraintsClause(opt));

            if (opt.EmitSemicolon) buffer.Append(";");
            return buffer.ToString();
        }

        public static String GetCSharpRef(this PropertyInfo p, ToCSharpOptions opt)
        {
            // since this is only ref, we can ignore loads of stuff
            opt = (opt ?? ToCSharpOptions.Terse).Clone();
            opt.EmitAttributes = false;
            opt.EmitVisibilityQualifier = false;
            opt.EmitStaticQualifier = false;
            opt.EmitAttributes = false;
            opt.EmitDeclaringType = true;
            opt.EmitSemicolon = false;

            return p.GetCSharpDecl(opt);
        }

        public static String GetCSharpDecl(this PropertyInfo p, ToCSharpOptions opt)
        {
            opt = opt ?? ToCSharpOptions.Terse;
            var buffer = new StringBuilder();

            if (opt.EmitAttributes && p.Attrs().IsNotEmpty())
                buffer.Append(p.GetCSharpAttributesClause(opt)).Append(" ");
            if (opt.EmitVisibilityQualifier) buffer.Append(p.GetCSharpVisibilityQualifier()).Append(" ");
            if (opt.EmitStaticQualifier && p.IsStatic()) buffer.Append("static ");
            buffer.Append(p.PropertyType.GetCSharpRef(opt)).Append(" ");
            if (opt.EmitDeclaringType) buffer.Append(p.DeclaringType.GetCSharpRef(ToCSharpOptions.Terse) + "::");
            buffer.Append(p.Name);

            buffer.Append(" { ");
            if (p.CanRead)
            {
                var getter = p.GetGetMethod(true);
                if (opt.EmitVisibilityQualifier) buffer.Append(getter.GetCSharpVisibilityQualifier()).Append(" ");
                buffer.Append("get; ");
            }
            if (p.CanWrite)
            {
                var setter = p.GetSetMethod(true);
                if (opt.EmitVisibilityQualifier) buffer.Append(setter.GetCSharpVisibilityQualifier()).Append(" ");
                buffer.Append("set; ");
            }
            buffer.Append("}");

            return buffer.ToString();
        }
    }
}
