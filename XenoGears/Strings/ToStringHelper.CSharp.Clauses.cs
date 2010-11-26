using System;
using System.Collections.Generic;
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
    public static partial class ToStringHelper
    {
        public static String GetCSharpTypeNameQualifier(this Type t, ToCSharpOptions opt)
        {
            if (t.IsGenericParameter)
            {
                return String.Empty;
            }
            else
            {
                switch (opt.NameQualifiers)
                {
                    case NameQualifiers.None:
                        return String.Empty;
                    case NameQualifiers.Namespace:
                        return t.Namespace + ".";
                    case NameQualifiers.GlobalAndNamespace:
                        return "global::" + t.Namespace + ".";
                    default:
                        throw AssertionHelper.Fail();
                }
            }
        }

        public static String GetCSharpVisibilityQualifier(this MemberInfo mi)
        {
            var vis = mi.Visibility();
            switch (vis)
            {
                case Visibility.Public:
                    return "public";
                case Visibility.FamilyOrAssembly:
                    return "protected internal";
                case Visibility.Family:
                    return "protected";
                case Visibility.Assembly:
                    return "internal";
                case Visibility.FamilyAndAssembly:
                    throw AssertionHelper.Fail();
                case Visibility.Private:
                    return "private";
                default:
                    throw AssertionHelper.Fail();
            }
        }

        public static String GetCSharpAttributesClause(this ICustomAttributeProvider cap, ToCSharpOptions opt)
        {
            if (cap.Attrs().Count() == 0)
            {
                return String.Empty;
            }
            else
            {
                var buffer = new StringBuilder();
                buffer.Append("[");

                var prefix = String.Empty;
                if (cap is ParameterInfo)
                {
                    /*IsRetVal ain't work */
                    var mb = ((ParameterInfo)cap).Member;
                    if (mb is MethodInfo && ((MethodInfo)(mb)).ReturnParameter == cap)
                    {
                        prefix = "return: ";
                    }
                }

                // todo. implement this using CustomAttributeData
                // that will preserve info about ctor and setters
                var attrs = cap.GetCustomAttributes(false);
                attrs.ForEach((attr, i) =>
                {
                    buffer.Append(prefix);
                    buffer.Append(attr.GetType().GetCSharpRef(opt));
                    if (i != attrs.Count() - 1) buffer.Append(", ");
                });

                buffer.Append("]");
                return buffer.ToString();
            }
        }

        public static String GetCSharpTypeArgsClause(this Type t, ToCSharpOptions opt)
        {
            return GetCSharpTypeArgsClause(t.XGetGenericArguments(), opt);
        }

        public static String GetCSharpTypeArgsClause(this MethodBase mb, ToCSharpOptions opt)
        {
            return GetCSharpTypeArgsClause(mb.XGetGenericArguments(), opt);
        }

        public static String GetCSharpTypeArgsClause(this Type[] targs, ToCSharpOptions opt)
        {
            if (targs.Count() == 0)
            {
                return String.Empty;
            }
            else
            {
                var buffer = new StringBuilder();
                buffer.Append("<");

                targs.ForEach((targ, i) =>
                {
                    if (opt.EmitAttributes && targ.IsGenericParameter && targ.Attrs().IsNotEmpty())
                        buffer.Append(targ.GetCSharpAttributesClause(opt)).Append(" ");
                    buffer.Append(targ.GetCSharpRef(opt));

                    if (i != targs.Count() - 1) buffer.Append(", ");
                });

                buffer.Append(">");
                return buffer.ToString();
            }
        }

        public static String GetCSharpTypeConstraintsClause(this Type t, ToCSharpOptions opt)
        {
            var gargs = t.Flatten(t1 => t1.XGetGenericArguments()).Where(garg => garg.IsGenericParameter);
            var clauses = gargs.Select(garg => garg.GetCSharpTypeConstraintsClause_Impl(opt));
            clauses = clauses.Where(clause => clause.IsNotEmpty());
            return opt.EmitTypeArgsConstraints && clauses.IsNotEmpty() ?
                (String.Empty.MkArray().Concat(clauses).StringJoin(Environment.NewLine)) : String.Empty;
        }

        public static String GetCSharpTypeConstraintsClause(this MethodBase mb, ToCSharpOptions opt)
        {
            var gargs = mb.XGetGenericArguments().Where(garg => garg.IsGenericParameter);
            var clauses = gargs.Select(garg => garg.GetCSharpTypeConstraintsClause_Impl(opt));
            clauses = clauses.Where(clause => clause.IsNotEmpty());
            return opt.EmitTypeArgsConstraints && clauses.IsNotEmpty() ? 
                (String.Empty.MkArray().Concat(clauses).StringJoin(Environment.NewLine)) : String.Empty;
        }

        private static String GetCSharpTypeConstraintsClause_Impl(this Type t, ToCSharpOptions opt)
        {
            t.IsGenericParameter.AssertTrue();
            var mods = new List<String>();

            if ((t.GenericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) == GenericParameterAttributes.ReferenceTypeConstraint)
                mods.Add("class");
            if ((t.GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) == GenericParameterAttributes.NotNullableValueTypeConstraint)
                mods.Add("struct");
            if ((t.GenericParameterAttributes & GenericParameterAttributes.DefaultConstructorConstraint) == GenericParameterAttributes.DefaultConstructorConstraint)
                mods.Add("new()");
            var inheritance = t.GetGenericParameterConstraints();
            inheritance.ForEach(t_c => mods.Add(t_c.GetCSharpRef(opt)));

            if (mods.IsEmpty())
            {
                return String.Empty;
            }
            else
            {
                var buffer = new StringBuilder();
                buffer.Append("    ");
                buffer.Append("where ");
                buffer.Append(t.GetCSharpRef(opt));
                buffer.Append(" : ");
                buffer.Append(mods.StringJoin());
                return buffer.ToString();
            }
        }
    }
}
