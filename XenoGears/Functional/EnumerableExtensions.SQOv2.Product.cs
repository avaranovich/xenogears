using System;
using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static decimal Product(this IEnumerable<decimal> source)
        {
            decimal num = 1M;
            foreach (decimal num2 in source)
            {
                num *= num2;
            }
            return num;
        }

        public static float Product(this IEnumerable<float> source)
        {
            double num = 1.0;
            foreach (float num2 in source)
            {
                num *= num2;
            }
            return (float)num;
        }

        public static double Product(this IEnumerable<double> source)
        {
            double num = 1.0;
            foreach (double num2 in source)
            {
                num *= num2;
            }
            return num;
        }

        public static int Product(this IEnumerable<int> source)
        {
            int num = 1;
            foreach (int num2 in source)
            {
                num *= num2;
            }
            return num;
        }

        public static uint Product(this IEnumerable<uint> source)
        {
            uint num = 1u;
            foreach (uint num2 in source)
            {
                num *= num2;
            }
            return num;
        }

        public static long Product(this IEnumerable<long> source)
        {
            long num = 1L;
            foreach (long num2 in source)
            {
                num *= num2;
            }
            return num;
        }

        public static ulong Product(this IEnumerable<ulong> source)
        {
            ulong num = 1u;
            foreach (ulong num2 in source)
            {
                num *= num2;
            }
            return num;
        }

        public static decimal Product<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select(selector).Product();
        }

        public static float Product<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select(selector).Product();
        }

        public static double Product<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select(selector).Product();
        }

        public static int Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select(selector).Product();
        }

        public static uint Product<TSource>(this IEnumerable<TSource> source, Func<TSource, uint> selector)
        {
            return source.Select(selector).Product();
        }

        public static long Product<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select(selector).Product();
        }

        public static ulong Product<TSource>(this IEnumerable<TSource> source, Func<TSource, ulong> selector)
        {
            return source.Select(selector).Product();
        }
    }
}
