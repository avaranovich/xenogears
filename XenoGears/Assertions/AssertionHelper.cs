using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using XenoGears.Functional;

namespace XenoGears.Assertions
{
    // todo. this class is one step implementing business rules approach
    // just add BRE to Exceptions and introduce overloads with BRE codes for assertion statements
    // assertion context can be passed via params Object[] or Dictionary<String, Object> parameter

    // todo. search and replace Single with AssertSingle, First with AssertFirst and likes
    // (this also includes Last and AssertLast)
    // also split the AssertionHelper into lots of partials just like EnumerableExtensions

    [DebuggerNonUserCode]
    public static class AssertionHelper
    {
        public static T AssertThat<T>(this T subject, Func<T, bool> assertion)
        {
            if (!assertion(subject))
            {
                throw new AssertionFailedException("An assertion has failed");
            }

            return subject;
        }

        public static IEnumerable<T> AssertThat<T>(this IEnumerable<T> subject, Func<T, bool> assertion)
        {
            subject.ForEach(el =>
            {
                if (!assertion(el))
                {
                    throw new AssertionFailedException("An assertion has failed");
                }
            });

            return subject;
        }

        public static T AssertNotNull<T>(this T obj)
            where T : class
        {
            if (obj == null)
            {
                throw new AssertionFailedException("This should be not null");
            }

            return obj;
        }

        public static T? AssertNotNull<T>(this T? obj)
            where T : struct
        {
            if (obj == null)
            {
                throw new AssertionFailedException("This should be not null");
            }

            return obj;
        }

        public static T AssertValue<T>(this T? obj)
            where T : struct
        {
            if (obj == null)
            {
                throw new AssertionFailedException("This should be not null");
            }

            return obj.Value;
        }

        public static T AssertNull<T>(this T obj)
            where T : class
        {
            if (obj != null)
            {
                throw new AssertionFailedException("This should be null");
            }

            return obj;
        }

        public static T? AssertNull<T>(this T? obj)
            where T : struct
        {
            if (obj != null)
            {
                throw new AssertionFailedException("This should be null");
            }

            return obj;
        }

        public static T AssertEmpty<T>(this T obj)
            where T : IEnumerable
        {
            if (obj.IsNotEmpty())
            {
                throw new AssertionFailedException("This should be empty");
            }

            return obj;
        }

        public static T AssertNotEmpty<T>(this T obj)
            where T : IEnumerable
        {
            if (obj.IsEmpty())
            {
                throw new AssertionFailedException("This should be not empty");
            }

            return obj;
        }

        public static T AssertNeitherNullNorEmpty<T>(this T obj)
            where T : IEnumerable
        {
            if (obj.IsNullOrEmpty())
            {
                throw new AssertionFailedException("This should be neither null nor empty");
            }

            return obj;
        }

        public static T AssertNullOrEmpty<T>(this T obj)
            where T : IEnumerable
        {
            if (obj.IsNeitherNullNorEmpty())
            {
                throw new AssertionFailedException("This should be null or empty");
            }

            return obj;
        }

        public static IEnumerable<T> AssertCount<T>(this IEnumerable<T> seq, int expected)
        {
            var actual = seq.Count();
            if (actual != expected)
            {
                throw new AssertionFailedException(String.Format(
                    "Expected {0} element(s), actually found {1}", expected, actual));
            }

            return seq;
        }

        public static IEnumerable<T> AssertCountIsNot<T>(this IEnumerable<T> seq, int expected)
        {
            var actual = seq.Count();
            if (actual == expected)
            {
                throw new AssertionFailedException(String.Format(
                    "Expected anything but {0} element(s)", expected));
            }

            return seq;
        }

        public static bool AssertFalse(this bool obj)
        {
            if (obj)
            {
                throw new AssertionFailedException("This should be false");
            }

            return false;
        }

        public static bool AssertTrue(this bool obj)
        {
            if (!obj)
            {
                throw new AssertionFailedException("This should be true");
            }

            return true;
        }

        public static Exception Fail()
        {
            throw new AssertionFailedException("This should not happen");
        }

        public static Exception Fail(Exception ex)
        {
            throw new AssertionFailedException("This should not happen", ex);
        }

        public static T Fail<T>(this T ctx)
        {
            throw new AssertionFailedException("This should not happen");
        }

        public static T Fail<T>(this T ctx, Exception ex)
        {
            throw new AssertionFailedException("This should not happen", ex);
        }

        public static T AssertCast<T>(this Object o)
        {
            try
            {
                return (T)o;
            }
            catch (InvalidCastException ice)
            {
                throw new AssertionFailedException(String.Format(
                   "Object '{0}' was expected to be of type '{1}' but had type '{2}'", 
                   o, typeof(T), o == null ? "null" : o.GetType().ToString()), ice);
            } 
        }

        public static T AssertCoerce<T>(this Object o)
        {
            try
            {
                // works around a dreaded cast-from-boxed issue
                // example: (long)(object)(int)o will crash
                if (o != null && o.GetType().IsValueType && typeof(T).IsValueType)
                {
                    var arg_src = Expression.Parameter(typeof(Object), "source");
                    var unboxAndCast = Expression.Lambda<Func<Object, T>>(
                        Expression.Convert(Expression.Convert(arg_src, o.GetType()), typeof(T)),
                        arg_src).Compile();

                    return unboxAndCast(o);
                }
                else
                {
                    return (T)o;
                }
            }
            catch (InvalidCastException ice)
            {
                throw new AssertionFailedException(String.Format(
                   "Object '{0}' was expected to be of type '{1}' but had type '{2}'", 
                   o, typeof(T), o == null ? "null" : o.GetType().ToString()), ice);
            } 
        }

        // use this signature when an argument is enumerable, 
        // but it's undesirable to use the following signature: 
        // IEnumerable<T> AssertCast<T>(this IEnumerable e)
        public static T AssertCast2<T>(this IEnumerable o)
        {
            return ((Object)o).AssertCast<T>();
        }

        public static IEnumerable<T> AssertCast<T>(this IEnumerable e)
        {
            try
            {
                return e == null ? null : e.Cast<T>();
            }
            catch (InvalidCastException ice)
            {
                throw new AssertionFailedException(String.Format(
                   "Object '{0}' was expected to be of type IEnumerable<'{1}'> but had type '{2}'",
                   e, typeof(T), e == null ? "null" : e.GetType().ToString()), ice);
            }
        }

        public static Type AssertCast<T>(this Type t)
        {
            // todo. this implementation is very incomplete
            if (typeof(T).IsAssignableFrom(t))
            {
                return t;
            }
            else
            {
                throw new AssertionFailedException(String.Format(
                   "Type '{0}' was expected to be convertible to type '{1}'",
                   t, typeof(T)));
            }
        }

        public static V AssertGetValue<K, V>(this IDictionary<K, V> map, K key)
        {
            if (!map.ContainsKey(key))
            {
                throw new AssertionFailedException(String.Format(
                    "Expected map to contain key '{0}'", key));
            }
            else
            {
                return map[key];
            }
        }

        public static T AssertSingle<T>(this IEnumerable<T> seq)
        {
            var actual = seq.Count();
            if (actual != 1)
            {
                throw new AssertionFailedException(String.Format(
                    "Expected a single element in a sequence, actually found {0} elements", actual));
            }

            return seq.Single();
        }

        public static T AssertSingle<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return seq.Where(filter).AssertSingle();
        }

        public static T AssertSingleOrDefault<T>(this IEnumerable<T> seq)
        {
            var actual = seq.Count();
            if (actual > 1)
            {
                throw new AssertionFailedException(String.Format(
                    "Expected a single or default element in a sequence, actually found {0} elements", actual));
            }

            return seq.SingleOrDefault();
        }

        public static T AssertSingleOrDefault<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return seq.Where(filter).AssertSingleOrDefault();
        }

        public static bool AssertImplies(this bool b1, bool b2)
        {
            if (b1 && !b2)
            {
                throw new AssertionFailedException("b1 should imply b2");
            }

            return b1;
        }

        public static bool AssertEquiv(this bool b1, bool b2)
        {
            b2.AssertImplies(b1);
            return b1.AssertImplies(b2);
        }

        public static IEnumerable<T> AssertEach<T>(this IEnumerable<T> seq, Func<T, bool> assertion)
        {
            var thunk = new AssertEach_Thunk<T>(assertion);
            seq.ForEach(thunk.Snippet);
            return seq;
        }

        [DebuggerNonUserCode]
        private class AssertEach_Thunk<T>
        {
            private readonly Func<T, bool> _assertion;
            public AssertEach_Thunk(Func<T, bool> assertion) { _assertion = assertion; }

            public void Snippet(T el)
            {
                _assertion(el).AssertTrue();
            }
        }

        private static void AssertEach_Assert<T>(T el, Func<T, bool> assertion)
        {
            assertion(el).AssertTrue();
        }

        public static IEnumerable<T> AssertAll<T>(this IEnumerable<T> seq, Func<T, bool> assertion)
        {
            seq.Select(assertion).All().AssertTrue();
            return seq;
        }

        public static IEnumerable<T> AssertAny<T>(this IEnumerable<T> seq, Func<T, bool> assertion)
        {
            seq.Select(assertion).Any().AssertTrue();
            return seq;
        }

        public static IEnumerable<T> AssertNone<T>(this IEnumerable<T> seq, Func<T, bool> assertion)
        {
            seq.Select(assertion).None().AssertTrue();
            return seq;
        }

        public static Match AssertMatch(this String input, String pattern)
        {
            var m = Regex.Match(input, pattern);
            if (!m.Success)
            {
                throw new AssertionFailedException(String.Format(
                    "Expected that \"{0}\" would match the \"{1}\" pattern", input, pattern));
            }

            return m;
        }

        public static Dictionary<String, String> AssertParse(this String input, String pattern)
        {
            var names = new List<String>();
            var m_meta = Regex.Match(pattern, @"\(\?\<(?<name>.*?)\>");
            for (; m_meta.Success; m_meta = m_meta.NextMatch())
            {
                var name = m_meta.Result("${name}");
                names.Add(name);
            }

            var m = input.AssertMatch(pattern);
            return names.ToDictionary(name => name, name => m.Result("${" + name + "}"));
        }

        public static String AssertExtract(this String input, String pattern)
        {
            return input.AssertParse(pattern).AssertSingle().Value;
        }

        public static T AssertFirst<T>(this IEnumerable<T> seq)
        {
            var actual = seq.Count();
            if (actual < 1)
            {
                throw new AssertionFailedException(String.Format(
                    "Expected at least one element in a sequence, actually found {0} elements", actual));
            }

            return seq.First();
        }

        public static T AssertFirst<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return seq.Where(filter).AssertFirst();
        }

        public static T AssertSecond<T>(this IEnumerable<T> seq)
        {
            var actual = seq.Count();
            if (actual < 2)
            {
                throw new AssertionFailedException(String.Format(
                    "Expected at least two elements in a sequence, actually found {0} elements", actual));
            }

            return seq.Second();
        }

        public static T AssertSecond<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return seq.Where(filter).AssertSecond();
        }

        public static T AssertThird<T>(this IEnumerable<T> seq)
        {
            var actual = seq.Count();
            if (actual < 3)
            {
                throw new AssertionFailedException(String.Format(
                    "Expected at least three elements in a sequence, actually found {0} elements", actual));
            }

            return seq.Third();
        }

        public static T AssertThird<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return seq.Where(filter).AssertThird();
        }

        public static T AssertFourth<T>(this IEnumerable<T> seq)
        {
            var actual = seq.Count();
            if (actual < 4)
            {
                throw new AssertionFailedException(String.Format(
                    "Expected at least four elements in a sequence, actually found {0} elements", actual));
            }

            return seq.Fourth();
        }

        public static T AssertFourth<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return seq.Where(filter).AssertFourth();
        }

        public static T AssertFifth<T>(this IEnumerable<T> seq)
        {
            var actual = seq.Count();
            if (actual < 5)
            {
                throw new AssertionFailedException(String.Format(
                    "Expected at least five elements in a sequence, actually found {0} elements", actual));
            }

            return seq.Fifth();
        }

        public static T AssertFifth<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return seq.Where(filter).AssertFifth();
        }

        public static T AssertLast<T>(this IEnumerable<T> seq)
        {
            var actual = seq.Count();
            if (actual < 1)
            {
                throw new AssertionFailedException(String.Format(
                    "Expected at least one element in a sequence, actually found {0} elements", actual));
            }

            return seq.Last();
        }

        public static T AssertLast<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return seq.Where(filter).AssertLast();
        }
    }
}