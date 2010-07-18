using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection;
using XenoGears.Reflection.Generics;

namespace XenoGears.Collections.Observable
{
    [DebuggerNonUserCode]
    public static class ObservableLists
    {
        public static IObservableList<T> Empty<T>()
        {
            return new ObservableList<T>();
        }

        public static IObservableList Empty(Type t)
        {
            var t_ocol = typeof(ObservableList<>).XMakeGenericType(t.AssertNotNull());
            return (IObservableList)Activator.CreateInstance(t_ocol);
        }

        public static IObservableList<T> Observe<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null ? Empty<T>() : new ObservableList<T>(enumerable);
        }

        public static IObservableList Observe(this IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return new ObservableList(enumerable);
            }
            else
            {
                var elType = enumerable.GetType().GetEnumerableElement();
                if (elType == null)
                {
                    return new ObservableList(enumerable);
                }
                else
                {
                    var t_ocol = typeof(ObservableList<>).XMakeGenericType(elType);
                    var cast_t = typeof(Enumerable).GetMethod("Cast").XMakeGenericMethod(elType);
                    var enum_oft = cast_t.Invoke(null, enumerable.MkArray());
                    return (IObservableList)Activator.CreateInstance(t_ocol, enum_oft.MkArray());
                }
            }
        }
    }
}