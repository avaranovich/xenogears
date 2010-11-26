using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Configuration.Default.Fluent
{
    [DebuggerNonUserCode]
    public class FluentRule : IFluentSettings<FluentRule>
    {
        public TypeRule Rule { get; private set; }
        public FluentRule(TypeRule rule) { Rule = rule.AssertNotNull(); }
        private FluentRule Record(Action<FluentConfig> change) { Rule.Clauses.Add(change); Rule.Apply(); return this; }
        private FluentRule Record<T>(Func<FluentConfig, T> change) { Rule.Clauses.Add(cfg => change(cfg)); Rule.Apply(); return this; }

        public FluentRule DefaultCtor { get { return Record(cfg => cfg.DefaultCtor); } }
        public FluentRule NotDefaultCtor { get { return Record(cfg => cfg.NotDefaultCtor); } }

        public FluentRule IsPrimitive { get { return Record(cfg => cfg.IsPrimitive); } }
        public FluentRule IsNotPrimitive { get { return Record(cfg => cfg.IsNotPrimitive); } }

        public FluentRule IsObject { get { return Record(cfg => cfg.IsObject); } }
        public FluentRule OptOutPublic { get { return Record(cfg => cfg.OptOutPublic); } }
        public FluentRule OptOutNonPublic { get { return Record(cfg => cfg.OptOutNonPublic); } }
        public FluentRule OptInPublic { get { return Record(cfg => cfg.OptInPublic); } }
        public FluentRule OptInNonPublic { get { return Record(cfg => cfg.OptInNonPublic); } }
        public FluentRule Slots(Func<Type, IEnumerable<MemberInfo>> slots) { return Record(cfg => cfg.Slots(slots)); }
        public FluentRule Slots(params MemberInfo[] slots) { return Record(cfg => cfg.Slots(slots)); }
        public FluentRule Slots(IEnumerable<MemberInfo> slots) { return Record(cfg => cfg.Slots(slots)); }
        public FluentRule Slots(Func<Type, IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { return Record(cfg => cfg.Slots(slots)); }
        public FluentRule Slots(Func<MemberInfo, bool> slots) { return Record(cfg => cfg.Slots(slots)); }
        public FluentRule NotSlots(Func<Type, IEnumerable<MemberInfo>> slots) { return Record(cfg => cfg.NotSlots(slots)); }
        public FluentRule NotSlots(params MemberInfo[] slots) { return Record(cfg => cfg.NotSlots(slots)); }
        public FluentRule NotSlots(IEnumerable<MemberInfo> slots) { return Record(cfg => cfg.NotSlots(slots)); }
        public FluentRule NotSlots(Func<Type, IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> slots) { return Record(cfg => cfg.NotSlots(slots)); }
        public FluentRule NotSlots(Func<MemberInfo, bool> slots) { return Record(cfg => cfg.NotSlots(slots)); }
        public FluentRule Fields(Func<Type, IEnumerable<FieldInfo>> fields) { return Record(cfg => cfg.Fields(fields)); }
        public FluentRule Fields(params FieldInfo[] fields) { return Record(cfg => cfg.Fields(fields)); }
        public FluentRule Fields(IEnumerable<FieldInfo> fields) { return Record(cfg => cfg.Fields(fields)); }
        public FluentRule Fields(Func<Type, IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { return Record(cfg => cfg.Fields(fields)); }
        public FluentRule Fields(Func<FieldInfo, bool> fields) { return Record(cfg => cfg.Fields(fields)); }
        public FluentRule NotFields(Func<Type, IEnumerable<FieldInfo>> fields) { return Record(cfg => cfg.NotFields(fields)); }
        public FluentRule NotFields(params FieldInfo[] fields) { return Record(cfg => cfg.NotFields(fields)); }
        public FluentRule NotFields(IEnumerable<FieldInfo> fields) { return Record(cfg => cfg.NotFields(fields)); }
        public FluentRule NotFields(Func<Type, IEnumerable<FieldInfo>, IEnumerable<FieldInfo>> fields) { return Record(cfg => cfg.NotFields(fields)); }
        public FluentRule NotFields(Func<FieldInfo, bool> fields) { return Record(cfg => cfg.NotFields(fields)); }
        public FluentRule Properties(Func<Type, IEnumerable<PropertyInfo>> properties) { return Record(cfg => cfg.Properties(properties)); }
        public FluentRule Properties(params PropertyInfo[] properties) { return Record(cfg => cfg.Properties(properties)); }
        public FluentRule Properties(IEnumerable<PropertyInfo> properties) { return Record(cfg => cfg.Properties(properties)); }
        public FluentRule Properties(Func<Type, IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { return Record(cfg => cfg.Properties(properties)); }
        public FluentRule Properties(Func<PropertyInfo, bool> properties) { return Record(cfg => cfg.Properties(properties)); }
        public FluentRule NotProperties(Func<Type, IEnumerable<PropertyInfo>> properties) { return Record(cfg => cfg.NotProperties(properties)); }
        public FluentRule NotProperties(params PropertyInfo[] properties) { return Record(cfg => cfg.NotProperties(properties)); }
        public FluentRule NotProperties(IEnumerable<PropertyInfo> properties) { return Record(cfg => cfg.NotProperties(properties)); }
        public FluentRule NotProperties(Func<Type, IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> properties) { return Record(cfg => cfg.NotProperties(properties)); }
        public FluentRule NotProperties(Func<PropertyInfo, bool> properties) { return Record(cfg => cfg.NotProperties(properties)); }
        public FluentRule LowercaseSlotNames() { return Record(cfg => cfg.LowercaseSlotNames()); }
        public FluentRule VerbatimSlotNames() { return Record(cfg => cfg.VerbatimSlotNames()); }
        public FluentRule IsNotObject { get { return Record(cfg => cfg.IsNotObject); } }

        public FluentRule IsList{ get { return Record(cfg => cfg.IsList); } }
        public FluentRule IsListOf(Type listElement) { return Record(cfg => cfg.IsListOf(listElement)); }
        public FluentRule IsListOf(Func<Type, Type> listElement) { return Record(cfg => cfg.IsListOf(listElement)); }
        public FluentRule IsNotList { get { return Record(cfg => cfg.IsNotList); } }

        public FluentRule IsHash { get { return Record(cfg => cfg.IsHash); } }
        public FluentRule IsHashOf(Type hashElement) { return Record(cfg => cfg.IsHashOf(hashElement)); }
        public FluentRule IsHashOf(Func<Type, Type> hashElement) { return Record(cfg => cfg.IsHashOf(hashElement)); }
        public FluentRule IsNotHash { get { return Record(cfg => cfg.IsNotHash); } }
    }
}