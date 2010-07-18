using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace XenoGears.Reflection
{
    [DebuggerNonUserCode]
    public static class SpecialNamesHelper
    {
        private static List<UDInfo> _ud_ops = new List<UDInfo>();
        [DebuggerNonUserCode] private class UDInfo { public String OperatorName { get; set; } public String MethodName { get; set; } }

        static SpecialNamesHelper()
        {
            _ud_ops.Add(new UDInfo { OperatorName = "UnaryPlus", MethodName = "op_UnaryPlus" });
            _ud_ops.Add(new UDInfo { OperatorName = "Negate", MethodName = "op_UnaryMinus" });
            _ud_ops.Add(new UDInfo { OperatorName = "Not", MethodName = "op_LogicalNot" });
            _ud_ops.Add(new UDInfo { OperatorName = "Negate", MethodName = "op_OnesComplement" });
            _ud_ops.Add(new UDInfo { OperatorName = "Or", MethodName = "op_BitwiseOr" });
            _ud_ops.Add(new UDInfo { OperatorName = "Xor", MethodName = "op_ExclusiveOr" });
            _ud_ops.Add(new UDInfo { OperatorName = "And", MethodName = "op_BitwiseAnd" });
            _ud_ops.Add(new UDInfo { OperatorName = "Equal", MethodName = "op_Equality" });
            _ud_ops.Add(new UDInfo { OperatorName = "NotEqual", MethodName = "op_Inequality" });
            _ud_ops.Add(new UDInfo { OperatorName = "GreaterThan", MethodName = "op_GreaterThan" });
            _ud_ops.Add(new UDInfo { OperatorName = "LessThan", MethodName = "op_LessThan" });
            _ud_ops.Add(new UDInfo { OperatorName = "GreaterThanOrEqual", MethodName = "op_GreaterThanOrEqual" });
            _ud_ops.Add(new UDInfo { OperatorName = "LessThanOrEqual", MethodName = "op_LessThanOrEqual" });
            _ud_ops.Add(new UDInfo { OperatorName = "RightShift", MethodName = "op_RightShift" });
            _ud_ops.Add(new UDInfo { OperatorName = "LeftShift", MethodName = "op_LeftShift" });
            _ud_ops.Add(new UDInfo { OperatorName = "Add", MethodName = "op_Addition" });
            _ud_ops.Add(new UDInfo { OperatorName = "Subtract", MethodName = "op_Subtraction" });
            _ud_ops.Add(new UDInfo { OperatorName = "Modulo", MethodName = "op_Modulus" });
            _ud_ops.Add(new UDInfo { OperatorName = "Multiply", MethodName = "op_Multiply" });
            _ud_ops.Add(new UDInfo { OperatorName = "Divide", MethodName = "op_Division" });
        }

        public static bool IsUserDefinedOperator(this MethodBase mb)
        {
            if (!mb.IsSpecialName) return false;
            return _ud_ops.Any(info => info.MethodName == mb.Name);
        }

        public static String UserDefinedOperatorType(this MethodBase mb)
        {
            if (!mb.IsSpecialName) return null;
            var info = _ud_ops.SingleOrDefault(info1 => info1.MethodName == mb.Name);
            return info == null ? null : info.OperatorName;
        }

        public static bool IsUserDefinedCast(this MethodBase mb)
        {
            return mb.IsImplicitCast() || mb.IsExplicitCast();
        }

        public static bool IsImplicitCast(this MethodBase mb)
        {
            if (!mb.IsSpecialName) return false;
            return mb.Name == "op_Implicit";
        }

        public static bool IsExplicitCast(this MethodBase mb)
        {
            if (!mb.IsSpecialName) return false;
            return mb.Name == "op_Explicit";
        }

        public static bool IsUserDefinedBool(this MethodBase mb)
        {
            return mb.IsUserDefinedTrue() || mb.IsUserDefinedFalse();
        }

        public static bool IsUserDefinedTrue(this MethodBase mb)
        {
            if (!mb.IsSpecialName) return false;
            return mb.Name == "op_True";
        }

        public static bool IsUserDefinedFalse(this MethodBase mb)
        {
            if (!mb.IsSpecialName) return false;
            return mb.Name == "op_False";
        }
    }
}
