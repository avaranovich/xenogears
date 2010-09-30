using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using XenoGears.Assertions;

namespace XenoGears.Strings
{
    [DebuggerNonUserCode]
    public static class ToFromStringHelper
    {
        public static bool SupportsSerializationToString<T>()
        {
            return SupportsSerializationToString(typeof(T));
        }

        public static bool SupportsSerializationToString(this Type t)
        {
            if (t == null) return false;
            var converter = TypeDescriptor.GetConverter(t).AssertNotNull();
            return converter.CanConvertTo(typeof(String)) && converter.CanConvertFrom(typeof(String));
        }

        public static T FromInvariantString<T>(this String s)
        {
            return s.FromInvariantString<T>();
        }

        public static T FromInvariantString<T>(this String s, String format)
        {
            return (T)FromInvariantString(typeof(T), s);
        }

        public static Object FromInvariantString(this Type t, String s)
        {
            return t.FromFormattedString(s, CultureInfo.InvariantCulture);
        }

        public static Object FromInvariantString(this Type t, String s, String format)
        {
            return t.FromFormattedString(s, format, CultureInfo.InvariantCulture);
        }

        public static Object FromInvariantString(this String s, Type t)
        {
            return t.FromFormattedString(s, CultureInfo.InvariantCulture);
        }

        public static Object FromInvariantString(this String s, Type t, String format)
        {
            return t.FromFormattedString(s, format, CultureInfo.InvariantCulture);
        }

        public static String ToInvariantString(this Object @object)
        {
            return @object.ToFormattedString(CultureInfo.InvariantCulture);
        }

        public static String ToInvariantString(this Object @object, String format)
        {
            return @object.ToFormattedString(format, CultureInfo.InvariantCulture);
        }

        public static T FromLocalString<T>(this String s, CultureInfo locale)
        {
            return (T)FromLocalString(typeof(T), s, locale);
        }

        public static T FromLocalString<T>(this String s, String format, CultureInfo locale)
        {
            return (T)FromLocalString(typeof(T), s, format, locale);
        }

        public static Object FromLocalString(this Type t, String s, CultureInfo locale)
        {
            return t.FromFormattedString(s, locale);
        }

        public static Object FromLocalString(this Type t, String s, String format, CultureInfo locale)
        {
            return t.FromFormattedString(s, format, locale);
        }

        public static Object FromLocalString(this String s, Type t, CultureInfo locale)
        {
            return t.FromFormattedString(s, locale);
        }

        public static Object FromLocalString(this String s, Type t, String format, CultureInfo locale)
        {
            return t.FromFormattedString(s, format, locale);
        }

        public static String ToLocalString(this Object @object, CultureInfo locale)
        {
            return @object.ToFormattedString(locale);
        }

        public static String ToLocalString(this Object @object, String format, CultureInfo locale)
        {
            return @object.ToFormattedString(format, locale);
        }

        public static T FromFormattedString<T>(this String s, String format)
        {
            return (T)FromFormattedString(typeof(T), s, format, CultureInfo.InvariantCulture);
        }

        public static Object FromFormattedString(this Type t, String s, String format)
        {
            return t.FromFormattedString(s, format, CultureInfo.InvariantCulture);
        }

        public static Object FromFormattedString(this String s, Type t, String format)
        {
            return t.FromFormattedString(s, format, CultureInfo.InvariantCulture);
        }

        public static String ToFormattedString(this Object @object, String format)
        {
            return @object.ToFormattedString(format, CultureInfo.InvariantCulture);
        }

        public static T FromFormattedString<T>(this String s, String format, IFormatProvider provider)
        {
            return (T)FromFormattedString(typeof(T), s, format, provider);
        }

        public static Object FromFormattedString(this Type t, String s, IFormatProvider provider)
        {
            if (s == null)
            {
                return null;
            }
            else
            {
                if (provider is CultureInfo)
                {
                    var locale = (CultureInfo)provider;
                    var converter = TypeDescriptor.GetConverter(t).AssertNotNull();
                    if (converter.CanConvertFrom(typeof(String)))
                    {
                        return converter.ConvertFromString(null, locale, s);
                    }
                    else
                    {
                        throw new NotSupportedException(t.ToString());
                    }
                }
                else
                {
                    // todo. find out the right way to do this
                    var m_parse = t.GetMethod("Parse", new []{typeof(String), typeof(IFormatProvider)});
                    m_parse = m_parse ?? t.GetMethod("ParseExact", new[]{typeof(String), typeof(IFormatProvider)});
                    if (m_parse != null && m_parse.IsStatic)
                    {
                        return m_parse.Invoke(null, new Object[]{s, provider});
                    }
                    else
                    {
                        throw new NotSupportedException(t.ToString());
                    }
                }
            }
        }

        public static Object FromFormattedString(this String s, Type t, IFormatProvider provider)
        {
            return t.FromFormattedString(s, provider);
        }

        public static Object FromFormattedString(this Type t, String s, String format, IFormatProvider provider)
        {
            if (s == null)
            {
                return null;
            }
            else
            {
                // todo. find out the right way to do this
                var m_parse = t.GetMethod("Parse", new []{typeof(String), typeof(String), typeof(IFormatProvider)});
                m_parse = m_parse ?? t.GetMethod("ParseExact", new[]{typeof(String), typeof(String), typeof(IFormatProvider)});
                if (m_parse != null && m_parse.IsStatic)
                {
                    return m_parse.Invoke(null, new Object[]{s, format, provider});
                }
                else
                {
                    throw new NotSupportedException(t.ToString());
                }
            }
        }

        public static Object FromFormattedString(this String s, Type t, String format, IFormatProvider provider)
        {
            return t.FromFormattedString(s, format, provider);
        }

        public static String ToFormattedString(this Object @object, IFormatProvider provider)
        {
            if (@object == null)
            {
                return null;
            }
            else
            {
                if (provider is CultureInfo)
                {
                    var locale = (CultureInfo)provider;
                    var converter = TypeDescriptor.GetConverter(@object).AssertNotNull();
                    if (converter.CanConvertTo(typeof(String)))
                    {
                        return converter.ConvertToString(null, locale, @object);
                    }
                    else
                    {
                        return @object.ToString();
                    }
                }
                else
                {
                    // todo. find out the right way to do this
                    var m_tostring = @object.GetType().GetMethod("ToString", new []{typeof(IFormatProvider)});
                    if (m_tostring != null && !m_tostring.IsStatic)
                    {
                        return m_tostring.Invoke(@object, new Object[]{provider}).AssertCast<String>();
                    }
                    else
                    {
                        throw new NotSupportedException(@object.GetType().ToString());
                    }
                }
            }
        }

        public static String ToFormattedString(this Object @object, String format, IFormatProvider provider)
        {
            if (@object == null)
            {
                return null;
            }
            else
            {
                // todo. find out the right way to do this
                var m_tostring = @object.GetType().GetMethod("ToString", new []{typeof(String), typeof(IFormatProvider)});
                if (m_tostring != null && !m_tostring.IsStatic)
                {
                    return m_tostring.Invoke(@object, new Object[]{format, provider}).AssertCast<String>();
                }
                else
                {
                    throw new NotSupportedException(@object.GetType().ToString());
                }
            }
        }
    }
}