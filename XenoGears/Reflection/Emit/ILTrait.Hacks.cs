using System;
using System.Diagnostics.SymbolStore;
using System.Reflection.Emit;
using XenoGears.Assertions;
using XenoGears.Reflection.Simple;
using XenoGears.Reflection.Typed;

namespace XenoGears.Reflection.Emit
{
    public static partial class ILTrait
    {
        public static Type CreateType(this TypeBuilder t, bool emitDebugInfo)
        {
            var module = t.Get("m_module").AssertCast<ModuleBuilder>();
            var moduleIsCapableOfEmittingDebugInfo = module.GetSymWriter() != null;
            emitDebugInfo.AssertImplies(moduleIsCapableOfEmittingDebugInfo);

            if (emitDebugInfo)
            {
                return t.CreateType();
            }
            else
            {
                var slot = module.GetSlot<ISymbolWriter>("m_iSymWriter");
                var bak = slot.Value;

                try
                {
                    slot.Value = null;
                    return t.CreateType();
                }
                finally 
                {
                    slot.Value = bak;
                }
            }
        }
    }
}
