using System;
using System.Diagnostics;
using System.Linq;
using XenoGears.Strings;

namespace XenoGears.Formats
{
    [DebuggerDisplay("{ToCompactString(), nq}")]
    [DebuggerTypeProxy(typeof(JsonDebugView))]
    [DebuggerNonUserCode]
    public partial class Json
    {
        [DebuggerDisplay("{ToString(), nq}{\"\", nq}", Name = "{_name, nq}{\"\", nq}")]
        [DebuggerNonUserCode]
        private class JsonDebugView
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly Json _json;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly String _name;
            public JsonDebugView(Json json) { _json = json; }
            public JsonDebugView(Json json, Object name) { _json = json; _name = name.ToInvariantString(); }
            public override String ToString() { return _json.ToDebugString(); }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Object zStructure
            {
                get
                {
                    var keys = _json.Keys;
                    return keys.Select(key => new JsonDebugView(_json[(Object)key], ((Object)key).ToString())).ToArray();
                }
            }
        }
    }
}
