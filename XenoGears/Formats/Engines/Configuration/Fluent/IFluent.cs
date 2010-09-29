using System;
using System.Collections.Generic;
using System.Reflection;

namespace XenoGears.Formats.Engines.Configuration.Fluent
{
    internal interface IFluent<T>
    {
        T DefaultCtor { get; }
        T NotDefaultCtor { get; }

        T IsPrimitive { get; }
        T IsNotPrimitive { get; }

        T IsObject { get; }
        T OptOutPublic { get; }
        T OptOutNonPublic { get; }
        T OptIn { get; }
        T Slots(Func<Type, IEnumerable<MemberInfo>> slots);
        T Slots(params MemberInfo[] slots);
        T Slots(IEnumerable<MemberInfo> slots);
        T Slots(Func<IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots);
        T Slots(Func<MemberInfo, bool> slots);
        T NotSlots(Func<Type, IEnumerable<MemberInfo>> slots);
        T NotSlots(params MemberInfo[] slots);
        T NotSlots(IEnumerable<MemberInfo> slots);
        T NotSlots(Func<IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots);
        T NotSlots(Func<MemberInfo, bool> slots);
        T Fields(Func<Type, IEnumerable<FieldInfo>> fields);
        T Fields(params FieldInfo[] fields);
        T Fields(IEnumerable<FieldInfo> fields);
        T Fields(Func<IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields);
        T Fields(Func<FieldInfo, bool> fields);
        T NotFields(Func<Type, IEnumerable<FieldInfo>> fields);
        T NotFields(params FieldInfo[] fields);
        T NotFields(IEnumerable<FieldInfo> fields);
        T NotFields(Func<IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields);
        T NotFields(Func<FieldInfo, bool> fields);
        T Properties(Func<Type, IEnumerable<PropertyInfo>> properties);
        T Properties(params PropertyInfo[] properties);
        T Properties(IEnumerable<PropertyInfo> properties);
        T Properties(Func<IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties);
        T Properties(Func<PropertyInfo, bool> properties);
        T NotProperties(Func<Type, IEnumerable<PropertyInfo>> properties);
        T NotProperties(params PropertyInfo[] properties);
        T NotProperties(IEnumerable<PropertyInfo> properties);
        T NotProperties(Func<IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties);
        T NotProperties(Func<PropertyInfo, bool> properties);
        T IsNotObject { get; }

        T IsList { get; }
        T IsListOf(Type listElement);
        T IsListOf(Func<Type, Type> listElement);
        T IsNotList { get; }

        T IsHash { get; }
        T IsHashOf(Type hashElement);
        T IsHashOf(Func<Type, Type> hashElement);
        T IsNotHash { get; }
    }
}