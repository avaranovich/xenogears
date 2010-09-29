using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Engines.Configuration.Annotations;
using XenoGears.Functional;

namespace XenoGears.Formats.Engines.Configuration.Fluent
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

        public SingleFluent IsPrimitive { get { _metadata.IsPrimitive = true; return this; } }
        public SingleFluent IsNotPrimitive { get { _metadata.IsPrimitive = false; return this; } }

        public SingleFluent IsObject { get { if (!_metadata.IsObject) { _metadata.IsObject = true; _metadata.Slots = Slots(JsonSlots.Default).ToList(); } return this; } }
        public SingleFluent OptOutPublic { get { _metadata.IsObject = true; _metadata.Slots = Slots(JsonSlots.OptOutPublic).ToList(); return this; } }
        public SingleFluent OptOutNonPublic { get { _metadata.IsObject = true; _metadata.Slots = Slots(JsonSlots.OptOutNonPublic).ToList(); return this; } }
        public SingleFluent OptIn { get { _metadata.IsObject = true; _metadata.Slots = Slots(JsonSlots.OptIn).ToList(); return this; } }
        public SingleFluent Slots(Func<Type, IEnumerable<MemberInfo>> slots) { var _1 = IsObject; _metadata.Slots.RemoveElements(slot => slot is MemberInfo); return Slots((slots ?? (_2 => Seq.Empty<MemberInfo>()))(_metadata.Type)); }
        public SingleFluent Slots(params MemberInfo[] slots) { return Slots((IEnumerable<MemberInfo>)slots); }
        public SingleFluent Slots(IEnumerable<MemberInfo> slots) { var _ = IsObject; _metadata.Slots.AddElements(slots ?? Seq.Empty<MemberInfo>()); return this; }
        public SingleFluent Slots(Func<IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { var _1 = IsObject; _metadata.Slots = (slots ?? (_2 => Seq.Empty<MemberInfo>()))(_metadata.Slots).ToList(); return this; }
        public SingleFluent Slots(Func<MemberInfo, bool> slots) { return Slots(_slots => _slots.Where(slots ?? (_ => false))); }
        public SingleFluent NotSlots(Func<Type, IEnumerable<MemberInfo>> slots) { return NotSlots((slots ?? (_ => Seq.Empty<MemberInfo>()))(_metadata.Type)); }
        public SingleFluent NotSlots(params MemberInfo[] slots) { return NotSlots((IEnumerable<MemberInfo>)slots); }
        public SingleFluent NotSlots(IEnumerable<MemberInfo> slots) { var _ = IsObject; _metadata.Slots.RemoveElements(slots ?? Seq.Empty<MemberInfo>()); return this; }
        public SingleFluent NotSlots(Func<IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { return NotSlots((slots ?? (_ => Seq.Empty<MemberInfo>()))(_metadata.Slots)); }
        public SingleFluent NotSlots(Func<MemberInfo, bool> slots) { return NotSlots(_slots => _slots.Where(slots ?? (_ => false))); }
        public SingleFluent Fields(Func<Type, IEnumerable<FieldInfo>> fields) { var _1 = IsObject; _metadata.Slots.RemoveElements(slot => slot is FieldInfo); return Fields((fields ?? (_2 => Seq.Empty<FieldInfo>()))(_metadata.Type)); }
        public SingleFluent Fields(params FieldInfo[] fields) { return Fields((IEnumerable<FieldInfo>)fields); }
        public SingleFluent Fields(IEnumerable<FieldInfo> fields) { var _ = IsObject; _metadata.Slots.AddElements(fields ?? Seq.Empty<MemberInfo>()); return this; }
        public SingleFluent Fields(Func<IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { var _1 = IsObject; _metadata.Slots = (Seq.Concat(_metadata.Slots.Where(s => !(s is FieldInfo)), (fields ?? (_2 => Seq.Empty<FieldInfo>()))(_metadata.Slots.OfType<FieldInfo>()))).ToList(); return this; }
        public SingleFluent Fields(Func<FieldInfo, bool> fields) { return Fields(_fields => _fields.Where(fields ?? (_ => false))); }
        public SingleFluent NotFields(Func<Type, IEnumerable<FieldInfo>> fields) { return NotFields((fields ?? (_ => Seq.Empty<FieldInfo>()))(_metadata.Type)); }
        public SingleFluent NotFields(params FieldInfo[] fields) { return NotFields((IEnumerable<FieldInfo>)fields); }
        public SingleFluent NotFields(IEnumerable<FieldInfo> fields) { var _ = IsObject; _metadata.Slots.RemoveElements(fields ?? Seq.Empty<MemberInfo>()); return this; }
        public SingleFluent NotFields(Func<IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { return NotFields((fields ?? (_ => Seq.Empty<FieldInfo>()))(_metadata.Slots.OfType<FieldInfo>())); }
        public SingleFluent NotFields(Func<FieldInfo, bool> fields) { return NotFields(_fields => _fields.Where(fields ?? (_ => false))); }
        public SingleFluent Properties(Func<Type, IEnumerable<PropertyInfo>> properties) { var _1 = IsObject; _metadata.Slots.RemoveElements(slot => slot is PropertyInfo); return Properties((properties ?? (_2 => Seq.Empty<PropertyInfo>()))(_metadata.Type)); }
        public SingleFluent Properties(params PropertyInfo[] properties) { return Properties((IEnumerable<PropertyInfo>)properties); }
        public SingleFluent Properties(IEnumerable<PropertyInfo> properties) { var _ = IsObject; _metadata.Slots.AddElements(properties ?? Seq.Empty<MemberInfo>()); return this; }
        public SingleFluent Properties(Func<IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { var _1 = IsObject; _metadata.Slots = (Seq.Concat(_metadata.Slots.Where(s => !(s is PropertyInfo)), (properties ?? (_2 => Seq.Empty<PropertyInfo>()))(_metadata.Slots.OfType<PropertyInfo>()))).ToList(); return this; }
        public SingleFluent Properties(Func<PropertyInfo, bool> properties) { return Properties(_properties => _properties.Where(properties ?? (_ => false))); }
        public SingleFluent NotProperties(Func<Type, IEnumerable<PropertyInfo>> properties) { return NotProperties((properties ?? (_ => Seq.Empty<PropertyInfo>()))(_metadata.Type)); }
        public SingleFluent NotProperties(params PropertyInfo[] properties) { return NotProperties((IEnumerable<PropertyInfo>)properties); }
        public SingleFluent NotProperties(IEnumerable<PropertyInfo> properties) { var _ = IsObject; _metadata.Slots.RemoveElements(properties ?? Seq.Empty<MemberInfo>()); return this; }
        public SingleFluent NotProperties(Func<IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { return NotProperties((properties ?? (_ => Seq.Empty<PropertyInfo>()))(_metadata.Slots.OfType<PropertyInfo>())); }
        public SingleFluent NotProperties(Func<PropertyInfo, bool> properties) { return NotProperties(_properties => _properties.Where(properties ?? (_ => false))); }
        public SingleFluent IsNotObject { get { _metadata.IsObject = false; _metadata.Slots = null; return this; } }
        private ReadOnlyCollection<MemberInfo> Slots(JsonSlots slots)
        {
            throw new NotImplementedException();
        }

        public SingleFluent IsList { get { if (!_metadata.IsList) { _metadata.IsList = true; _metadata.ListElement = ListElement(); } return this; } }
        public SingleFluent IsListOf(Type listElement) { _metadata.IsList = true; _metadata.ListElement = listElement; return this; }
        public SingleFluent IsListOf(Func<Type, Type> listElement) { return IsListOf((listElement ?? (_ => null))(_metadata.Type)); }
        public SingleFluent IsNotList { get { _metadata.IsList = false; _metadata.ListElement = null; return this; } }
        private Type ListElement()
        {
            // don't throw if it ain't have enumerable element! just return Object
            throw new NotImplementedException();
        }

        public SingleFluent IsHash { get { if (!_metadata.IsHash) { _metadata.IsHash = true; _metadata.HashElement = HashElement(); } return this; } }
        public SingleFluent IsHashOf(Type hashElement) { _metadata.IsHash = true; _metadata.HashElement = hashElement; return this; }
        public SingleFluent IsHashOf(Func<Type, Type> hashElement) { return IsHashOf((hashElement ?? (_ => null))(_metadata.Type)); }
        public SingleFluent IsNotHash { get { _metadata.IsHash = false; _metadata.HashElement = null; return this; } }
        private Type HashElement()
        {
            // don't throw if it ain't have hash element! just return Object
            throw new NotImplementedException();
        }
    }
}