using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using XenoGears.Streams;
using XenoGears.Strings;

namespace XenoGears.Formats
{
    [DebuggerNonUserCode]
    public partial class Xml
    {
        public static XDocument Parse(String s)
        {
            if (s == null) return null;
            return XDocument.Parse(s);
        }

        public static XDocument ParseOrDefault(String s)
        {
            return ParseOrDefault(s, null as XDocument);
        }

        public static XDocument ParseOrDefault(String s, XDocument @default)
        {
            return ParseOrDefault(s, () => @default);
        }

        public static XDocument ParseOrDefault(String s, Func<XDocument> @default)
        {
            try { return Parse(s); }
            catch { return (@default ?? (() => null))(); }
        }

        public static XDocument Load(String uri)
        {
            if (uri == null) return null;

            var is_remote = uri.StartsWith("http://") || uri.StartsWith("https://");
            if (is_remote)
            {
                var req = (HttpWebRequest)WebRequest.Create(uri);
                req.Credentials = CredentialCache.DefaultCredentials;

                try
                {
                    var resp = (HttpWebResponse)req.GetResponse();
                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("GET for \"{1}\" has failed: {2}{0}{3}",
                            Environment.NewLine, uri, resp.StatusCode, resp.GetResponseStream().DumpToString()));
                    }

                    var s_xml = resp.GetResponseStream().DumpToString();
                    try { return Parse(s_xml); }
                    catch (Exception ex)
                    {
                        throw new Exception(String.Format("Failed to parse XML response from \"{1}\":{2}{0}{3}",
                            Environment.NewLine, uri, ex.Message, s_xml));
                    }
                }
                catch (WebException wex)
                {
                    if (wex.Response != null)
                    {
                        var resp = (HttpWebResponse)wex.Response;
                        throw new Exception(String.Format("GET for \"{1}\" has failed: {2}{0}{3}",
                            Environment.NewLine, uri, resp.StatusCode, resp.GetResponseStream().DumpToString()), wex);
                    }
                    else
                    {
                        throw new Exception(String.Format("GET for \"{1}\" has failed: {0}{2}",
                            Environment.NewLine, uri, wex));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("GET for \"{1}\" has failed: {0}{2}",
                        Environment.NewLine, uri, ex));
                }
            }
            else
            {
                if (uri.StartsWith("file:///")) uri = uri.Slice("file:///".Length);

                var is_web = HttpContext.Current != null;
                var path = is_web ? HttpContext.Current.Server.MapPath(uri) : uri;
                path = Path.GetFullPath(path);
                if (!File.Exists(path)) throw new Exception(String.Format(
                    "READ for \"{0}\" has failed: file \"{1}\" does not exist", uri, path));

                var s_xml = File.ReadAllText(path);
                try { return Parse(s_xml); }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("Failed to parse XML from \"{1}\":{2}{0}{3}",
                        Environment.NewLine, uri, ex.Message, s_xml));
                }
            }
        }

        public static XDocument LoadOrDefault(String uri)
        {
            return LoadOrDefault(uri, null as XDocument);
        }

        public static XDocument LoadOrDefault(String uri, XDocument @default)
        {
            return LoadOrDefault(uri, () => @default);
        }

        public static XDocument LoadOrDefault(String uri, Func<XDocument> @default)
        {
            try { return Load(uri); }
            catch { return (@default ?? (() => null))(); }
        }

        public static XDocument Read(Stream s)
        {
            var s_json = new StreamReader(s).ReadToEnd();
            try { return XDocument.Parse(s_json); }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to parse XML from stream \"{1}\":{2}{0}{3}",
                    Environment.NewLine, s, ex.Message, s_json));
            }
        }

        public static XDocument ReadOrDefault(Stream s)
        {
            return ReadOrDefault(s, null as XDocument);
        }

        public static XDocument ReadOrDefault(Stream s, XDocument @default)
        {
            return ReadOrDefault(s, () => @default);
        }

        public static XDocument ReadOrDefault(Stream s, Func<XDocument> @default)
        {
            try { return Read(s); }
            catch { return (@default ?? (() => null))(); }
        }

        public static dynamic Read(TextReader r)
        {
            var s_json = r.ReadToEnd();
            try { return XDocument.Load(s_json); }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to parse XML from reader \"{1}\":{2}{0}{3}",
                    Environment.NewLine, r, ex.Message, s_json));
            }
        }

        public static XDocument ReadOrDefault(TextReader r)
        {
            return ReadOrDefault(r, null as XDocument);
        }

        public static XDocument ReadOrDefault(TextReader r, XDocument @default)
        {
            return ReadOrDefault(r, () => @default);
        }

        public static XDocument ReadOrDefault(TextReader r, Func<XDocument> @default)
        {
            try { return Read(r); }
            catch { return (@default ?? (() => null))(); }
        }

        public static void Save(String uri, XDocument xml)
        {
            if (uri == null) return;

            var is_remote = uri.StartsWith("http://") || uri.StartsWith("https://");
            if (is_remote)
            {
                var req = (HttpWebRequest)WebRequest.Create(uri);
                req.Credentials = CredentialCache.DefaultCredentials;

                req.Method = xml == null ? "DELETE" : "POST";
                if (xml != null) new StreamWriter(req.GetRequestStream()).Write(xml.ToString());

                try
                {
                    var resp = (HttpWebResponse)req.GetResponse();
                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("{4} for \"{1}\" has failed: {2}{0}{3}",
                            Environment.NewLine, uri, resp.StatusCode, resp.GetResponseStream().DumpToString(), req.Method));
                    }
                }
                catch (WebException wex)
                {
                    if (wex.Response != null)
                    {
                        var resp = (HttpWebResponse)wex.Response;
                        throw new Exception(String.Format("{4} for \"{1}\" has failed: {2}{0}{3}",
                            Environment.NewLine, uri, resp.StatusCode, resp.GetResponseStream().DumpToString(), wex, req.Method));
                    }
                    else
                    {
                        throw new Exception(String.Format("{3} for \"{1}\" has failed: {0}{2}",
                            Environment.NewLine, uri, wex, req.Method));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("{3} for \"{1}\" has failed: {0}{2}",
                        Environment.NewLine, uri, ex, req.Method));
                }
            }
            else
            {
                if (uri.StartsWith("file:///")) uri = uri.Slice("file:///".Length);

                var is_web = HttpContext.Current != null;
                var path = is_web ? HttpContext.Current.Server.MapPath(uri) : uri;
                path = Path.GetFullPath(path);
                if (!File.Exists(path)) throw new Exception(String.Format(
                    "{2} for \"{0}\" has failed: file \"{1}\" does not exist", uri, path, xml == null ? "DELETE" : "WRITE"));

                var s_json = xml == null ? null : xml.ToString();
                if (s_json == null) File.Delete(path);
                else File.WriteAllText(path, s_json);
            }
        }

        public static void Write(Stream s, XDocument xml)
        {
            if (s == null || xml == null) return;
            var s_json = xml.ToString();
            var b_json = Encoding.UTF8.GetBytes(s_json);
            s.Write(b_json, 0, b_json.Length);
        }

        public static void Write(TextWriter w, XDocument xml)
        {
            if (w == null || xml == null) return;
            var s_json = xml.ToString();
            w.Write(s_json);
        }
    }
}