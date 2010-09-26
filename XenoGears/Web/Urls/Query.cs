using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using XenoGears.Assertions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;
using System.Linq;

namespace XenoGears.Web.Urls
{
    [DebuggerNonUserCode]
    public class Query : BaseNonStrictDictionary<String, String>, ICloneable
    {
        private IDictionary<String, String> _impl;
        public void MakeReadOnly() { _impl = _impl.ToReadOnly(); }

        public Query(String queryString)
        {
            queryString = queryString ?? String.Empty;
            queryString = HttpUtility.UrlDecode(queryString);
            if (queryString.StartsWith("?")) queryString = queryString.Substring(1);

            _impl = new Dictionary<String, String>();
            if (queryString.IsEmpty()) return;

            queryString.Split('&').ForEach(part =>
            {
                var i_eq = part.IndexOf("=");
                var key = i_eq == -1 ? part : part.Substring(0, i_eq);
                var value = i_eq == -1 ? String.Empty : part.Substring(i_eq + 1);
                _impl.Add(key, value);
            });
        }

        public override String ToString()
        {
            return this.Select(kvp => kvp.Value.IsNullOrEmpty() ? kvp.Key :
                String.Format("{0}={1}", kvp.Key, kvp.Value)).StringJoin("&");
        }

        #region Boilerplate of implementing dictionary methods

        public override int Count { get { return _impl.Count; } }
        public override IEnumerator<KeyValuePair<String, String>> GetEnumerator() { return _impl.GetEnumerator(); }
        public override bool ContainsKey(String key) { return _impl.ContainsKey(key); }
        public override bool TryGetValue(String key, out String value) { return _impl.TryGetValue(key, out value); }
        public override bool IsReadOnly { get { return ((IDictionary<String, String>)_impl).IsReadOnly; } }
        public override void Add(String key, String value) { _impl.Add(key, value); }
        public override bool Remove(String key) { return _impl.Remove(key); }
        public override void Clear() { _impl.Clear(); }
        protected override void SetValue(String key, String value) { _impl[key] = value; }

        #endregion

        Object ICloneable.Clone() { return Clone(); }
        public Query Clone()
        {
            return new Query(this.ToString());
        }

        public static implicit operator String(Query query)
        {
            return query == null ? null : query.ToString();
        }

        public static implicit operator Query(String queryString)
        {
            return queryString == null ? null : new Query(queryString);
        }
    }
}