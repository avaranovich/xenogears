using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using XenoGears.Web.Urls;

namespace XenoGears.Formats
{
    [DebuggerNonUserCode]
    public static class XmlExtensions
    {
        public static dynamic ParseXml(this String url)
        {
            return Xml.Parse(url);
        }

        public static dynamic ParseXmlOrDefault(this String url)
        {
            return Xml.ParseOrDefault(url);
        }

        public static dynamic ParseXmlOrDefault(this String url, XDocument xml)
        {
            return Xml.ParseOrDefault(url, xml);
        }

        public static dynamic ParseXmlOrDefault(this String url, Func<XDocument> xml)
        {
            return Xml.ParseOrDefault(url, xml);
        }

        public static dynamic LoadXml(this String url)
        {
            return Xml.Load(url);
        }

        public static dynamic LoadXmlOrDefault(this String url)
        {
            return Xml.LoadOrDefault(url);
        }

        public static dynamic LoadXmlOrDefault(this String url, XDocument xml)
        {
            return Xml.LoadOrDefault(url, xml);
        }

        public static dynamic LoadXmlOrDefault(this String url, Func<XDocument> xml)
        {
            return Xml.LoadOrDefault(url, xml);
        }

        public static dynamic LoadXml(this FileInfo url)
        {
            return Xml.Load(url == null ? null : url.FullName);
        }

        public static dynamic LoadXmlOrDefault(this FileInfo url)
        {
            return Xml.LoadOrDefault(url == null ? null : url.FullName);
        }

        public static dynamic LoadXmlOrDefault(this FileInfo url, XDocument xml)
        {
            return Xml.LoadOrDefault(url == null ? null : url.FullName, xml);
        }

        public static dynamic LoadXmlOrDefault(this FileInfo url, Func<XDocument> xml)
        {
            return Xml.LoadOrDefault(url == null ? null : url.FullName, xml);
        }

        public static dynamic LoadXml(this Uri url)
        {
            return Xml.Load(url == null ? null : url.AbsoluteUri);
        }

        public static dynamic LoadXmlOrDefault(this Uri url)
        {
            return Xml.LoadOrDefault(url == null ? null : url.AbsoluteUri);
        }

        public static dynamic LoadXmlOrDefault(this Uri url, XDocument xml)
        {
            return Xml.LoadOrDefault(url == null ? null : url.AbsoluteUri, xml);
        }

        public static dynamic LoadXmlOrDefault(this Uri url, Func<XDocument> xml)
        {
            return Xml.LoadOrDefault(url == null ? null : url.AbsoluteUri, xml);
        }

        public static dynamic LoadXml(this Url url)
        {
            return Xml.Load(url);
        }

        public static dynamic LoadXmlOrDefault(this Url url)
        {
            return Xml.LoadOrDefault(url);
        }

        public static dynamic LoadXmlOrDefault(this Url url, XDocument xml)
        {
            return Xml.LoadOrDefault(url, xml);
        }

        public static dynamic LoadXmlOrDefault(this Url url, Func<XDocument> xml)
        {
            return Xml.LoadOrDefault(url, xml);
        }

        public static dynamic ReadXml(this Stream s)
        {
            return Xml.Read(s);
        }

        public static dynamic ReadXmlOrDefault(this Stream s)
        {
            return Xml.ReadOrDefault(s);
        }

        public static dynamic ReadXmlOrDefault(this Stream s, XDocument xml)
        {
            return Xml.ReadOrDefault(s, xml);
        }

        public static dynamic ReadXmlOrDefault(this Stream s, Func<XDocument> xml)
        {
            return Xml.ReadOrDefault(s, xml);
        }

        public static dynamic ReadXml(this TextReader w)
        {
            return Xml.Read(w);
        }

        public static dynamic ReadXmlOrDefault(this TextReader w)
        {
            return Xml.ReadOrDefault(w);
        }

        public static dynamic ReadXmlOrDefault(this TextReader w, XDocument xml)
        {
            return Xml.ReadOrDefault(w, xml);
        }

        public static dynamic ReadXmlOrDefault(this TextReader w, Func<XDocument> xml)
        {
            return Xml.ReadOrDefault(w, xml);
        }

        public static void SaveXml(this String url, XDocument xml)
        {
            Xml.Save(url, xml);
        }

        public static void SaveXml(this FileInfo url, XDocument xml)
        {
            Xml.Save(url == null ? null : url.FullName, xml);
        }

        public static void SaveXml(this Uri url, XDocument xml)
        {
            Xml.Save(url.AbsoluteUri, xml);
        }

        public static void SaveXml(this Url url, XDocument xml)
        {
            Xml.Save(url, xml);
        }

        public static void WriteXml(this Stream s, XDocument xml)
        {
            Xml.Write(s, xml);
        }

        public static void WriteXml(this TextWriter w, XDocument xml)
        {
            Xml.Write(w, xml);
        }
    }
}