using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Reflection.Typed;
using System.Linq;

namespace XenoGears.Reflection.Emit.Hackarounds
{
    [DebuggerNonUserCode]
    internal static class FixMethodBuilderSignature_WithHackaround
    {
        private static Type CreateType_WithHackaround(this TypeBuilder t)
        {
            var asmData = t.Module.Assembly.GetSlot<_>("m_assemblyData").Value.Value;
            if (asmData.GetSlot<bool>("m_isSynchronized").Value)
            {
                lock (asmData)
                {
                    return t.CreateTypeNoLock_WithHackaround();
                }
            }
            else
            {
                return t.CreateTypeNoLock_WithHackaround();
            }
        }

        private static Type CreateTypeNoLock_WithHackaround(this TypeBuilder t)
        {
            var m_runtimeType = (((Object)t).GetSlot<_>("m_runtimeType").Value ?? new _(null)).Value;
            Action<Object> set_m_runtimeType = value => typeof(TypeBuilder).GetField("m_runtimeType", BF.All).SetValue(t, value);
            var ThrowIfGeneric = ((Object)t).GetAction("ThrowIfGeneric");
            var ThrowIfCreated = ((Object)t).GetAction("ThrowIfCreated");
            var m_typeInterfaces = ((Object)t).GetSlot<Type[]>("m_typeInterfaces").Value ?? new Type[0];
            var m_module = ((Object)t).GetSlot<ModuleBuilder>("m_module").Value;
            var GetTypeTokenInternal = m_module.GetFunc<Type, TypeToken>("GetTypeTokenInternal");
            var m_typeParent = ((Object)t).GetSlot<Type>("m_typeParent").Value;
            var m_declMeth = ((Object)t).GetSlot<MethodBuilder>("m_declMeth").Value;
            var m_DeclaringType = ((Object)t).GetSlot<TypeBuilder>("m_DeclaringType").Value;
            var m_DeclaringType_m_runtimeType = m_DeclaringType == null ? null : (((Object)m_DeclaringType).GetSlot<_>("m_runtimeType").Value ?? new _(null)).Value;
            var m_tdType = ((Object)t).GetSlot<TypeToken>("m_tdType").Value;
            Action<TypeToken> set_m_tdType = value => { ((Object)t).GetSlot<TypeToken>("m_tdType").Value = value; };
            var m_genParamPos = ((Object)t).GetSlot<int>("m_genParamPos").Value;
            var m_genParamAttributes = ((Object)t).GetSlot<GenericParameterAttributes>("m_genParamAttributes").Value;
            var m_strName = ((Object)t).GetSlot<String>("m_strName").Value;
            var m_ca = ((Object)t).GetSlot<ArrayList>("m_ca").Value;
            var m_DeclaringType_m_tdType = m_DeclaringType == null ? TypeToken.Empty : ((Object)m_DeclaringType).GetSlot<TypeToken>("m_tdType").Value;
            var MetadataTokenInternal = ((Object)t).GetSlot<int>("MetadataTokenInternal").Value;
            Action<bool> set_m_hasBeenCreated = value => { ((Object)t).GetSlot<bool>("m_hasBeenCreated").Value = value; };
            var m_inst = ((Object)t).GetSlot<GenericTypeParameterBuilder[]>("m_inst").Value;
            var m_isHiddenGlobalType = ((Object)t).GetSlot<bool>("m_isHiddenGlobalType").Value;
            var m_constructorCount = ((Object)t).GetSlot<int>("m_constructorCount").Value;
            var m_iAttr = ((Object)t).GetSlot<TypeAttributes>("m_iAttr").Value;
            var m_listMethods = ((Object)t).GetSlot<ArrayList>("m_listMethods").Value;

            if (t.IsCreated())
            {
                return (Type)m_runtimeType;
            }
            else
            {
                ThrowIfGeneric();
                ThrowIfCreated();

                var interfaceTokens = new int[m_typeInterfaces.Length];
                for (int i = 0; i < m_typeInterfaces.Length; i++)
                {
                    interfaceTokens[i] = GetTypeTokenInternal(m_typeInterfaces[i]).Token;
                }

                var tkParent = 0;
                if (m_typeParent != null)
                {
                    tkParent = GetTypeTokenInternal(m_typeParent).Token;
                }

                if (t.IsGenericParameter)
                {
                    if (m_typeParent != null)
                    {
                        interfaceTokens = new int[m_typeInterfaces.Length + 1];
                        interfaceTokens[interfaceTokens.Length - 1] = tkParent;
                    }

                    for (var k = 0; k < m_typeInterfaces.Length; k++)
                    {
                        interfaceTokens[k] = GetTypeTokenInternal(m_typeInterfaces[k]).Token;
                    }

                    int num4 = (m_declMeth == null) ? m_DeclaringType_m_tdType.Token : m_declMeth.GetToken().Token;
//                    private int InternalDefineGenParam(string name, int tkParent, int position, int attributes, int[] interfaceTokens, Module module, int tkTypeDef)
                    var InternalDefineGenParam = typeof(TypeBuilder).GetFunc<String, int, int, int, int[], Module, int, int>("InternalDefineGenParam");
                    Func<int, TypeToken> typeTokenCtor = tt => (TypeToken)typeof(TypeToken).GetConstructors(BF.All).Single(c => c.GetParameters().Count() == 1).Invoke(new object[]{tt});
                    set_m_tdType(typeTokenCtor(InternalDefineGenParam(m_strName, num4, m_genParamPos, (int)m_genParamAttributes, interfaceTokens, m_module, 0)));
                    if (m_ca != null)
                    {
                        foreach (var attr in m_ca)
                        {
                            var Bake = attr.GetAction<ModuleBuilder, int>("Bake");
                            Bake(m_module, MetadataTokenInternal);
                        }
                    }
                    set_m_hasBeenCreated(true);
                    return t;
                }
                else
                {
                    if (((m_tdType.Token & 0xffffff) != 0) && ((tkParent & 0xffffff) != 0))
                    {
                        var InternalSetParentType = typeof(TypeBuilder).GetAction<int, int, Module>("InternalSetParentType");
                        InternalSetParentType(m_tdType.Token, tkParent, m_module);
                    }
                    if (m_inst != null)
                    {
                        GenericTypeParameterBuilder[] inst = m_inst;
                        for (int m = 0; m < inst.Length; m++)
                        {
                            Type type = inst[m];
                            if (type is GenericTypeParameterBuilder)
                            {
                                var gtpb = ((GenericTypeParameterBuilder)type);
                                gtpb.GetSlot<TypeBuilder>("m_type").Value.CreateType();
                            }
                        }
                    }
                    if (((!m_isHiddenGlobalType && (m_constructorCount == 0)) && 
                        (((m_iAttr & TypeAttributes.Interface) == TypeAttributes.AutoLayout) && !t.IsValueType)) && 
                        ((m_iAttr & (TypeAttributes.Sealed | TypeAttributes.Abstract)) != (TypeAttributes.Sealed | TypeAttributes.Abstract)))
                    {
                        t.DefineDefaultConstructor(MethodAttributes.Public);
                    }
                    int count = m_listMethods.Count;
                    for (int j = 0; j < count; j++)
                    {
                        MethodBuilder builder = (MethodBuilder)m_listMethods[j];
                        var m_ilGenerator = builder.GetSlot<ILGenerator>("m_ilGenerator").Value;
                        var m_canBeRuntimeImpl = builder.GetSlot<bool>("m_canBeRuntimeImpl").Value;
                        var GetBody = builder.GetFunc<byte[]>("GetBody");
                        var GetTokenFixups = builder.GetFunc<int[]>("GetTokenFixups");
                        var GetRVAFixups = builder.GetFunc<int[]>("GetRVAFixups");
                        var GetMaxStackSize = m_ilGenerator.GetFunc<int>("GetMaxStackSize");
                        var CreateMethodBodyHelper = builder.GetAction<ILGenerator>("CreateMethodBodyHelper");

                        if (builder.IsGenericMethodDefinition)
                        {
                            builder.GetToken();
                        }
                        MethodAttributes attributes = builder.Attributes;
                        if (((builder.GetMethodImplementationFlags() & (MethodImplAttributes.PreserveSig | MethodImplAttributes.Unmanaged | MethodImplAttributes.CodeTypeMask)) == MethodImplAttributes.IL) && ((attributes & MethodAttributes.PinvokeImpl) == MethodAttributes.ReuseSlot))
                        {
                            int maxStackSize;
                            int num8;

                            var sh = builder.GetFunc<SignatureHelper>("GetLocalsSignature")();
                            var ppp = new Object[1];
                            var InternalGetSignature = sh.GetType().GetMethod("InternalGetSignature", BF.All);
                            var signature = (byte[])InternalGetSignature.Invoke(sh, ppp);
                            num8 = (int)ppp[0];

                            if (((attributes & MethodAttributes.Abstract) != MethodAttributes.ReuseSlot) && ((m_iAttr & TypeAttributes.Abstract) == TypeAttributes.AutoLayout))
                            {
                                throw new InvalidOperationException("InvalidOperation_BadTypeAttributesNotAbstract");
                            }
                            byte[] body = GetBody();
                            if ((attributes & MethodAttributes.Abstract) != MethodAttributes.ReuseSlot)
                            {
                                if (body != null)
                                {
                                    throw new InvalidOperationException("InvalidOperation_BadMethodBody");
                                }
                            }
                            else if ((body == null) || (body.Length == 0))
                            {
                                if (m_ilGenerator != null)
                                {
                                    CreateMethodBodyHelper(builder.GetILGenerator());
                                }
                                body = GetBody();
                                if (((body == null) || (body.Length == 0)) && !m_canBeRuntimeImpl)
                                {
                                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "InvalidOperation_BadEmptyMethodBody", new object[] { builder.Name }));
                                }
                            }
                            if (m_ilGenerator != null)
                            {
                                maxStackSize = GetMaxStackSize();
                            }
                            else
                            {
                                maxStackSize = 0x10;
                            }
                            var exceptionInstances = (Object[])builder.GetFunc<_>("GetExceptionInstances")().Value;
                            int[] tokenFixups = GetTokenFixups();
                            int[] rVAFixups = GetRVAFixups();
                            Object destinationArray = null;
                            int[] numArray5 = null;
                            int[] numArray6 = null;
                            if (exceptionInstances != null)
                            {
                                var __ExceptionInstance = Type.GetType("System.Reflection.Emit.__ExceptionInstance");
                                destinationArray = Activator.CreateInstance(__ExceptionInstance.MakeArrayType(), exceptionInstances.Length);
                                Array.Copy(exceptionInstances, (Array)destinationArray, exceptionInstances.Length);
                            }
                            if (tokenFixups != null)
                            {
                                numArray5 = new int[tokenFixups.Length];
                                Array.Copy(tokenFixups, numArray5, tokenFixups.Length);
                            }
                            if (rVAFixups != null)
                            {
                                numArray6 = new int[rVAFixups.Length];
                                Array.Copy(rVAFixups, numArray6, rVAFixups.Length);
                            }
//                            internal static void InternalSetMethodIL(int methodHandle, bool isInitLocals, byte[] body, byte[] LocalSig, int sigLength, int maxStackSize, int numExceptions, __ExceptionInstance[] exceptions, int[] tokenFixups, int[] rvaFixups, Module module)
                            var InternalSetMethodIL = typeof(TypeBuilder).GetAction<int, bool, byte[], byte[], int, int, int, _, int[], int[], Module>("InternalSetMethodIL");
                            InternalSetMethodIL(builder.GetToken().Token, builder.InitLocals, body, signature, num8, maxStackSize, builder.GetFunc<int>("GetNumberOfExceptions")(), new _(destinationArray), numArray5, numArray6, m_module);
                            var m_assemblyData = t.Assembly.GetSlot<_>("m_assemblyData").Value.Value;
                            if (m_assemblyData.GetSlot<AssemblyBuilderAccess>("m_access").Value == 
                                AssemblyBuilderAccess.Run)
                            {
                                builder.GetAction("ReleaseBakedStructures")();
                            }
                        }
                    }
                    set_m_hasBeenCreated(true);
                    Type type2 = ((Object)t).GetFunc<int, Module, Type>("TermCreateClass")(m_tdType.Token, m_module);
                    if (m_isHiddenGlobalType)
                    {
                        return null;
                    }
                    set_m_runtimeType(type2);
                    if ((m_DeclaringType != null) && (m_DeclaringType_m_runtimeType != null))
                    {
                        ((Object)m_DeclaringType_m_runtimeType).GetAction("InvalidateCachedNestedType")();
                    }
                    return type2;
                }
            }
        }
    }
}
