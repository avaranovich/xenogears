using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static String StringJoin(this IEnumerable objects)
        {
            return objects.StringJoin(", ");
        }

        public static String StringJoin(this IEnumerable objects, String delim)
        {
            return objects.Cast<Object>().Select(@object => "" + @object).StringJoin(delim);
        }

        private static String StringJoin(this IEnumerable<String> strings, String delim)
        {
            return String.Join(delim, strings.ToArray());
        }

        public static String StringJoin<T>(this T[] arr)
        {
            return arr.StringJoin(" ");
        }

        public static String StringJoin<T>(this T[] arr, String el_delim)
        {
            return ((IEnumerable<T>)arr).StringJoin(el_delim);
        }

        public static String StringJoin<T>(this T[,] arr)
        {
            return arr.StringJoin(" ", Environment.NewLine);
        }

        public static String StringJoin<T>(this T[,] arr, String el_delim, String row_delim)
        {
            // todo #1. align all values within a column
            // todo #2. add row and column numbers

            var buf = new StringBuilder();
            for (var i = 0; i < arr.Height(); i++)
            {
                for (var j = 0; j < arr.Width(); j++)
                {
                    buf.Append(arr[i, j]);
                    if (j < arr.Width() - 1) buf.Append(el_delim);
                }
                if (i < arr.Height() - 1) buf.Append(row_delim);
            }

            return buf.ToString();
        }

        public static String StringJoin<T>(this T[,,] arr)
        {
            return arr.StringJoin(" ", Environment.NewLine, Environment.NewLine + Environment.NewLine);
        }

        public static String StringJoin<T>(this T[,,] arr, String el_delim, String row_delim, String matrix_delim)
        {
            throw new NotImplementedException();
        }
    }
}