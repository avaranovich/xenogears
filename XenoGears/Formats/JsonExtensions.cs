using System;
using System.IO;
using System.Reflection;

namespace XenoGears.Formats
{
    public static class JsonExtensions
    {
        public static dynamic ParseJson(this String url)
        {
            return Json.Parse(url);
        }

        public static dynamic ParseJsonOrDefault(this String url)
        {
            return Json.ParseOrDefault(url);
        }

        public static dynamic ParseJsonOrDefault(this String url, dynamic json)
        {
            return Json.ParseOrDefault(url);
        }

        public static dynamic ParseJsonOrDefault(this String url, Func<dynamic> json)
        {
            return Json.ParseOrDefault(url);
        }

        public static dynamic LoadJson(this String url)
        {
            return Json.Load(url);
        }

        public static dynamic LoadJsonOrDefault(this String url)
        {
            return Json.LoadOrDefault(url);
        }

        public static dynamic LoadJsonOrDefault(this String url, dynamic json)
        {
            return Json.LoadOrDefault(url);
        }

        public static dynamic LoadJsonOrDefault(this String url, Func<dynamic> json)
        {
            return Json.LoadOrDefault(url);
        }

        public static dynamic ReadJson(this Stream s)
        {
            return Json.Read(s);
        }

        public static dynamic ReadJsonOrDefault(this Stream s)
        {
            return Json.ReadOrDefault(s);
        }

        public static dynamic ReadJsonOrDefault(this Stream s, dynamic json)
        {
            return Json.ReadOrDefault(s);
        }

        public static dynamic ReadJsonOrDefault(this Stream s, Func<dynamic> json)
        {
            return Json.ReadOrDefault(s);
        }

        public static dynamic ReadJson(this TextReader w)
        {
            return Json.Read(w);
        }

        public static dynamic ReadJsonOrDefault(this TextReader w)
        {
            return Json.ReadOrDefault(w);
        }

        public static dynamic ReadJsonOrDefault(this TextReader w, dynamic json)
        {
            return Json.ReadOrDefault(w);
        }

        public static dynamic ReadJsonOrDefault(this TextReader w, Func<dynamic> json)
        {
            return Json.ReadOrDefault(w);
        }

        public static void SaveJson(this String url, dynamic json)
        {
            new Json(json).Save(url);
        }

        public static void WriteJson(this Stream s, dynamic json)
        {
            new Json(json).Write(s);
        }

        public static void WriteJson(this TextWriter w, dynamic json)
        {
            new Json(json).Write(w);
        }

        public static dynamic ToJson(this Object value)
        {
            return new Json(value);
        }

        public static dynamic ToJson(this Object value, Type descriptor)
        {
            return new Json(value, descriptor);
        }

        public static dynamic ToJson(this Object value, PropertyInfo descriptor)
        {
            return new Json(value, descriptor);
        }

        public static dynamic ToJson(this Object value, MemberInfo descriptor)
        {
            return new Json(value, descriptor);
        }

        public static T ToObject<T>(this Json json)
        {
            return json.Deserialize<T>();
        }

        public static T ToObject<T>(this Json json, T pattern)
        {
            return json.Deserialize(pattern);
        }

        public static Object ToObject(this Json json, Type descriptor)
        {
            return json.Deserialize(descriptor);
        }

        public static Object ToObject(this Json json, PropertyInfo descriptor)
        {
            return json.Deserialize(descriptor);
        }

        public static Object ToObject(this Json json, MemberInfo descriptor)
        {
            return json.Deserialize(descriptor);
        }
    }
}