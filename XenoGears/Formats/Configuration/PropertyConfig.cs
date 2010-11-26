using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Adapters.Core;
using XenoGears.Formats.Adapters.Lambda;
using XenoGears.Formats.Engines.Core;
using XenoGears.Formats.Engines.Lambda;
using XenoGears.Formats.Validators.Core;
using XenoGears.Formats.Validators.Lambda;
using XenoGears.Functional;
using XenoGears.Reflection.Attributes;
using XenoGears.Reflection;

namespace XenoGears.Formats.Configuration
{
    [DebuggerNonUserCode]
    public class PropertyConfig : Config
    {
        public PropertyConfig(PropertyInfo property)
            : base(property)
        {
        }

        public ReadOnlyCollection<PropertyAdapter> Adapters
        {
            get
            {
                if (Property != null && Property.DeclaringType.Name.StartsWith("Reified_"))
                {
                    var p_base = Property.Hierarchy().AssertSecond();
                    var base_adapters = p_base.Config().Adapters;
                    return base_adapters;
                }
                else
                {
                    var lam_adapters = (Hash.GetOrDefault(typeof(LambdaAdapters.LambdaAfterDeserializePropertyAdapter)).AssertCast<IEnumerable<PropertyAdapter>>() ?? Seq.Empty<PropertyAdapter>()).OrderBy(adapter => adapter.Weight).ToReadOnly();
                    lam_adapters = Seq.Concat(lam_adapters, (Hash.GetOrDefault(typeof(LambdaAdapters.LambdaBeforeSerializePropertyAdapter)).AssertCast<IEnumerable<PropertyAdapter>>() ?? Seq.Empty<PropertyAdapter>())).OrderBy(adapter => adapter.Weight).ToReadOnly();
                    var a_adapters = (Type.Attrs<PropertyAdapter>() ?? Seq.Empty<PropertyAdapter>()).OrderBy(adapter => adapter.Weight).ToReadOnly();
                    var adapters = Seq.Concat(lam_adapters, a_adapters).OrderBy(adapter => adapter.Weight).ToReadOnly();
                    return adapters;
                }
            }
        }

        public ReadOnlyCollection<PropertyValidator> Validators
        {
            get
            {
                if (Property != null && Property.DeclaringType.Name.StartsWith("Reified_"))
                {
                    var p_base = Property.Hierarchy().AssertSecond();
                    var base_validators = p_base.Config().Validators;
                    return base_validators;
                }
                else
                {
                    var lam_validators = Hash.GetOrDefault(typeof(LambdaValidators.LambdaPropertyValidator)).AssertCast<IEnumerable<PropertyValidator>>() ?? Seq.Empty<PropertyValidator>();
                    var a_validators = Property.Attrs<PropertyValidator>() ?? Seq.Empty<PropertyValidator>();
                    var validators = Seq.Concat(lam_validators, a_validators).ToReadOnly();
                    return validators;
                }
            }
        }

        public PropertyEngine Engine
        {
            get
            {
                if (Property != null && Property.DeclaringType.Name.StartsWith("Reified_"))
                {
                    var p_base = Property.Hierarchy().AssertSecond();
                    var base_engine = p_base.Config().Engine;
                    return base_engine;
                }
                else
                {
                    var lam_engine = Hash.GetOrDefault(typeof(LambdaEngines.LambdaPropertyEngine)).AssertCast<PropertyEngine>();
                    var a_engine = Property.AttrOrNull<PropertyEngine>();
                    var type_engine = lam_engine ?? a_engine;
                    return type_engine;
                }
            }
        }
    }
}