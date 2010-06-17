using System;
using System.Collections.Generic;
using System.Linq;
using XenoGears.Assertions;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<Tuple<T, T>> Slide2<T>(this IEnumerable<T> seq)
        {
            return seq.SlideInner2();
        }

        public static IEnumerable<Tuple<T, T, T>> Slide3<T>(this IEnumerable<T> seq)
        {
            return seq.SlideInner3();
        }

        public static IEnumerable<ITuple> Slide<T>(this IEnumerable<T> seq, int windowWidth)
        {
            return seq.SlideInner(windowWidth);
        }
        
        public static IEnumerable<Tuple<T, T>> SlideInner2<T>(this IEnumerable<T> seq)
        {
            return seq.SlideInner(2).Cast<Tuple<T, T>>();
        }

        public static IEnumerable<Tuple<T, T, T>> SlideInner3<T>(this IEnumerable<T> seq)
        {
            return seq.SlideInner(3).Cast<Tuple<T, T, T>>();
        }

        public static IEnumerable<ITuple> SlideInner<T>(this IEnumerable<T> seq, int windowWidth)
        {
            (seq.Count() >= windowWidth).AssertTrue();

            var window = seq.Take(windowWidth);
            yield return Tuple.New(
                window.Cast<Object>().ToArray(),
                windowWidth.Times(typeof(T)).ToArray());

            foreach (var el in seq.Skip(windowWidth))
            {
                window = window.ShiftLeft(1, el);
                yield return Tuple.New(
                    window.Cast<Object>().ToArray(),
                    windowWidth.Times(typeof(T)).ToArray());
            }
        }

        public static IEnumerable<Tuple<T, T>> SlideOuter2<T>(this IEnumerable<T> seq)
        {
            return seq.SlideOuter(2).Cast<Tuple<T, T>>();
        }

        public static IEnumerable<Tuple<T, T, T>> SlideOuter3<T>(this IEnumerable<T> seq)
        {
            return seq.SlideOuter(3).Cast<Tuple<T, T, T>>();
        }

        public static IEnumerable<ITuple> SlideOuter<T>(this IEnumerable<T> seq, int windowWidth)
        {
            var window = windowWidth.Times(default(T));
            foreach (var el in seq)
            {
                window = window.ShiftLeft(1, el);
                yield return Tuple.New(
                    window.Cast<Object>().ToArray(),
                    windowWidth.Times(typeof(T)).ToArray());
            }

            for (int i = 0; i < windowWidth - 1; ++i)
            {
                window = window.ShiftLeft(1, default(T));
                yield return Tuple.New(
                    window.Cast<Object>().ToArray(),
                    windowWidth.Times(typeof(T)).ToArray());
            }
        }
    }
}
