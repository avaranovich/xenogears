using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Reflection.Attributes;

namespace XenoGears.Formats.Configuration.Default.Annotations
{
    [DebuggerNonUserCode]
    public static class JsonSlotsHelper
    {
        public static ReadOnlyCollection<MemberInfo> JsonSlots(this Type root, JsonSlots options)
        {
            if (root == null) return null;

            var slots = new List<MemberInfo>();
            root.Hierarchy().ForEach(t =>
            {
                var t_slots = t.JsonSlotsForThisVeryT(options);
                t_slots = t_slots.Where(s_t => slots.None(s => s.Overrides(s_t))).ToReadOnly();
                slots.AddElements(t_slots);
            });

            return slots.Distinct().ToReadOnly();
        }

        private static ReadOnlyCollection<MemberInfo> JsonSlotsForThisVeryT(this Type t, JsonSlots options)
        {
            if (options == Annotations.JsonSlots.None)
            {
                return Seq.Empty<MemberInfo>().ToReadOnly();
            }
            else if (options == Annotations.JsonSlots.OptOutPublic)
            {
                return t.GetProperties(BF.PublicInstance | BF.DeclOnly).Where(pi => !pi.HasAttr<JsonExcludeAttribute>() && pi.GetIndexParameters().IsEmpty()).Cast<MemberInfo>().ToReadOnly();
            }
            else if (options == Annotations.JsonSlots.OptOutNonPublic)
            {
                return t.GetProperties(BF.AllInstance | BF.DeclOnly).Where(pi => !pi.HasAttr<JsonExcludeAttribute>() && pi.GetIndexParameters().IsEmpty()).Cast<MemberInfo>().ToReadOnly();
            }
            else if (options == Annotations.JsonSlots.OptInPublic)
            {
                return t.GetProperties(BF.PublicInstance | BF.DeclOnly).Where(pi => pi.HasAttr<JsonIncludeAttribute>() && pi.GetIndexParameters().IsEmpty()).Cast<MemberInfo>().ToReadOnly();
            }
            else if (options == Annotations.JsonSlots.OptInNonPublic)
            {
                return t.GetProperties(BF.AllInstance | BF.DeclOnly).Where(pi => pi.HasAttr<JsonIncludeAttribute>() && pi.GetIndexParameters().IsEmpty()).Cast<MemberInfo>().ToReadOnly();
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }
    }
}