using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Configuration.Default.Annotations;
using XenoGears.Functional;
using XenoGears.Reflection;

namespace XenoGears.Formats.Configuration.Default.Fluent
{
    public class SingleFluent : IFluent<SingleFluent>
    {
        private readonly JsonMetadata _metadata;
        public SingleFluent(JsonMetadata metadata)
        {
            _metadata = metadata.AssertNotNull();

            // todo. if _metadata.Initialized is false, then init by default
            // using annotations of type _metadata.Type
            // todo. also apply all rules before the instance is initialized
            throw new NotImplementedException();
        }

        public SingleFluent DefaultCtor { get { _metadata.DefaultCtor = true; return this; } }
        public SingleFluent NotDefaultCtor { get { _metadata.DefaultCtor = false; return this; } }

        public SingleFluent IsPrimitive { get { if (!_metadata.IsPrimitive) { _metadata.IsPrimitive = true; _metadata.DynamicPrimitive = null; } return this; } }
        public SingleFluent IsPrimitiveOf(Func<Object, Object> dynamicPrimitive) { var _ = IsPrimitive; _metadata.DynamicPrimitive = dynamicPrimitive; return this; }
        public SingleFluent IsNotPrimitive { get { _metadata.IsPrimitive = false; return this; } }

        public SingleFluent IsObject { get { if (!_metadata.IsObject) { _metadata.IsObject = true; _metadata.Slots = Slots(JsonSlots.Default).ToList(); _metadata.DynamicObject = null; } return this; } }
        public SingleFluent OptOutPublic { get { _metadata.IsObject = true; _metadata.Slots = Slots(JsonSlots.OptOutPublic).ToList(); _metadata.DynamicObject = null; return this; } }
        public SingleFluent OptOutNonPublic { get { _metadata.IsObject = true; _metadata.Slots = Slots(JsonSlots.OptOutNonPublic).ToList(); _metadata.DynamicObject = null; return this; } }
        public SingleFluent OptIn { get { _metadata.IsObject = true; _metadata.Slots = Slots(JsonSlots.OptIn).ToList(); _metadata.DynamicObject = null; return this; } }
        public SingleFluent Slots(Func<Type, IEnumerable<MemberInfo>> slots) { var _1 = IsObject; _metadata.Slots.RemoveElements(slot => slot is MemberInfo); return Slots((slots ?? (_2 => Seq.Empty<MemberInfo>()))(_metadata.Type)); }
        public SingleFluent Slots(params MemberInfo[] slots) { return Slots((IEnumerable<MemberInfo>)slots); }
        public SingleFluent Slots(IEnumerable<MemberInfo> slots) { var _ = IsObject; _metadata.Slots.AddElements(slots ?? Seq.Empty<MemberInfo>()); _metadata.DynamicObject = null; return this; }
        public SingleFluent Slots(Func<Type, IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { var _1 = IsObject; _metadata.Slots = (slots ?? ((_2, _3) => Seq.Empty<MemberInfo>()))(_metadata.Type, _metadata.Slots).ToList(); _metadata.DynamicObject = null; return this; }
        public SingleFluent Slots(Func<MemberInfo, bool> slots) { return Slots((_1, _slots) => _slots.Where(slots ?? (_2 => false))); }
        public SingleFluent NotSlots(Func<Type, IEnumerable<MemberInfo>> slots) { return NotSlots((slots ?? (_ => Seq.Empty<MemberInfo>()))(_metadata.Type)); }
        public SingleFluent NotSlots(params MemberInfo[] slots) { return NotSlots((IEnumerable<MemberInfo>)slots); }
        public SingleFluent NotSlots(IEnumerable<MemberInfo> slots) { var _ = IsObject; _metadata.Slots.RemoveElements(slots ?? Seq.Empty<MemberInfo>()); _metadata.DynamicObject = null; return this; }
        public SingleFluent NotSlots(Func<Type, IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { return NotSlots((slots ?? ((_1, _2) => Seq.Empty<MemberInfo>()))(_metadata.Type, _metadata.Slots)); }
        public SingleFluent NotSlots(Func<MemberInfo, bool> slots) { return NotSlots((_1, _slots) => _slots.Where(slots ?? (_2 => false))); }
        public SingleFluent Fields(Func<Type, IEnumerable<FieldInfo>> fields) { var _1 = IsObject; _metadata.Slots.RemoveElements(slot => slot is FieldInfo); return Fields((fields ?? (_2 => Seq.Empty<FieldInfo>()))(_metadata.Type)); }
        public SingleFluent Fields(params FieldInfo[] fields) { return Fields((IEnumerable<FieldInfo>)fields); }
        public SingleFluent Fields(IEnumerable<FieldInfo> fields) { var _ = IsObject; _metadata.Slots.AddElements(fields ?? Seq.Empty<MemberInfo>()); _metadata.DynamicObject = null; return this; }
        public SingleFluent Fields(Func<Type, IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { var _1 = IsObject; _metadata.Slots = (Seq.Concat(_metadata.Slots.Where(s => !(s is FieldInfo)), (fields ?? ((_2, _3) => Seq.Empty<FieldInfo>()))(_metadata.Type, _metadata.Slots.OfType<FieldInfo>()))).ToList(); return this; }
        public SingleFluent Fields(Func<FieldInfo, bool> fields) { return Fields((_1, _fields) => _fields.Where(fields ?? (_2 => false))); }
        public SingleFluent NotFields(Func<Type, IEnumerable<FieldInfo>> fields) { return NotFields((fields ?? (_ => Seq.Empty<FieldInfo>()))(_metadata.Type)); }
        public SingleFluent NotFields(params FieldInfo[] fields) { return NotFields((IEnumerable<FieldInfo>)fields); }
        public SingleFluent NotFields(IEnumerable<FieldInfo> fields) { var _ = IsObject; _metadata.Slots.RemoveElements(fields ?? Seq.Empty<MemberInfo>()); _metadata.DynamicObject = null; return this; }
        public SingleFluent NotFields(Func<Type, IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { return NotFields((fields ?? ((_1, _2) => Seq.Empty<FieldInfo>()))(_metadata.Type, _metadata.Slots.OfType<FieldInfo>())); }
        public SingleFluent NotFields(Func<FieldInfo, bool> fields) { return NotFields((_1, _fields) => _fields.Where(fields ?? (_2 => false))); }
        public SingleFluent Properties(Func<Type, IEnumerable<PropertyInfo>> properties) { var _1 = IsObject; _metadata.Slots.RemoveElements(slot => slot is PropertyInfo); return Properties((properties ?? (_2 => Seq.Empty<PropertyInfo>()))(_metadata.Type)); }
        public SingleFluent Properties(params PropertyInfo[] properties) { return Properties((IEnumerable<PropertyInfo>)properties); }
        public SingleFluent Properties(IEnumerable<PropertyInfo> properties) { var _ = IsObject; _metadata.Slots.AddElements(properties ?? Seq.Empty<MemberInfo>()); _metadata.DynamicObject = null; return this; }
        public SingleFluent Properties(Func<Type, IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { var _1 = IsObject; _metadata.Slots = (Seq.Concat(_metadata.Slots.Where(s => !(s is PropertyInfo)), (properties ?? ((_2, _3) => Seq.Empty<PropertyInfo>()))(_metadata.Type, _metadata.Slots.OfType<PropertyInfo>()))).ToList(); return this; }
        public SingleFluent Properties(Func<PropertyInfo, bool> properties) { return Properties((_1, _properties) => _properties.Where(properties ?? (_2 => false))); }
        public SingleFluent NotProperties(Func<Type, IEnumerable<PropertyInfo>> properties) { return NotProperties((properties ?? (_ => Seq.Empty<PropertyInfo>()))(_metadata.Type)); }
        public SingleFluent NotProperties(params PropertyInfo[] properties) { return NotProperties((IEnumerable<PropertyInfo>)properties); }
        public SingleFluent NotProperties(IEnumerable<PropertyInfo> properties) { var _ = IsObject; _metadata.Slots.RemoveElements(properties ?? Seq.Empty<MemberInfo>()); _metadata.DynamicObject = null; return this; }
        public SingleFluent NotProperties(Func<Type, IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { return NotProperties((properties ?? ((_1, _2) => Seq.Empty<PropertyInfo>()))(_metadata.Type, _metadata.Slots.OfType<PropertyInfo>())); }
        public SingleFluent NotProperties(Func<PropertyInfo, bool> properties) { return NotProperties((_1, _properties) => _properties.Where(properties ?? (_2 => false))); }
        public SingleFluent IsObjectOf(Func<Object, Dictionary<String, Object>> dynamicObject) { var _ = IsObject; _metadata.Slots = null; _metadata.DynamicObject = dynamicObject; return this; }
        public SingleFluent IsNotObject { get { _metadata.IsObject = false; _metadata.Slots = null; _metadata.DynamicObject = null; return this; } }
        private ReadOnlyCollection<MemberInfo> Slots(JsonSlots slots)
        {
            throw new NotImplementedException();
        }

        public SingleFluent IsList { get { if (!_metadata.IsList) { _metadata.IsList = true; _metadata.ListElement = _metadata.Type.GetEnumerableElement(); _metadata.DynamicList = null; } return this; } }
        public SingleFluent IsListOf(Type listElement) { _metadata.IsList = true; _metadata.ListElement = listElement; _metadata.DynamicList = null; return this; }
        public SingleFluent IsListOf(Func<Type, Type> listElement) { return IsListOf((listElement ?? (_ => null))(_metadata.Type)); }
        public SingleFluent IsListOf(Func<Object, IEnumerable<Object>> dynamicList) { var _ = IsList; _metadata.ListElement = null; _metadata.DynamicList = dynamicList; return this; }
        public SingleFluent IsNotList { get { _metadata.IsList = false; _metadata.ListElement = null; _metadata.DynamicList = null; return this; } }

        public SingleFluent IsHash { get { if (!_metadata.IsHash) { _metadata.IsHash = true; _metadata.HashElement = _metadata.Type.GetDictionaryValue(); _metadata.DynamicHash = null; } return this; } }
        public SingleFluent IsHashOf(Type hashElement) { _metadata.IsHash = true; _metadata.HashElement = hashElement; _metadata.DynamicHash = null; return this; }
        public SingleFluent IsHashOf(Func<Type, Type> hashElement) { return IsHashOf((hashElement ?? (_ => null))(_metadata.Type)); }
        public SingleFluent IsHashOf(Func<Object, Dictionary<Object, Object>> dynamicHash) { var _ = IsHash; _metadata.HashElement = null; _metadata.DynamicHash = dynamicHash; return this; }
        public SingleFluent IsNotHash { get { _metadata.IsHash = false; _metadata.HashElement = null; _metadata.DynamicHash = null; return this; } }
    }
}