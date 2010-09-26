using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using XenoGears.Functional;

namespace XenoGears.Formats
{
    [DebuggerDisplay("{ToCompactString(), nq}")]
    [DebuggerTypeProxy(typeof(JsonDebugView))]
//    [DebuggerNonUserCode]
    public partial class Json
    {
        [DebuggerDisplay("{ToString(), nq}{\"\", nq}", Name = "{_name, nq}{\"\", nq}")]
        [DebuggerNonUserCode]
        private class JsonDebugView
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly Json _json;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly String _name;
            public JsonDebugView(Json json) { _json = json; }
            public JsonDebugView(Json json, String name) { _json = json; _name = name; }
            public override String ToString() { return _json.ToString(); }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Object zStructure
            {
                get
                {
                    var keys = ((IDynamicMetaObjectProvider)_json).GetMetaObject(Expression.Constant(_json)).GetDynamicMemberNames().ToReadOnly();
                    if (_json.IsArray) keys = keys.OrderBy(key => int.Parse(key)).ToReadOnly();
                    return keys.Select(key => new JsonDebugView(_json[key], key)).ToArray();
                }
            }
        }
    }
}
