﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using XenoGears.Web.Urls;

namespace XenoGears.Formats
{
    [DebuggerNonUserCode]
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

        public static dynamic ParseJsonOrDefault(this String url, Object json)
        {
            return Json.ParseOrDefault(url, json);
        }

        public static dynamic ParseJsonOrDefault(this String url, Func<Object> json)
        {
            return Json.ParseOrDefault(url, json);
        }

        public static dynamic LoadJson(this String url)
        {
            return Json.Load(url, null as ICredentials, null as Json);
        }

        public static dynamic LoadJson(this String url, Json args, ICredentials credentials = null)
        {
            return Json.Load(url, credentials, args);
        }

        public static dynamic LoadJson(this String url, ICredentials credentials, Json args = null)
        {
            return Json.Load(url, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this String url)
        {
            return Json.LoadOrDefault(url, null as ICredentials, null as Json);
        }

        public static dynamic LoadJsonOrDefault(this String url, Json args, ICredentials credentials = null)
        {
            return Json.LoadOrDefault(url, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this String url, ICredentials credentials, Json args = null)
        {
            return Json.LoadOrDefault(url, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this String url, Object json)
        {
            return Json.LoadOrDefault(url, json, null as ICredentials, null as Json);
        }

        public static dynamic LoadJsonOrDefault(this String url, Object json, Json args, ICredentials credentials = null)
        {
            return Json.LoadOrDefault(url, json, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this String url, Object json, ICredentials credentials, Json args = null)
        {
            return Json.LoadOrDefault(url, json, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this String url, Func<Object> json)
        {
            return Json.LoadOrDefault(url, json, null as ICredentials, null as Json);
        }

        public static dynamic LoadJsonOrDefault(this String url, Func<Object> json, Json args, ICredentials credentials = null)
        {
            return Json.LoadOrDefault(url, json, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this String url, Func<Object> json, ICredentials credentials, Json args = null)
        {
            return Json.LoadOrDefault(url, json, credentials, args);
        }

        public static dynamic LoadJson(this FileInfo url)
        {
            return Json.Load(url == null ? null : url.FullName);
        }

        public static dynamic LoadJsonOrDefault(this FileInfo url)
        {
            return Json.LoadOrDefault(url == null ? null : url.FullName);
        }

        public static dynamic LoadJsonOrDefault(this FileInfo url, Object json)
        {
            return Json.LoadOrDefault(url == null ? null : url.FullName, json);
        }

        public static dynamic LoadJsonOrDefault(this FileInfo url, Func<Object> json)
        {
            return Json.LoadOrDefault(url == null ? null : url.FullName, json);
        }

        public static dynamic LoadJson(this Uri url)
        {
            return Json.Load(url == null ? null : url.AbsoluteUri, null as ICredentials, null as Json);
        }

        public static dynamic LoadJson(this Uri url, Json args, ICredentials credentials = null)
        {
            return Json.Load(url == null ? null : url.AbsoluteUri, credentials, args);
        }

        public static dynamic LoadJson(this Uri url, ICredentials credentials, Json args = null)
        {
            return Json.Load(url == null ? null : url.AbsoluteUri, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Uri url)
        {
            return Json.LoadOrDefault(url == null ? null : url.AbsoluteUri, null as ICredentials, null as Json);
        }

        public static dynamic LoadJsonOrDefault(this Uri url, Json args, ICredentials credentials = null)
        {
            return Json.LoadOrDefault(url == null ? null : url.AbsoluteUri, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Uri url, ICredentials credentials, Json args = null)
        {
            return Json.LoadOrDefault(url == null ? null : url.AbsoluteUri, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Uri url, Object json)
        {
            return Json.LoadOrDefault(url == null ? null : url.AbsoluteUri, json, null as ICredentials, null as Json);
        }

        public static dynamic LoadJsonOrDefault(this Uri url, Object json, Json args, ICredentials credentials = null)
        {
            return Json.LoadOrDefault(url == null ? null : url.AbsoluteUri, json, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Uri url, Object json, ICredentials credentials, Json args = null)
        {
            return Json.LoadOrDefault(url == null ? null : url.AbsoluteUri, json, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Uri url, Func<Object> json)
        {
            return Json.LoadOrDefault(url == null ? null : url.AbsoluteUri, json, null as ICredentials, null as Json);
        }

        public static dynamic LoadJsonOrDefault(this Uri url, Func<Object> json, Json args, ICredentials credentials = null)
        {
            return Json.LoadOrDefault(url == null ? null : url.AbsoluteUri, json, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Uri url, Func<Object> json, ICredentials credentials, Json args = null)
        {
            return Json.LoadOrDefault(url == null ? null : url.AbsoluteUri, json, credentials, args);
        }

        public static dynamic LoadJson(this Url url)
        {
            return Json.Load(url, null as ICredentials, null as Json);
        }

        public static dynamic LoadJson(this Url url, Json args, ICredentials credentials = null)
        {
            return Json.Load(url, credentials, args);
        }

        public static dynamic LoadJson(this Url url, ICredentials credentials, Json args = null)
        {
            return Json.Load(url, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Url url)
        {
            return Json.LoadOrDefault(url, null as ICredentials, null as Json);
        }

        public static dynamic LoadJsonOrDefault(this Url url, Json args, ICredentials credentials = null)
        {
            return Json.LoadOrDefault(url, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Url url, ICredentials credentials, Json args = null)
        {
            return Json.LoadOrDefault(url, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Url url, Object json)
        {
            return Json.LoadOrDefault(url, json, null as ICredentials, null as Json);
        }

        public static dynamic LoadJsonOrDefault(this Url url, Object json, Json args, ICredentials credentials = null)
        {
            return Json.LoadOrDefault(url, json, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Url url, Object json, ICredentials credentials, Json args = null)
        {
            return Json.LoadOrDefault(url, json, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Url url, Func<Object> json)
        {
            return Json.LoadOrDefault(url, json, null as ICredentials, null as Json);
        }

        public static dynamic LoadJsonOrDefault(this Url url, Func<Object> json, Json args, ICredentials credentials = null)
        {
            return Json.LoadOrDefault(url, json, credentials, args);
        }

        public static dynamic LoadJsonOrDefault(this Url url, Func<Object> json, ICredentials credentials, Json args = null)
        {
            return Json.LoadOrDefault(url, json, credentials, args);
        }

        public static dynamic ReadJson(this Stream s)
        {
            return Json.Read(s);
        }

        public static dynamic ReadJsonOrDefault(this Stream s)
        {
            return Json.ReadOrDefault(s);
        }

        public static dynamic ReadJsonOrDefault(this Stream s, Object json)
        {
            return Json.ReadOrDefault(s, json);
        }

        public static dynamic ReadJsonOrDefault(this Stream s, Func<Object> json)
        {
            return Json.ReadOrDefault(s, json);
        }

        public static dynamic ReadJson(this TextReader w)
        {
            return Json.Read(w);
        }

        public static dynamic ReadJsonOrDefault(this TextReader w)
        {
            return Json.ReadOrDefault(w);
        }

        public static dynamic ReadJsonOrDefault(this TextReader w, Object json)
        {
            return Json.ReadOrDefault(w, json);
        }

        public static dynamic ReadJsonOrDefault(this TextReader w, Func<Object> json)
        {
            return Json.ReadOrDefault(w, json);
        }

        public static void SaveJson(this String url, Object json, ICredentials credentials = null)
        {
            new Json(json).Save(url, credentials);
        }

        public static void SaveJson(this FileInfo url, Object json, ICredentials credentials = null)
        {
            new Json(json).Save(url == null ? null : url.FullName, credentials);
        }

        public static void SaveJson(this Uri url, Object json, ICredentials credentials = null)
        {
            new Json(json).Save(url.AbsoluteUri, credentials);
        }

        public static void SaveJson(this Url url, Object json, ICredentials credentials = null)
        {
            new Json(json).Save(url, credentials);
        }

        public static void WriteJson(this Stream s, Object json)
        {
            new Json(json).Write(s);
        }

        public static void WriteJson(this TextWriter w, Object json)
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