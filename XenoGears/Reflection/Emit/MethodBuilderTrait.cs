using System.Diagnostics;
using System.Reflection.Emit;

namespace XenoGears.Reflection.Emit
{
    [DebuggerNonUserCode]
    public static class MethodBuilderTrait
    {
        public static void ImplementByDefault(this MethodBuilder source)
        {
            source.il().lddefault(source.ReturnParameter.ParameterType).ret();
        }
    }
}