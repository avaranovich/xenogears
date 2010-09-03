using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using System.Threading;
using XenoGears.Reflection.Emit2;

namespace XenoGears.Reflection.Anonymous
{
    // Source code shamelessly copy/pasted from http://www.codeplex.com/interlinq
    // todo: rewrite this stuff by my own hands
    [DebuggerNonUserCode]
    internal class AnonymousTypeBuilder
    {
        private static int _nextInstanceId = 0;
        private readonly int _instanceId = Interlocked.Increment(ref _nextInstanceId);
        private String ClassName { get { return "XenoGearsAnonymousType<" + _instanceId.ToString("X") + ">"; } }
        private String[] GenericClassParameterNames
        {
            get
            {
                var list = new List<String>();
                for (var i = 0; i < PropertyNames.Length; i++)
                {
                    var propName = PropertyNames[i];
                    list.Add(String.Format("T_{0}", propName));
                }

                return list.ToArray();
            }
        }
        private String[] PropertyNames { get; set; }
        private String[] FieldNames { get { return 
            PropertyNames.Select(pname => "_" + pname.ToLowerInvariant()[0] + pname.Substring(1)).ToArray(); }}

        private Type[] PropertyTypes { get; set; }

        public AnonymousTypeBuilder(string[] propNames, Type[] propTypes)
        {
            PropertyNames = propNames;
            PropertyTypes = propTypes;
        }

        public Type ToAnonymousType()
        {
            return AnonymousTypesCache.Get(PropertyNames, PropertyTypes, () =>
            {
                var key = typeof(AnonymousTypeBuilder).Assembly.ReadKey("XenoGears.XenoGears.snk");
                var unit = Codegen.Units["XenoGears.AnonymousTypes", key];
                return GenerateAnonymousType(unit.Module);
            });
        }

        private void AddDebuggerHiddenAttribute(ConstructorBuilder constructor)
        {
            var type = typeof(DebuggerHiddenAttribute);
            var customBuilder = new CustomAttributeBuilder(type.GetConstructor(new Type[0]), new object[0]);
            constructor.SetCustomAttribute(customBuilder);
        }

        private void AddDebuggerHiddenAttribute(MethodBuilder method)
        {
            var type = typeof(DebuggerHiddenAttribute);
            var customBuilder = new CustomAttributeBuilder(type.GetConstructor(new Type[0]), new object[0]);
            method.SetCustomAttribute(customBuilder);
        }

        private Type GenerateAnonymousType(ModuleBuilder dynamicTypeModule)
        {
            var dynamicType = dynamicTypeModule.DefineType(ClassName, TypeAttributes.BeforeFieldInit | TypeAttributes.Sealed);
            var builderArray = dynamicType.DefineGenericParameters(GenericClassParameterNames);
            var fields = new List<FieldBuilder>();
            for (var i = 0; i < FieldNames.Length; i++)
            {
                var item = dynamicType.DefineField(FieldNames[i], builderArray[i], FieldAttributes.InitOnly | FieldAttributes.Private);
                var type = typeof(DebuggerBrowsableAttribute);
                var customBuilder = new CustomAttributeBuilder(type.GetConstructor(new Type[] { typeof(DebuggerBrowsableState) }), new object[] { DebuggerBrowsableState.Never });
                item.SetCustomAttribute(customBuilder);
                fields.Add(item);
            }
            var list2 = new List<PropertyBuilder>();
            for (var j = 0; j < PropertyNames.Length; j++)
            {
                var builder4 = GenerateProperty(dynamicType, PropertyNames[j], fields[j]);
//                var type = typeof(JsonPropertyAttribute);
//                var customBuilder2 = new CustomAttributeBuilder(type.GetConstructor(new Type[0]), new object[0]);
//                builder4.SetCustomAttribute(customBuilder2);
                list2.Add(builder4);
            }
            GenerateClassAttributes(dynamicType, PropertyNames);
            GenerateConstructor(dynamicType, PropertyNames, fields);
            GenerateEqualsMethod(dynamicType, fields.ToArray());
            GenerateGetHashCodeMethod(dynamicType, fields.ToArray());
            GenerateToStringMethod(dynamicType, PropertyNames, fields.ToArray());
            return dynamicType.CreateType().MakeGenericType(PropertyTypes);
        }

        private void GenerateClassAttributes(TypeBuilder dynamicType, string[] propertyNames)
        {
            var type = typeof(CompilerGeneratedAttribute);
            var customBuilder = new CustomAttributeBuilder(type.GetConstructor(new Type[0]), new object[0]);
            dynamicType.SetCustomAttribute(customBuilder);
            var type2 = typeof(DebuggerDisplayAttribute);
            var builder2 = new StringBuilder(@"\{ ");
            var flag = true;
            foreach (var str in propertyNames)
            {
                builder2.AppendFormat("{0}{1} = ", flag ? "" : ", ", str);
                builder2.Append("{");
                builder2.Append(str);
                builder2.Append("}");
                flag = false;
            }
            builder2.Append(" }");
            var property = type2.GetProperty("Type");
            var builder3 = new CustomAttributeBuilder(type2.GetConstructor(new Type[] { typeof(string) }), new object[] { builder2.ToString() }, new PropertyInfo[] { property }, new object[] { "<Anonymous Type>" });
            dynamicType.SetCustomAttribute(builder3);
        }

        private void GenerateConstructor(TypeBuilder dynamicType, string[] propertyNames, List<FieldBuilder> fields)
        {
            var constructor = dynamicType.DefineConstructor(MethodAttributes.RTSpecialName | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Public, CallingConventions.Standard, (from f in fields select f.FieldType).ToArray<Type>());
            var iLGenerator = constructor.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));
            for (var i = 0; i < propertyNames.Length; i++)
            {
                var strParamName = propertyNames[i];
                var field = fields[i];
                var builder3 = constructor.DefineParameter(i + 1, ParameterAttributes.None, strParamName);
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Ldarg, builder3.Position);
                iLGenerator.Emit(OpCodes.Stfld, field);
            }
            iLGenerator.Emit(OpCodes.Ret);
            AddDebuggerHiddenAttribute(constructor);
        }

        private void GenerateEqualsMethod(TypeBuilder dynamicType, FieldBuilder[] fields)
        {
            var methodInfoBody = dynamicType.DefineMethod("Equals", MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Public, CallingConventions.Standard, typeof(bool), new Type[] { typeof(object) });
            methodInfoBody.DefineParameter(0, ParameterAttributes.None, "value");
            var iLGenerator = methodInfoBody.GetILGenerator();
            var local = iLGenerator.DeclareLocal(dynamicType);
            var label = iLGenerator.DefineLabel();
            var label2 = iLGenerator.DefineLabel();
            iLGenerator.Emit(OpCodes.Ldarg_1);
            iLGenerator.Emit(OpCodes.Isinst, dynamicType);
            iLGenerator.Emit(OpCodes.Stloc, local);
            iLGenerator.Emit(OpCodes.Ldloc, local);
            var type = typeof(EqualityComparer<>);
            foreach (var builder3 in fields)
            {
                var type2 = type.MakeGenericType(new Type[] { builder3.FieldType });
                var getMethod = type.GetProperty("Default", BindingFlags.Public | BindingFlags.Static).GetGetMethod();
                var method = TypeBuilder.GetMethod(type2, getMethod);
                iLGenerator.Emit(OpCodes.Brfalse_S, label);
                iLGenerator.EmitCall(OpCodes.Call, method, null);
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Ldfld, builder3);
                iLGenerator.Emit(OpCodes.Ldloc, local);
                iLGenerator.Emit(OpCodes.Ldfld, builder3);
                var type3 = type.GetGenericArguments()[0];
                var info3 = type.GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { type3, type3 }, null);
                var methodInfo = TypeBuilder.GetMethod(type2, info3);
                iLGenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
            }
            iLGenerator.Emit(OpCodes.Br_S, label2);
            iLGenerator.MarkLabel(label);
            iLGenerator.Emit(OpCodes.Ldc_I4_0);
            iLGenerator.MarkLabel(label2);
            iLGenerator.Emit(OpCodes.Ret);
            dynamicType.DefineMethodOverride(methodInfoBody, typeof(object).GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance));
            AddDebuggerHiddenAttribute(methodInfoBody);
        }

        private void GenerateGetHashCodeMethod(TypeBuilder dynamicType, FieldBuilder[] fields)
        {
            var methodInfoBody = dynamicType.DefineMethod("GetHashCode", MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Public, CallingConventions.Standard, typeof(int), new Type[0]);
            var iLGenerator = methodInfoBody.GetILGenerator();
            var type = typeof(EqualityComparer<>);
            var local = iLGenerator.DeclareLocal(typeof(int));
            iLGenerator.Emit(OpCodes.Ldc_I4, -747105811);
            iLGenerator.Emit(OpCodes.Stloc, local);
            foreach (var builder3 in fields)
            {
                iLGenerator.Emit(OpCodes.Ldc_I4, -1521134295);
                iLGenerator.Emit(OpCodes.Ldloc, local);
                iLGenerator.Emit(OpCodes.Mul);
                var type2 = type.MakeGenericType(new Type[] { builder3.FieldType });
                var getMethod = type.GetProperty("Default", BindingFlags.Public | BindingFlags.Static).GetGetMethod();
                var method = TypeBuilder.GetMethod(type2, getMethod);
                iLGenerator.EmitCall(OpCodes.Call, method, null);
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Ldfld, builder3);
                var type3 = type.GetGenericArguments()[0];
                var info3 = type.GetMethod("GetHashCode", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { type3 }, null);
                var methodInfo = TypeBuilder.GetMethod(type2, info3);
                iLGenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
                iLGenerator.Emit(OpCodes.Add);
                iLGenerator.Emit(OpCodes.Stloc, local);
            }
            iLGenerator.Emit(OpCodes.Ldloc, local);
            iLGenerator.Emit(OpCodes.Ret);
            dynamicType.DefineMethodOverride(methodInfoBody, typeof(object).GetMethod("GetHashCode", BindingFlags.Public | BindingFlags.Instance));
            AddDebuggerHiddenAttribute(methodInfoBody);
        }

        private MethodBuilder GenerateGetMethod(TypeBuilder dynamicType, PropertyBuilder property, FieldBuilder field)
        {
            var builder = dynamicType.DefineMethod(string.Format("get_{0}", property.Name), MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Public);
            builder.SetReturnType(field.FieldType);
            var iLGenerator = builder.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Ldfld, field);
            iLGenerator.Emit(OpCodes.Ret);
            return builder;
        }

        private PropertyBuilder GenerateProperty(TypeBuilder dynamicType, string propertyName, FieldBuilder field)
        {
            var property = dynamicType.DefineProperty(propertyName, PropertyAttributes.None, field.FieldType, null);
            var mdBuilder = GenerateGetMethod(dynamicType, property, field);
            property.SetGetMethod(mdBuilder);
            return property;
        }

        private void GenerateToStringMethod(TypeBuilder dynamicType, string[] propertyNames, FieldBuilder[] fields)
        {
            var methodInfoBody = dynamicType.DefineMethod("ToString", MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Public, CallingConventions.Standard, typeof(string), new Type[0]);
            var iLGenerator = methodInfoBody.GetILGenerator();
            var local = iLGenerator.DeclareLocal(typeof(StringBuilder));
            var methodInfo = typeof(StringBuilder).GetMethod("Append", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(object) }, null);
            var info2 = typeof(StringBuilder).GetMethod("Append", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(string) }, null);
            var info3 = typeof(object).GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
            iLGenerator.Emit(OpCodes.Newobj, typeof(StringBuilder).GetConstructor(new Type[0]));
            iLGenerator.Emit(OpCodes.Stloc, local);
            iLGenerator.Emit(OpCodes.Ldloc, local);
            iLGenerator.Emit(OpCodes.Ldstr, "{ ");
            iLGenerator.EmitCall(OpCodes.Callvirt, info2, null);
            iLGenerator.Emit(OpCodes.Pop);
            var flag = true;
            for (var i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                iLGenerator.Emit(OpCodes.Ldloc, local);
                iLGenerator.Emit(OpCodes.Ldstr, (flag ? "" : ", ") + propertyNames[i] + " = ");
                iLGenerator.EmitCall(OpCodes.Callvirt, info2, null);
                iLGenerator.Emit(OpCodes.Pop);
                iLGenerator.Emit(OpCodes.Ldloc, local);
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Ldfld, field);
                iLGenerator.Emit(OpCodes.Box, field.FieldType);
                iLGenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
                iLGenerator.Emit(OpCodes.Pop);
                flag = false;
            }
            iLGenerator.Emit(OpCodes.Ldloc, local);
            iLGenerator.Emit(OpCodes.Ldstr, " }");
            iLGenerator.EmitCall(OpCodes.Callvirt, info2, null);
            iLGenerator.Emit(OpCodes.Pop);
            iLGenerator.Emit(OpCodes.Ldloc, local);
            iLGenerator.EmitCall(OpCodes.Callvirt, info3, null);
            iLGenerator.Emit(OpCodes.Ret);
            dynamicType.DefineMethodOverride(methodInfoBody, typeof(object).GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance));
            AddDebuggerHiddenAttribute(methodInfoBody);
        }
    }
}
