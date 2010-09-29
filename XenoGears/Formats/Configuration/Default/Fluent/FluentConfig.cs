using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Formats.Configuration.Default.Annotations;
using XenoGears.Functional;

namespace XenoGears.Formats.Configuration.Default.Fluent
{
    [DebuggerNonUserCode]
    public class FluentConfig : IFluentSettings<FluentConfig>
    {
        public TypeConfig Config { get; private set; }
        public FluentConfig(TypeConfig config) { Config = config; }

        public FluentConfig DefaultCtor { get { Config.DefaultCtor = true; return this; } }
        public FluentConfig NotDefaultCtor { get { Config.DefaultCtor = false; return this; } }

        public FluentConfig IsPrimitive { get { if (!Config.IsPrimitive) { Config.IsPrimitive = true; } return this; } }
        public FluentConfig IsNotPrimitive { get { Config.IsPrimitive = false; return this; } }

        public FluentConfig IsObject { get { if (!Config.IsObject) { Config.IsObject = true; Config.Slots = Config.Type.JsonSlots(JsonSlots.Default).ToList(); } return this; } }
        public FluentConfig OptOutPublic { get { Config.IsObject = true; Config.Slots = Config.Type.JsonSlots(JsonSlots.OptOutPublic).ToList(); return this; } }
        public FluentConfig OptOutNonPublic { get { Config.IsObject = true; Config.Slots = Config.Type.JsonSlots(JsonSlots.OptOutNonPublic).ToList(); return this; } }
        public FluentConfig OptIn { get { Config.IsObject = true; Config.Slots = Config.Type.JsonSlots(JsonSlots.OptIn).ToList(); return this; } }
        public FluentConfig Slots(Func<Type, IEnumerable<MemberInfo>> slots) { var _1 = IsObject; Config.Slots.RemoveElements(slot => slot is MemberInfo); return Slots((slots ?? (_2 => Seq.Empty<MemberInfo>()))(Config.Type)); }
        public FluentConfig Slots(params MemberInfo[] slots) { return Slots((IEnumerable<MemberInfo>)slots); }
        public FluentConfig Slots(IEnumerable<MemberInfo> slots) { var _ = IsObject; Config.Slots.AddElements(slots ?? Seq.Empty<MemberInfo>()); return this; }
        public FluentConfig Slots(Func<Type, IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { var _1 = IsObject; Config.Slots = (slots ?? ((_2, _3) => Seq.Empty<MemberInfo>()))(Config.Type, Config.Slots).ToList(); return this; }
        public FluentConfig Slots(Func<MemberInfo, bool> slots) { return Slots((_1, _slots) => _slots.Where(slots ?? (_2 => false))); }
        public FluentConfig NotSlots(Func<Type, IEnumerable<MemberInfo>> slots) { return NotSlots((slots ?? (_ => Seq.Empty<MemberInfo>()))(Config.Type)); }
        public FluentConfig NotSlots(params MemberInfo[] slots) { return NotSlots((IEnumerable<MemberInfo>)slots); }
        public FluentConfig NotSlots(IEnumerable<MemberInfo> slots) { var _ = IsObject; Config.Slots.RemoveElements(slots ?? Seq.Empty<MemberInfo>()); return this; }
        public FluentConfig NotSlots(Func<Type, IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { return NotSlots((slots ?? ((_1, _2) => Seq.Empty<MemberInfo>()))(Config.Type, Config.Slots)); }
        public FluentConfig NotSlots(Func<MemberInfo, bool> slots) { return NotSlots((_1, _slots) => _slots.Where(slots ?? (_2 => false))); }
        public FluentConfig Fields(Func<Type, IEnumerable<FieldInfo>> fields) { var _1 = IsObject; Config.Slots.RemoveElements(slot => slot is FieldInfo); return Fields((fields ?? (_2 => Seq.Empty<FieldInfo>()))(Config.Type)); }
        public FluentConfig Fields(params FieldInfo[] fields) { return Fields((IEnumerable<FieldInfo>)fields); }
        public FluentConfig Fields(IEnumerable<FieldInfo> fields) { var _ = IsObject; Config.Slots.AddElements(fields ?? Seq.Empty<MemberInfo>()); return this; }
        public FluentConfig Fields(Func<Type, IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { var _1 = IsObject; Config.Slots = (Seq.Concat(Config.Slots.Where(s => !(s is FieldInfo)), (fields ?? ((_2, _3) => Seq.Empty<FieldInfo>()))(Config.Type, Config.Slots.OfType<FieldInfo>()))).ToList(); return this; }
        public FluentConfig Fields(Func<FieldInfo, bool> fields) { return Fields((_1, _fields) => _fields.Where(fields ?? (_2 => false))); }
        public FluentConfig NotFields(Func<Type, IEnumerable<FieldInfo>> fields) { return NotFields((fields ?? (_ => Seq.Empty<FieldInfo>()))(Config.Type)); }
        public FluentConfig NotFields(params FieldInfo[] fields) { return NotFields((IEnumerable<FieldInfo>)fields); }
        public FluentConfig NotFields(IEnumerable<FieldInfo> fields) { var _ = IsObject; Config.Slots.RemoveElements(fields ?? Seq.Empty<MemberInfo>()); return this; }
        public FluentConfig NotFields(Func<Type, IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { return NotFields((fields ?? ((_1, _2) => Seq.Empty<FieldInfo>()))(Config.Type, Config.Slots.OfType<FieldInfo>())); }
        public FluentConfig NotFields(Func<FieldInfo, bool> fields) { return NotFields((_1, _fields) => _fields.Where(fields ?? (_2 => false))); }
        public FluentConfig Properties(Func<Type, IEnumerable<PropertyInfo>> properties) { var _1 = IsObject; Config.Slots.RemoveElements(slot => slot is PropertyInfo); return Properties((properties ?? (_2 => Seq.Empty<PropertyInfo>()))(Config.Type)); }
        public FluentConfig Properties(params PropertyInfo[] properties) { return Properties((IEnumerable<PropertyInfo>)properties); }
        public FluentConfig Properties(IEnumerable<PropertyInfo> properties) { var _ = IsObject; Config.Slots.AddElements(properties ?? Seq.Empty<MemberInfo>()); return this; }
        public FluentConfig Properties(Func<Type, IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { var _1 = IsObject; Config.Slots = (Seq.Concat(Config.Slots.Where(s => !(s is PropertyInfo)), (properties ?? ((_2, _3) => Seq.Empty<PropertyInfo>()))(Config.Type, Config.Slots.OfType<PropertyInfo>()))).ToList(); return this; }
        public FluentConfig Properties(Func<PropertyInfo, bool> properties) { return Properties((_1, _properties) => _properties.Where(properties ?? (_2 => false))); }
        public FluentConfig NotProperties(Func<Type, IEnumerable<PropertyInfo>> properties) { return NotProperties((properties ?? (_ => Seq.Empty<PropertyInfo>()))(Config.Type)); }
        public FluentConfig NotProperties(params PropertyInfo[] properties) { return NotProperties((IEnumerable<PropertyInfo>)properties); }
        public FluentConfig NotProperties(IEnumerable<PropertyInfo> properties) { var _ = IsObject; Config.Slots.RemoveElements(properties ?? Seq.Empty<MemberInfo>()); return this; }
        public FluentConfig NotProperties(Func<Type, IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { return NotProperties((properties ?? ((_1, _2) => Seq.Empty<PropertyInfo>()))(Config.Type, Config.Slots.OfType<PropertyInfo>())); }
        public FluentConfig NotProperties(Func<PropertyInfo, bool> properties) { return NotProperties((_1, _properties) => _properties.Where(properties ?? (_2 => false))); }
        public FluentConfig IsNotObject { get { Config.IsObject = false; Config.Slots = null; return this; } }

        public FluentConfig IsList { get { if (!Config.IsList) { Config.IsList = true; Config.ListElement = Config.Type.ListElement(); } return this; } }
        public FluentConfig IsListOf(Type listElement) { Config.IsList = true; Config.ListElement = listElement ?? Config.Type.ListElement(); return this; }
        public FluentConfig IsListOf(Func<Type, Type> listElement) { return IsListOf((listElement ?? (_ => null))(Config.Type)); }
        public FluentConfig IsNotList { get { Config.IsList = false; Config.ListElement = null; return this; } }

        public FluentConfig IsHash { get { if (!Config.IsHash) { Config.IsHash = true; Config.HashElement = Config.Type.HashElement(); } return this; } }
        public FluentConfig IsHashOf(Type hashElement) { Config.IsHash = true; Config.HashElement = hashElement ?? Config.Type.HashElement(); return this; }
        public FluentConfig IsHashOf(Func<Type, Type> hashElement) { return IsHashOf((hashElement ?? (_ => null))(Config.Type)); }
        public FluentConfig IsNotHash { get { Config.IsHash = false; Config.HashElement = null; return this; } }
    }
}