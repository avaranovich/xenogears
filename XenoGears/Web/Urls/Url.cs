using System;
using System.Diagnostics;
using System.Web;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Web.Urls
{
    [DebuggerNonUserCode]
    public partial class Url : ICloneable
    {
        private bool _isReadOnly = false;
        public void MakeReadOnly() { _isReadOnly = true; }

        private String _host;
        private String _path;
        private Query _query;

        /// <summary>
        /// Host component of the url.
        /// <para>----</para>
        /// <para>E.g. the "http://localhost:1000/" part</para>
        /// <para>of the "http://localhost:1000/daas/Items?$filter=Type%2FDescription%20eq%20%27Equipment%27" url.</para>
        /// <para>----</para>
        /// <para>Will be null if the url is a relative one, e.g. "/daas/services/ad?sync".</para>
        /// <para>If an empty string is assigned to this property, it will be reset to null.</para>
        /// <para>Always ends with a slash.</para>
        /// <para>If a non-empty string without trailing slash is assigned to this property, the slash will be appended automatically.</para>
        /// </summary>
        public String Host
        {
            get { return _host; }
            set
            {
                _isReadOnly.AssertFalse();

                _host = value.IsNullOrEmpty() ? null : value;
                _host = HttpUtility.UrlDecode(_host);
                if (_host != null && !_host.EndsWith("/")) _host += "/";
            }
        }

        /// <summary>
        /// Everything except the host component of the url, i.e. includes both service and query string components.
        /// <para>----</para>
        /// <para>E.g. the "/daas/Items?$filter=Type%2FDescription%20eq%20%27Equipment%27" part</para>
        /// <para>of the "http://localhost:1000/daas/Items?$filter=Type%2FDescription%20eq%20%27Equipment%27" url.</para>
        /// <para>----</para>
        /// <para>Is stored and displayed in url-decoded form.</para>
        /// <para>Always starts with a slash.</para>
        /// <para>If a string without leading slash is assigned to this property, the slash will be prepended automatically.</para>
        /// </summary>
        public String Resource
        {
            get
            {
                var resource = _path;
                if (_query.IsNotEmpty()) resource += ("?" + _query);
                return resource;
            }
            set
            {
                _isReadOnly.AssertFalse();

                value = value ?? String.Empty;
                value = HttpUtility.UrlDecode(value);
                if (!value.StartsWith("/")) value = "/" + value;

                var i_question = value.IndexOf("?");
                _path = i_question == -1 ? value : value.Substring(0, i_question);
                _query = new Query(i_question == -1 ? null : value.Substring(i_question + 1));
            }
        }

        /// <summary>
        /// The resource component except the query string part of the url.
        /// <para>----</para>
        /// <para>E.g. the "/daas/Items" part</para>
        /// <para>of the "http://localhost:1000/daas/Items?$filter=Type%2FDescription%20eq%20%27Equipment%27" url.</para>
        /// <para>----</para>
        /// <para>Is stored and displayed in url-decoded form.</para>
        /// <para>Always starts with a slash.</para>
        /// <para>If a string without leading slash is assigned to this property, the slash will be prepended automatically.</para>
        /// <para>Supported mostly for integration with ASP.NET. Most likely, using Parse with appropriate template passed will be more powerful.</para>
        /// </summary>
        public String Path
        {
            get { return _path; }
            set
            {
                _isReadOnly.AssertFalse();

                _path = value ?? String.Empty;
                _path = HttpUtility.UrlDecode(_path);
                if (!_path.StartsWith("/")) _path = "/" + _path;
            }
        }

        /// <summary>
        /// A hashmap that represents the query string component of the url.
        /// <para>----</para>
        /// <para>E.g. the ["$filter" => "Type/Description eq 'Equipment'"] map</para>
        /// <para>for the "http://localhost:1000/daas/Items?$filter=Type%2FDescription%20eq%20%27Equipment%27" url.</para>
        /// <para>----</para>
        /// <para>The hashmap doesn't crash if indexed by non-existent key. It just returns null.</para>
        /// <para>Both keys and values are stored in url-decoded form.</para>
        /// <para>Supported mostly for integration with ASP.NET. Most likely, using Parse with appropriate template passed will be more powerful.</para>
        /// </summary>
        public Query Query
        {
            get { return _query; }
            set
            {
                _isReadOnly.AssertFalse();
                _query = value ?? new Query(null);
            }
        }

        public Url(Uri uri)
            : this(uri.ToString())
        {
        }

        public Url(String url)
        {
            var i_afterProto = url.IndexOf("://");
            if (i_afterProto != -1)
            {
                var nextSlash = url.IndexOf("/", i_afterProto + 3);
                if (nextSlash == -1) nextSlash = url.Length;

                Host = url.Substring(0, nextSlash);
                url = url.Substring(nextSlash);
            }

            Resource = url;
        }

        public override String ToString()
        {
            var s_url = Resource;
            if (_host != null) s_url = _host + s_url.Substring(1);
            return s_url;
        }

        Object ICloneable.Clone() { return Clone(); }
        public Url Clone()
        {
            return new Url(this.ToString());
        }

        public static implicit operator String(Url url)
        {
            return url == null ? null : url.ToString();
        }

        public static implicit operator Url(String url)
        {
            return url == null ? null : new Url(url);
        }

        public static implicit operator Uri(Url url)
        {
            return url == null ? null : new Url(url.ToString());
        }

        public static implicit operator Url(Uri uri)
        {
            return uri == null ? null : new Url(uri.ToString());
        }
    }
}