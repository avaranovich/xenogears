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
        private readonly Config _config;
        public SingleFluent(Config config)
        {
            _config = config.AssertNotNull();

            // todo. if _config.Initialized is false, then init by default
            // using annotations of type _metadata.Type
            // todo. also apply all rules before the instance is initialized
            throw new NotImplementedException();
        }

        public SingleFluent DefaultCtor { get { _config.DefaultCtor = true; return this; } }
        public SingleFluent NotDefaultCtor { get { _config.DefaultCtor = false; return this; } }

        public SingleFluent IsPrimitive { get { if (!_config.IsPrimitive) { _config.IsPrimitive = true; _config.DynamicPrimitive = null; } return this; } }
        public SingleFluent IsPrimitiveOf(Func<Object, Object> dynamicPrimitive) { var _ = IsPrimitive; _config.DynamicPrimitive = dynamicPrimitive; return this; }
        public SingleFluent IsNotPrimitive { get { _config.IsPrimitive = false; return this; } }

        public SingleFluent IsObject { get { if (!_config.IsObject) { _config.IsObject = true; _config.Slots = Slots(JsonSlots.Default).ToList(); _config.DynamicObject = null; } return this; } }
        public SingleFluent OptOutPublic { get { _config.IsObject = true; _config.Slots = Slots(JsonSlots.OptOutPublic).ToList(); _config.DynamicObject = null; return this; } }
        public SingleFluent OptOutNonPublic { get { _config.IsObject = true; _config.Slots = Slots(JsonSlots.OptOutNonPublic).ToList(); _config.DynamicObject = null; return this; } }
        public SingleFluent OptIn { get { _config.IsObject = true; _config.Slots = Slots(JsonSlots.OptIn).ToList(); _config.DynamicObject = null; return this; } }
        public SingleFluent Slots(Func<Type, IEnumerable<MemberInfo>> slots) { var _1 = IsObject; _config.Slots.RemoveElements(slot => slot is MemberInfo); return Slots((slots ?? (_2 => Seq.Empty<MemberInfo>()))(_config.Type)); }
        public SingleFluent Slots(params MemberInfo[] slots) { return Slots((IEnumerable<MemberInfo>)slots); }
        public SingleFluent Slots(IEnumerable<MemberInfo> slots) { var _ = IsObject; _config.Slots.AddElements(slots ?? Seq.Empty<MemberInfo>()); _config.DynamicObject = null; return this; }
        public SingleFluent Slots(Func<Type, IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { var _1 = IsObject; _config.Slots = (slots ?? ((_2, _3) => Seq.Empty<MemberInfo>()))(_config.Type, _config.Slots).ToList(); _config.DynamicObject = null; return this; }
        public SingleFluent Slots(Func<MemberInfo, bool> slots) { return Slots((_1, _slots) => _slots.Where(slots ?? (_2 => false))); }
        public SingleFluent NotSlots(Func<Type, IEnumerable<MemberInfo>> slots) { return NotSlots((slots ?? (_ => Seq.Empty<MemberInfo>()))(_config.Type)); }
        public SingleFluent NotSlots(params MemberInfo[] slots) { return NotSlots((IEnumerable<MemberInfo>)slots); }
        public SingleFluent NotSlots(IEnumerable<MemberInfo> slots) { var _ = IsObject; _config.Slots.RemoveElements(slots ?? Seq.Empty<MemberInfo>()); _config.DynamicObject = null; return this; }
        public SingleFluent NotSlots(Func<Type, IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { return NotSlots((slots ?? ((_1, _2) => Seq.Empty<MemberInfo>()))(_config.Type, _config.Slots)); }
        public SingleFluent NotSlots(Func<MemberInfo, bool> slots) { return NotSlots((_1, _slots) => _slots.Where(slots ?? (_2 => false))); }
        public SingleFluent Fields(Func<Type, IEnumerable<FieldInfo>> fields) { var _1 = IsObject; _config.Slots.RemoveElements(slot => slot is FieldInfo); return Fields((fields ?? (_2 => Seq.Empty<FieldInfo>()))(_config.Type)); }
        public SingleFluent Fields(params FieldInfo[] fields) { return Fields((IEnumerable<FieldInfo>)fields); }
        public SingleFluent Fields(IEnumerable<FieldInfo> fields) { var _ = IsObject; _config.Slots.AddElements(fields ?? Seq.Empty<MemberInfo>()); _config.DynamicObject = null; return this; }
        public SingleFluent Fields(Func<Type, IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { var _1 = IsObject; _config.Slots = (Seq.Concat(_config.Slots.Where(s => !(s is FieldInfo)), (fields ?? ((_2, _3) => Seq.Empty<FieldInfo>()))(_config.Type, _config.Slots.OfType<FieldInfo>()))).ToList(); return this; }
        public SingleFluent Fields(Func<FieldInfo, bool> fields) { return Fields((_1, _fields) => _fields.Where(fields ?? (_2 => false))); }
        public SingleFluent NotFields(Func<Type, IEnumerable<FieldInfo>> fields) { return NotFields((fields ?? (_ => Seq.Empty<FieldInfo>()))(_config.Type)); }
        public SingleFluent NotFields(params FieldInfo[] fields) { return NotFields((IEnumerable<FieldInfo>)fields); }
        public SingleFluent NotFields(IEnumerable<FieldInfo> fields) { var _ = IsObject; _config.Slots.RemoveElements(fields ?? Seq.Empty<MemberInfo>()); _config.DynamicObject = null; return this; }
        public SingleFluent NotFields(Func<Type, IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { return NotFields((fields ?? ((_1, _2) => Seq.Empty<FieldInfo>()))(_config.Type, _config.Slots.OfType<FieldInfo>())); }
        public SingleFluent NotFields(Func<FieldInfo, bool> fields) { return NotFields((_1, _fields) => _fields.Where(fields ?? (_2 => false))); }
        public SingleFluent Properties(Func<Type, IEnumerable<PropertyInfo>> properties) { var _1 = IsObject; _config.Slots.RemoveElements(slot => slot is PropertyInfo); return Properties((properties ?? (_2 => Seq.Empty<PropertyInfo>()))(_config.Type)); }
        public SingleFluent Properties(params PropertyInfo[] properties) { return Properties((IEnumerable<PropertyInfo>)properties); }
        public SingleFluent Properties(IEnumerable<PropertyInfo> properties) { var _ = IsObject; _config.Slots.AddElements(properties ?? Seq.Empty<MemberInfo>()); _config.DynamicObject = null; return this; }
        public SingleFluent Properties(Func<Type, IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { var _1 = IsObject; _config.Slots = (Seq.Concat(_config.Slots.Where(s => !(s is PropertyInfo)), (properties ?? ((_2, _3) => Seq.Empty<PropertyInfo>()))(_config.Type, _config.Slots.OfType<PropertyInfo>()))).ToList(); return this; }
        public SingleFluent Properties(Func<PropertyInfo, bool> properties) { return Properties((_1, _properties) => _properties.Where(properties ?? (_2 => false))); }
        public SingleFluent NotProperties(Func<Type, IEnumerable<PropertyInfo>> properties) { return NotProperties((properties ?? (_ => Seq.Empty<PropertyInfo>()))(_config.Type)); }
        public SingleFluent NotProperties(params PropertyInfo[] properties) { return NotProperties((IEnumerable<PropertyInfo>)properties); }
        public SingleFluent NotProperties(IEnumerable<PropertyInfo> properties) { var _ = IsObject; _config.Slots.RemoveElements(properties ?? Seq.Empty<MemberInfo>()); _config.DynamicObject = null; return this; }
        public SingleFluent NotProperties(Func<Type, IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { return NotProperties((properties ?? ((_1, _2) => Seq.Empty<PropertyInfo>()))(_config.Type, _config.Slots.OfType<PropertyInfo>())); }
        public SingleFluent NotProperties(Func<PropertyInfo, bool> properties) { return NotProperties((_1, _properties) => _properties.Where(properties ?? (_2 => false))); }
        public SingleFluent IsObjectOf(Func<Object, Dictionary<String, Object>> dynamicObject) { var _ = IsObject; _config.Slots = null; _config.DynamicObject = dynamicObject; return this; }
        public SingleFluent IsNotObject { get { _config.IsObject = false; _config.Slots = null; _config.DynamicObject = null; return this; } }
        private ReadOnlyCollection<MemberInfo> Slots(JsonSlots slots)
        {
            throw new NotImplementedException();
        }

        public SingleFluent IsList { get { if (!_config.IsList) { _config.IsList = true; _config.ListElement = _config.Type.GetEnumerableElement(); _config.DynamicList = null; } return this; } }
        public SingleFluent IsListOf(Type listElement) { _config.IsList = true; _config.ListElement = listElement; _config.DynamicList = null; return this; }
        public SingleFluent IsListOf(Func<Type, Type> listElement) { return IsListOf((listElement ?? (_ => null))(_config.Type)); }
        public SingleFluent IsListOf(Func<Object, IEnumerable<Object>> dynamicList) { var _ = IsList; _config.ListElement = null; _config.DynamicList = dynamicList; return this; }
        public SingleFluent IsNotList { get { _config.IsList = false; _config.ListElement = null; _config.DynamicList = null; return this; } }

        public SingleFluent IsHash { get { if (!_config.IsHash) { _config.IsHash = true; _config.HashElement = _config.Type.GetDictionaryValue(); _config.DynamicHash = null; } return this; } }
        public SingleFluent IsHashOf(Type hashElement) { _config.IsHash = true; _config.HashElement = hashElement; _config.DynamicHash = null; return this; }
        public SingleFluent IsHashOf(Func<Type, Type> hashElement) { return IsHashOf((hashElement ?? (_ => null))(_config.Type)); }
        public SingleFluent IsHashOf(Func<Object, Dictionary<Object, Object>> dynamicHash) { var _ = IsHash; _config.HashElement = null; _config.DynamicHash = dynamicHash; return this; }
        public SingleFluent IsNotHash { get { _config.IsHash = false; _config.HashElement = null; _config.DynamicHash = null; return this; } }
    }
}