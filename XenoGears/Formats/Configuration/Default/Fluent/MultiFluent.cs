using System;
using System.Collections.Generic;
using System.Reflection;

namespace XenoGears.Formats.Configuration.Default.Fluent
{
    public class MultiFluent : IFluent<MultiFluent>
    {
        public MultiFluent DefaultCtor { get { throw new NotImplementedException(); } }
        public MultiFluent NotDefaultCtor { get { throw new NotImplementedException(); } } 

        public MultiFluent IsPrimitive { get { throw new NotImplementedException(); } }
        public MultiFluent IsPrimitiveOf(Func<Object, Object> dynamicPrimitive) { throw new NotImplementedException(); }
        public MultiFluent IsNotPrimitive { get { throw new NotImplementedException(); } }

        public MultiFluent IsObject { get { throw new NotImplementedException(); } }
        public MultiFluent OptOutPublic { get { throw new NotImplementedException(); } }
        public MultiFluent OptOutNonPublic { get { throw new NotImplementedException(); } }
        public MultiFluent OptIn { get { throw new NotImplementedException(); } }
        public MultiFluent Slots(Func<Type, IEnumerable<MemberInfo>> slots) { throw new NotImplementedException(); }
        public MultiFluent Slots(params MemberInfo[] slots) { throw new NotImplementedException(); }
        public MultiFluent Slots(IEnumerable<MemberInfo> slots) { throw new NotImplementedException(); }
        public MultiFluent Slots(Func<Type, IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { throw new NotImplementedException(); }
        public MultiFluent Slots(Func<MemberInfo, bool> slots) { throw new NotImplementedException(); }
        public MultiFluent NotSlots(Func<Type, IEnumerable<MemberInfo>> slots) { throw new NotImplementedException(); }
        public MultiFluent NotSlots(params MemberInfo[] slots) { throw new NotImplementedException(); }
        public MultiFluent NotSlots(IEnumerable<MemberInfo> slots) { throw new NotImplementedException(); }
        public MultiFluent NotSlots(Func<Type, IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { throw new NotImplementedException(); }
        public MultiFluent NotSlots(Func<MemberInfo, bool> slots) { throw new NotImplementedException(); }
        public MultiFluent Fields(Func<Type, IEnumerable<FieldInfo>> fields) { throw new NotImplementedException(); }
        public MultiFluent Fields(params FieldInfo[] fields) { throw new NotImplementedException(); }
        public MultiFluent Fields(IEnumerable<FieldInfo> fields) { throw new NotImplementedException(); }
        public MultiFluent Fields(Func<Type, IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { throw new NotImplementedException(); }
        public MultiFluent Fields(Func<FieldInfo, bool> fields) { throw new NotImplementedException(); }
        public MultiFluent NotFields(Func<Type, IEnumerable<FieldInfo>> fields) { throw new NotImplementedException(); }
        public MultiFluent NotFields(params FieldInfo[] fields) { throw new NotImplementedException(); }
        public MultiFluent NotFields(IEnumerable<FieldInfo> fields) { throw new NotImplementedException(); }
        public MultiFluent NotFields(Func<Type, IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { throw new NotImplementedException(); }
        public MultiFluent NotFields(Func<FieldInfo, bool> fields) { throw new NotImplementedException(); }
        public MultiFluent Properties(Func<Type, IEnumerable<PropertyInfo>> properties) { throw new NotImplementedException(); }
        public MultiFluent Properties(params PropertyInfo[] properties) { throw new NotImplementedException(); }
        public MultiFluent Properties(IEnumerable<PropertyInfo> properties) { throw new NotImplementedException(); }
        public MultiFluent Properties(Func<Type, IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { throw new NotImplementedException(); }
        public MultiFluent Properties(Func<PropertyInfo, bool> properties) { throw new NotImplementedException(); }
        public MultiFluent NotProperties(Func<Type, IEnumerable<PropertyInfo>> properties) { throw new NotImplementedException(); }
        public MultiFluent NotProperties(params PropertyInfo[] properties) { throw new NotImplementedException(); }
        public MultiFluent NotProperties(IEnumerable<PropertyInfo> properties) { throw new NotImplementedException(); }
        public MultiFluent NotProperties(Func<Type, IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { throw new NotImplementedException(); }
        public MultiFluent NotProperties(Func<PropertyInfo, bool> properties) { throw new NotImplementedException(); }
        public MultiFluent IsObjectOf(Func<Object, Dictionary<String, Object>> dynamicObject) { throw new NotImplementedException(); }
        public MultiFluent IsNotObject { get { throw new NotImplementedException(); } }

        public MultiFluent IsList { get { throw new NotImplementedException(); } }
        public MultiFluent IsListOf(Type listElement) { throw new NotImplementedException(); }
        public MultiFluent IsListOf(Func<Type, Type> listElement) { throw new NotImplementedException(); }
        public MultiFluent IsListOf(Func<Object, IEnumerable<Object>> dynamicList) { throw new NotImplementedException(); }
        public MultiFluent IsNotList { get { throw new NotImplementedException(); } }

        public MultiFluent IsHash { get { throw new NotImplementedException(); } }
        public MultiFluent IsHashOf(Type hashElement) { throw new NotImplementedException(); }
        public MultiFluent IsHashOf(Func<Type, Type> hashElement) { throw new NotImplementedException(); }
        public MultiFluent IsHashOf(Func<Object, Dictionary<Object, Object>> dynamicHash) { throw new NotImplementedException(); }
        public MultiFluent IsNotHash { get { throw new NotImplementedException(); } }
    }
}
