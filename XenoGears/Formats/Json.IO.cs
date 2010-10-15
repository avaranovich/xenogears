using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using XenoGears.Streams;
using XenoGears.Strings;

namespace XenoGears.Formats
{
    public partial class Json
    {
        public static dynamic Load(String uri)
        {
            return Load(uri, null as Json, null as ICredentials);
        }

        public static dynamic Load(String uri, Json args, ICredentials credentials = null)
        {
            return Load(uri, credentials, args);
        }

        public static dynamic Load(String uri, ICredentials credentials, Json args = null)
        {
            if (uri == null) return null;

            var is_remote = uri.StartsWith("http://") || uri.StartsWith("https://");
            if (is_remote)
            {
                var req = (HttpWebRequest)WebRequest.Create(uri);
                req.Credentials = credentials ?? CredentialCache.DefaultCredentials;
                req.Accept = "application/json,*/*";

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
                        throw new Exception(String.Format("Failed to parse JSON response from \"{1}\":{2}{0}{3}",
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
                var path = is_web && uri.StartsWith("~") ? HttpContext.Current.Server.MapPath(uri) : uri;
                path = Path.GetFullPath(path);
                if (!File.Exists(path)) throw new Exception(String.Format(
                    "READ for \"{0}\" has failed: file \"{1}\" does not exist", uri, path));

                var s_json = File.ReadAllText(path);
                try { return Parse(s_json); }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("Failed to parse JSON from \"{1}\":{2}{0}{3}",
                        Environment.NewLine, uri, ex.Message, s_json));
                }
            }
        }

        public static dynamic LoadOrDefault(String uri, ICredentials credentials = null, Json args = null)
        {
            return LoadOrDefault(uri, null as Json, credentials, args);
        }

        public static dynamic LoadOrDefault(String uri, Object @default, ICredentials credentials = null, Json args = null)
        {
            return LoadOrDefault(uri, () => @default, credentials, args);
        }

        public static dynamic LoadOrDefault(String uri, Func<Object> @default, ICredentials credentials = null, Json args = null)
        {
            try { return Load(uri, credentials, args); }
            catch { return new Json((@default ?? (() => null))()); }
        }

        public static dynamic Read(Stream s)
        {
            var s_json = new StreamReader(s).ReadToEnd();
            try { return Parse(s_json); }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to parse JSON from stream \"{1}\":{2}{0}{3}",
                    Environment.NewLine, s, ex.Message, s_json));
            }
        }

        public static dynamic ReadOrDefault(Stream s)
        {
            return ReadOrDefault(s, null as Json);
        }

        public static dynamic ReadOrDefault(Stream s, Object @default)
        {
            return ReadOrDefault(s, () => @default);
        }

        public static dynamic ReadOrDefault(Stream s, Func<Object> @default)
        {
            try { return Read(s); }
            catch { return new Json((@default ?? (() => null))()); }
        }

        public static dynamic Read(TextReader r)
        {
            var s_json = r.ReadToEnd();
            try { return Parse(s_json); }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to parse JSON from reader \"{1}\":{2}{0}{3}",
                    Environment.NewLine, r, ex.Message, s_json));
            }
        }

        public static dynamic ReadOrDefault(TextReader r)
        {
            return ReadOrDefault(r, null as Json);
        }

        public static dynamic ReadOrDefault(TextReader r, Object @default)
        {
            return ReadOrDefault(r, () => @default);
        }

        public static dynamic ReadOrDefault(TextReader r, Func<Object> @default)
        {
            try { return Read(r); }
            catch { return new Json((@default ?? (() => null))()); }
        }

        public static void Save(String uri, Json json, ICredentials credentials = null)
        {
            if (uri == null) return;

            var is_remote = uri.StartsWith("http://") || uri.StartsWith("https://");
            if (is_remote)
            {
                var req = (HttpWebRequest)WebRequest.Create(uri);
                req.Credentials = credentials ?? CredentialCache.DefaultCredentials;

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
                var path = is_web && uri.StartsWith("~") ? HttpContext.Current.Server.MapPath(uri) : uri;
                path = Path.GetFullPath(path);
                if (!File.Exists(path)) throw new Exception(String.Format(
                    "{2} for \"{0}\" has failed: file \"{1}\" does not exist", uri, path, json == null ? "DELETE" : "WRITE"));

                var s_json = json == null ? null : json.ToCompactString();
                if (s_json == null) File.Delete(path);
                else File.WriteAllText(path, s_json);
            }
        }

        public void Save(String uri, ICredentials credentials = null)
        {
            Save(uri, this, credentials);
        }

        public static void Write(Stream s, Json json)
        {
            if (s == null || json == null) return;
            var s_json = json.ToCompactString();
            var b_json = Encoding.UTF8.GetBytes(s_json);
            s.Write(b_json, 0, b_json.Length);
        }

        public void Write(Stream s)
        {
            Write(s, this);
        }

        public static void Write(TextWriter w, Json json)
        {
            if (w == null || json == null) return;
            var s_json = json.ToCompactString();
            w.Write(s_json);
        }

        public void Write(TextWriter w)
        {
            Write(w, this);
        }
    }
}