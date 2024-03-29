﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Formats.Adapters.Core;
using XenoGears.Formats.Adapters.Lambda;
using XenoGears.Formats.Engines;
using XenoGears.Formats.Engines.Core;
using XenoGears.Formats.Engines.Lambda;
using XenoGears.Formats.Validators.Core;
using XenoGears.Formats.Validators.Lambda;
using XenoGears.Functional;
using XenoGears.Reflection;
using XenoGears.Reflection.Attributes;

namespace XenoGears.Formats.Configuration
{
    [DebuggerNonUserCode]
    public class TypeConfig : Config
    {
        public TypeConfig(Type type)
            : base(type)
        {
        }

        public ReadOnlyCollection<TypeAdapter> Adapters
        {
            get
            {
                if (Type != null && Type.Name.StartsWith("Reified_"))
                {
                    var t_base = Type.Hierarchy().AssertSecond();
                    var base_adapters = t_base.Config().Adapters;
                    return base_adapters;
                }
                else
                {
                    var lam_adapters = (Hash.GetOrDefault(typeof(LambdaAdapters.LambdaAfterDeserializeTypeAdapter)).AssertCast<IEnumerable<TypeAdapter>>() ?? Seq.Empty<TypeAdapter>()).OrderBy(adapter => adapter.Weight).ToReadOnly();
                    lam_adapters = Seq.Concat(lam_adapters, (Hash.GetOrDefault(typeof(LambdaAdapters.LambdaBeforeSerializeTypeAdapter)).AssertCast<IEnumerable<TypeAdapter>>() ?? Seq.Empty<TypeAdapter>())).OrderBy(adapter => adapter.Weight).ToReadOnly();
                    var a_adapters = (Type.Attrs<TypeAdapter>() ?? Seq.Empty<TypeAdapter>()).OrderBy(adapter => adapter.Weight).ToReadOnly();
                    var adapters = Seq.Concat(lam_adapters, a_adapters).OrderBy(adapter => adapter.Weight).ToReadOnly();
                    return adapters;
                }
            }
        }

        public ReadOnlyCollection<TypeValidator> Validators
        {
            get
            {
                if (Type != null && Type.Name.StartsWith("Reified_"))
                {
                    var t_base = Type.Hierarchy().AssertSecond();
                    var base_validators = t_base.Config().Validators;
                    return base_validators;
                }
                else
                {
                    var lam_validators = Hash.GetOrDefault(typeof(LambdaValidators.LambdaTypeValidator)).AssertCast<IEnumerable<TypeValidator>>() ?? Seq.Empty<TypeValidator>();
                    var a_validators = Type.Attrs<TypeValidator>() ?? Seq.Empty<TypeValidator>();
                    var validators = Seq.Concat(lam_validators, a_validators).ToReadOnly();
                    return validators;
                }
            }
        }

        public TypeEngine Engine
        {
            get
            {
                if (Type != null && Type.Name.StartsWith("Reified_"))
                {
                    var t_base = Type.Hierarchy().AssertSecond();
                    var base_engine = t_base.Config().Engine;
                    return base_engine;
                }
                else
                {
                    var lam_engine = Hash.GetOrDefault(typeof(LambdaEngines.LambdaTypeEngine)).AssertCast<TypeEngine>();
                    var a_engine = Type.AttrOrNull<TypeEngine>();
                    var type_engine = lam_engine ?? a_engine ?? new DefaultEngine();
                    return type_engine;
                }
            }
        }
    }
}