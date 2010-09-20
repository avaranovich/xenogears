using System;
using System.IO;
using System.Net;
using System.Web;
using XenoGears.Assertions;
using XenoGears.Streams;
using XenoGears.Strings;

namespace XenoGears.Formats
{
    public partial class Json
    {
        public static Json Load(String uri)
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

                    var s_json = resp.GetResponseStream().DumpToString();
                    try { return Parse(s_json); }
                    catch (Exception ex)
                    {
                        throw new Exception(String.Format("Failed to parse response from \"{1}\":{2}{0}{3}",
                            Environment.NewLine, uri, ex.Message, s_json));
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

                var s_json = File.ReadAllText(path);
                try { return Parse(s_json); }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("Failed to parse response from \"{1}\":{2}{0}{3}",
                        Environment.NewLine, uri, ex.Message, s_json));
                }
            }
        }

        public static void Save(String uri, Json json)
        {
            if (uri == null) return;

            var is_remote = uri.StartsWith("http://") || uri.StartsWith("https://");
            if (is_remote)
            {
                var req = (HttpWebRequest)WebRequest.Create(uri);
                req.Credentials = CredentialCache.DefaultCredentials;

                req.Method = json == null ? "DELETE" : "POST";
                if (json != null) new StreamWriter(req.GetRequestStream()).Write(json.ToCompactString());

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
                    "{2} for \"{0}\" has failed: file \"{1}\" does not exist", uri, path, json == null ? "DELETE" : "WRITE"));

                var s_json = json == null ? null : json.ToCompactString();
                if (s_json == null) File.Delete(path);
                else File.WriteAllText(path, s_json);
            }
        }
    }
}