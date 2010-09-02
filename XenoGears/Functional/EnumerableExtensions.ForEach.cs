using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static void ForEach(this IEnumerable seq, Action<Object> action)
        {
            if (seq == null) return;
            ForEach(seq.Cast<Object>(), action);
        }

        public static void ForEach<T>(this IEnumerable seq, Action<T> action)
        {
            if (seq == null) return;
            ForEach(seq.Cast<T>(), action);
        }

        public static void ForEach(this IEnumerable seq, Action<Object, int> action)
        {
            if (seq == null) return;
            ForEach(seq.Cast<Object>(), action);
        }

        public static void ForEach<T>(this IEnumerable seq, Action<T, int> action)
        {
            if (seq == null) return;
            ForEach(seq.Cast<T>(), action);
        }

        public static void ForEach<T>(this IEnumerable<T> seq, Action<T> action)
        {
            foreach (var element in (seq ?? Enumerable.Empty<T>()).ToArray())
            {
                action(element);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> seq, Action<T, int> action)
        {
            var i = 0;
            foreach (var element in (seq ?? Enumerable.Empty<T>()).ToArray())
            {
                action(element, i++);
            }
        }

        public static void RunEach(this IEnumerable<Action> actions)
        {
            foreach (var action in (actions ?? Enumerable.Empty<Action>()).ToArray())
            {
                action();
            }
        }

//        public static void Ping(this IEnumerable seq)
//        {
//            foreach (var element in seq ?? new ArrayList())
//            {
//                // just do nothing
//            }
//        }

        public static void Ping<T>(this IEnumerable<T> seq)
        {
            foreach (var element in seq ?? Enumerable.Empty<T>())
            {
                // just do nothing
            }
        }
    }
}