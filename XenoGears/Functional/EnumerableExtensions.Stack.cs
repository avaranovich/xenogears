using System;
using System.Collections.Generic;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static T PopOrDefault<T>(this Stack<T> stack)
        {
            return stack.IsEmpty() ? default(T) : stack.Pop();
        }

        public static T PopOrDefault<T>(this Stack<T> stack, T @default)
        {
            return stack.IsEmpty() ? @default : stack.Pop();
        }

        public static T PopOrDefault<T>(this Stack<T> stack, Func<T> @default)
        {
            return stack.IsEmpty() ? @default() : stack.Pop();
        }

        public static T PeekOrDefault<T>(this Stack<T> stack)
        {
            return stack.IsEmpty() ? default(T) : stack.Peek();
        }

        public static T PeekOrDefault<T>(this Stack<T> stack, T @default)
        {
            return stack.IsEmpty() ? @default : stack.Peek();
        }

        public static T PeekOrDefault<T>(this Stack<T> stack, Func<T> @default)
        {
            return stack.IsEmpty() ? @default() : stack.Peek();
        }
    }
}
