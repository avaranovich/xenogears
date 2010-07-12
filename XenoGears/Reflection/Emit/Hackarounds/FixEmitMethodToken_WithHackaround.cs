using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using XenoGears.Assertions;
using XenoGears.Reflection.Typed;

namespace XenoGears.Reflection.Emit.Hackarounds
{
    [DebuggerNonUserCode]
    public static class FixEmitMethodToken_WithHackaround
    {
        public static int GetMethodToken_Hackaround(this ILGenerator il, MethodBase method)
        {
            return il.GetMethodToken_Hackaround(method, null);
        }

        public static int GetMethodToken_Hackaround(this ILGenerator il, MethodBase method, Type[] optionalParameterTypes)
        {
            var methodBuilder = il.GetSlot<MethodInfo>("m_methodBuilder").Value.AssertCast<MethodBuilder>();
            var typeBuilder = methodBuilder.GetSlot<TypeBuilder>("m_containingType").Value;
            var module = ((Object)typeBuilder).GetSlot<ModuleBuilder>("m_module").Value;

            if (method.IsGenericMethod)
            {
                var mi = method as MethodInfo;
                if (!method.IsGenericMethodDefinition && (method is MethodInfo))
                {
                    mi = ((MethodInfo)method).GetGenericMethodDefinition();
                }

                var handle = 0x00;
                if (!mi.Module.Equals(module) || 
                    ((mi.DeclaringType != null) && mi.DeclaringType.IsGenericType))
                {
                    handle = module.GetMemberRefToken(mi, optionalParameterTypes);
                }
                else
                {
                    var getMethodTokenInternal = module.GetFunc<MethodInfo, MethodToken>("GetMethodTokenInternal");
                    handle = getMethodTokenInternal(mi).Token;
                }

                var sh = SignatureHelper_WithHackaround.GetMethodSpecSigHelper(module, method.GetGenericArguments());
                int length; var signature = sh.InternalGetSignature(out length);

                var internalDefineMethodSpec = typeof(TypeBuilder).GetFunc<int, byte[], int, Module, int>("InternalDefineMethodSpec"); ;
                return internalDefineMethodSpec(handle, signature, length, module);
            }
            else if (((method.CallingConvention & CallingConventions.VarArgs) == 0x0) && ((method.DeclaringType == null) || !method.DeclaringType.IsGenericType))
            {
                if (method is MethodInfo)
                {
                    var getMethodTokenInternal = module.GetFunc<MethodInfo, MethodToken>("GetMethodTokenInternal");
                    return getMethodTokenInternal(method as MethodInfo).Token;
                }
                else
                {
                    return module.GetConstructorToken(method as ConstructorInfo).Token;
                }
            }
            else
            {
                return module.GetMemberRefToken(method, optionalParameterTypes);
            }
        }

        private static int GetMemberRefToken(this ModuleBuilder module, MethodBase method, Type[] optionalParameterTypes)
        {
            Type[] parameterTypes;
            Type returnType;
            int typeSpecTokenWithBytes;
            int num4;

            int cGenericParameters = 0x0;
            if (method.IsGenericMethod)
            {
                if (!method.IsGenericMethodDefinition)
                {
                    throw new InvalidOperationException();
                }
                cGenericParameters = method.GetGenericArguments().Length;
            }

            if ((optionalParameterTypes != null) && ((method.CallingConvention & CallingConventions.VarArgs) == 0x0))
            {
                throw new InvalidOperationException("InvalidOperation_NotAVarArgCallingConvention");
            }

            var mi = method as MethodInfo;
            if (method.DeclaringType.IsGenericType)
            {
                MethodBase ctor = null;
                if (method.GetType().Name == "MethodOnTypeBuilderInstantiation")
                {
                    ctor = method.GetSlot<MethodBase>("m_method").Value;
                }
                else
                {
                    if (method.GetType().Name == "ConstructorOnTypeBuilderInstantiation")
                    {
                        ctor = method.GetSlot<MethodBase>("m_ctor").Value;
                    }
                    else if ((method is MethodBuilder) || (method is ConstructorBuilder))
                    {
                        ctor = method;
                    }
                    else if (method.IsGenericMethod)
                    {
                        ctor = mi.GetGenericMethodDefinition();
                        var token = ctor.GetSlot<int>("MetadataTokenInternal").Value;
                        ctor = ctor.Module.ResolveMethod(token, ctor.GetGenericArguments(), (ctor.DeclaringType != null) ? ctor.DeclaringType.GetGenericArguments() : null);
                    }
                    else
                    {
                        ctor = method;
                        var token = ctor.GetSlot<int>("MetadataTokenInternal").Value;
                        ctor = method.Module.ResolveMethod(token, null, (ctor.DeclaringType != null) ? ctor.DeclaringType.GetGenericArguments() : null);
                    }
                }

                var getParameterTypes = ctor.GetFunc<Type[]>("GetParameterTypes");
                var getReturnType = ctor.GetFunc<Type>("GetReturnType");
                parameterTypes = getParameterTypes();
                returnType = getReturnType();
            }
            else
            {
                var getParameterTypes = method.GetFunc<Type[]>("GetParameterTypes");
                var getReturnType = method.GetFunc<Type>("GetReturnType");
                parameterTypes = getParameterTypes();
                returnType = getReturnType();
            }

            if (method.DeclaringType.IsGenericType)
            {
                int num3;
                byte[] buffer = SignatureHelper_WithHackaround.GetTypeSigToken(module, method.DeclaringType).InternalGetSignature(out num3);
                var internalGetTypeSpecTokenWithBytes = module.GetFunc<byte[], int, int>("InternalGetTypeSpecTokenWithBytes");
                typeSpecTokenWithBytes = internalGetTypeSpecTokenWithBytes(buffer, num3);
            }
            else if (method.Module.GetSlot<Module>("InternalModule").Value !=
                     module.GetSlot<Module>("InternalModule").Value)
            {
                typeSpecTokenWithBytes = module.GetTypeToken(method.DeclaringType).Token;
            }
            else if (mi != null)
            {
                typeSpecTokenWithBytes = module.GetMethodToken(method as MethodInfo).Token;
            }
            else
            {
                typeSpecTokenWithBytes = module.GetConstructorToken(method as ConstructorInfo).Token;
            }

            var sh = module.GetMemberRefSignature(method.CallingConvention, returnType, parameterTypes, optionalParameterTypes, cGenericParameters);
            var signature = sh.InternalGetSignature(out num4);

            var internalGetMemberRefFromSignature = module.GetFunc<int, String, byte[], int, int>("InternalGetMemberRefFromSignature");
            return internalGetMemberRefFromSignature(typeSpecTokenWithBytes, method.Name, signature, num4);
        }

        private static SignatureHelper_WithHackaround GetMemberRefSignature(this Module module, CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes, int cGenericParameters)
        {
            int length;
            int num2;
            if (parameterTypes == null)
            {
                length = 0x0;
            }
            else
            {
                length = parameterTypes.Length;
            }
            var helper = SignatureHelper_WithHackaround.GetMethodSigHelper(module, call, returnType, cGenericParameters);
            for (num2 = 0x0; num2 < length; num2++)
            {
                helper.AddArgument(parameterTypes[num2]);
            }
            if ((optionalParameterTypes != null) && (optionalParameterTypes.Length != 0x0))
            {
                helper.AddSentinel();
                for (num2 = 0x0; num2 < optionalParameterTypes.Length; num2++)
                {
                    helper.AddArgument(optionalParameterTypes[num2]);
                }
            }
            return helper;
        }
    }
}