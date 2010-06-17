using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using XenoGears.Assertions;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        // GENERAL CASE OF GENERATORS

        public static IEnumerable<T> Unfold<T>(this T seed, Func<T, T> iter)
        {
            return seed.Unfoldi(iter);
        }

        public static IEnumerable<T> Unfold<T>(this T seed, Func<T, T> iter, Func<T, bool> alive)
        {
            return seed.Unfoldi(iter, alive);
        }

        public static IEnumerable<T> Unfoldi<T>(this T seed, Func<T, T> iter)
        {
            return Unfoldi(seed, iter, t => true);
        }

        public static IEnumerable<T> Unfoldi<T>(this T seed, Func<T, T> iter, Func<T, bool> alive)
        {
            for (var curr = seed; alive(curr); curr = iter(curr))
                yield return curr;
        }

        public static IEnumerable<T> Unfolde<T>(this T seed, Func<T, T> iter)
        {
            return seed.Unfoldi(iter).Skip(1);
        }

        public static IEnumerable<T> Unfolde<T>(this T seed, Func<T, T> iter, Func<T, bool> alive)
        {
            return seed.Unfoldi(iter, alive).Skip(1);
        }

        // DUPLICATE OBJECTS

        public static String Repeat(this String s, int times)
        {
            (times >= 0).AssertTrue();
            var buf = new StringBuilder();
            times.TimesDo(_ => buf.Append(s));
            return buf.ToString();
        }

        public static IEnumerable<T> Repeat<T>(this T t, int times)
        {
            (times >= 0).AssertTrue();
            return 1.UpTo(times).Select(_ => t);
        }

        public static IEnumerable<T> RepeatInfinite<T>(this T t)
        {
            return Seq.Infinite(_ => t);
        }

        public static String Times(this int times, String s)
        {
            return s.Repeat(times);
        }

        public static IEnumerable<T> Times<T>(this int times, T t)
        {
            return t.Repeat(times);
        }

        public static IEnumerable<T> TimesInfinite<T>(this T t)
        {
            return t.RepeatInfinite();
        }

        public static IEnumerable<T> Times<T>(this int count, Func<T> func)
        {
            (count >= 0).AssertTrue();
            for (var i = 0; i < count; ++i)
            {
                yield return func();
            }
        }

        public static IEnumerable<T> Times<T>(this int count, Func<int, T> func)
        {
            (count >= 0).AssertTrue();
            for (var i = 0; i < count; ++i)
            {
                yield return func(i);
            }
        }

        // see Seq.Infinite for TimesYield of infinite length
        // we can't make such methods to be extensions ones since it will be unusable

        // DUPLICATE ACTIONS

        public static void TimesDo(this int count, Action action)
        {
            (count >= 0).AssertTrue();
            for (var i = 0; i < count; ++i)
            {
                action();
            }
        }

        public static void TimesDo(this int count, Action<int> action)
        {
            (count >= 0).AssertTrue();
            for (var i = 0; i < count; ++i)
            {
                action(i);
            }
        }
    }
}