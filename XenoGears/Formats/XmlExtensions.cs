using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml.Linq;
using XenoGears.Web.Urls;

namespace XenoGears.Formats
{
    [DebuggerNonUserCode]
    public static class XmlExtensions
    {
        public static XDocument ParseXml(this String url)
        {
            return Xml.Parse(url);
        }

        public static XDocument ParseXmlOrDefault(this String url)
        {
            return Xml.ParseOrDefault(url);
        }

        public static XDocument ParseXmlOrDefault(this String url, XDocument xml)
        {
            return Xml.ParseOrDefault(url, xml);
        }

        public static XDocument ParseXmlOrDefault(this String url, Func<XDocument> xml)
        {
            return Xml.ParseOrDefault(url, xml);
        }

        public static XDocument LoadXml(this String url, ICredentials credentials = null)
        {
            return Xml.Load(url, credentials);
        }

        public static XDocument LoadXmlOrDefault(this String url, ICredentials credentials = null)
        {
            return Xml.LoadOrDefault(url, credentials);
        }

        public static XDocument LoadXmlOrDefault(this String url, XDocument xml, ICredentials credentials = null)
        {
            return Xml.LoadOrDefault(url, xml, credentials);
        }

        public static XDocument LoadXmlOrDefault(this String url, Func<XDocument> xml, ICredentials credentials = null)
        {
            return Xml.LoadOrDefault(url, xml, credentials);
        }

        public static XDocument LoadXml(this FileInfo url)
        {
            return Xml.Load(url == null ? null : url.FullName);
        }

        public static XDocument LoadXmlOrDefault(this FileInfo url)
        {
            return Xml.LoadOrDefault(url == null ? null : url.FullName);
        }

        public static XDocument LoadXmlOrDefault(this FileInfo url, XDocument xml)
        {
            return Xml.LoadOrDefault(url == null ? null : url.FullName, xml);
        }

        public static XDocument LoadXmlOrDefault(this FileInfo url, Func<XDocument> xml)
        {
            return Xml.LoadOrDefault(url == null ? null : url.FullName, xml);
        }

        public static XDocument LoadXml(this Uri url, ICredentials credentials = null)
        {
            return Xml.Load(url == null ? null : url.AbsoluteUri, credentials);
        }

        public static XDocument LoadXmlOrDefault(this Uri url, ICredentials credentials = null)
        {
            return Xml.LoadOrDefault(url == null ? null : url.AbsoluteUri, credentials);
        }

        public static XDocument LoadXmlOrDefault(this Uri url, XDocument xml, ICredentials credentials = null)
        {
            return Xml.LoadOrDefault(url == null ? null : url.AbsoluteUri, xml, credentials);
        }

        public static XDocument LoadXmlOrDefault(this Uri url, Func<XDocument> xml, ICredentials credentials = null)
        {
            return Xml.LoadOrDefault(url == null ? null : url.AbsoluteUri, xml, credentials);
        }

        public static XDocument LoadXml(this Url url, ICredentials credentials = null)
        {
            return Xml.Load(url, credentials);
        }

        public static XDocument LoadXmlOrDefault(this Url url, ICredentials credentials = null)
        {
            return Xml.LoadOrDefault(url, credentials);
        }

        public static XDocument LoadXmlOrDefault(this Url url, XDocument xml, ICredentials credentials = null)
        {
            return Xml.LoadOrDefault(url, xml, credentials);
        }

        public static XDocument LoadXmlOrDefault(this Url url, Func<XDocument> xml, ICredentials credentials = null)
        {
            return Xml.LoadOrDefault(url, xml, credentials);
        }

        public static XDocument ReadXml(this Stream s)
        {
            return Xml.Read(s);
        }

        public static XDocument ReadXmlOrDefault(this Stream s)
        {
            return Xml.ReadOrDefault(s);
        }

        public static XDocument ReadXmlOrDefault(this Stream s, XDocument xml)
        {
            return Xml.ReadOrDefault(s, xml);
        }

        public static XDocument ReadXmlOrDefault(this Stream s, Func<XDocument> xml)
        {
            return Xml.ReadOrDefault(s, xml);
        }

        public static XDocument ReadXml(this TextReader w)
        {
            return Xml.Read(w);
        }

        public static XDocument ReadXmlOrDefault(this TextReader w)
        {
            return Xml.ReadOrDefault(w);
        }

        public static XDocument ReadXmlOrDefault(this TextReader w, XDocument xml)
        {
            return Xml.ReadOrDefault(w, xml);
        }

        public static XDocument ReadXmlOrDefault(this TextReader w, Func<XDocument> xml)
        {
            return Xml.ReadOrDefault(w, xml);
        }

        public static void SaveXml(this String url, XDocument xml, ICredentials credentials = null)
        {
            Xml.Save(url, xml, credentials);
        }

        public static void SaveXml(this FileInfo url, XDocument xml, ICredentials credentials = null)
        {
            Xml.Save(url == null ? null : url.FullName, xml, credentials);
        }

        public static void SaveXml(this Uri url, XDocument xml, ICredentials credentials = null)
        {
            Xml.Save(url.AbsoluteUri, xml, credentials);
        }

        public static void SaveXml(this Url url, XDocument xml, ICredentials credentials = null)
        {
            Xml.Save(url, xml, credentials);
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