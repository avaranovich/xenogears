using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Formats;
using XenoGears.Functional;
using XenoGears.Logging;
using XenoGears.Web.Rest.Context;
using Log = XenoGears.Web.Logging.Log;

namespace XenoGears.Web.Rest.Dispatch
{
    [DebuggerNonUserCode]
    public class HandlerContext
    {
        public RestRequest Request { get; private set; }
        public ReadOnlyDictionary<String, String> Env { get; private set; }
        public MethodInfo Code { get; private set; }
        private LevelLogger Debug { get { return Log.Dispatch.Debug; } }
        private LevelLogger Info { get { return Log.Dispatch.Info; } }
        private LevelLogger Error { get { return Log.Dispatch.Error; } }

        public HandlerContext(RestRequest request, ReadOnlyDictionary<String, String> env, MethodInfo code)
        {
            Request = request.AssertNotNull();
            Env = env.AssertNotNull();
            Code = code.AssertNotNull();

            // todo:
            // 1) allow to return void, json, string, byte[], stream and any other object (the latter will then be serialized to json)
            // 2) allow non-statics
            // 3) allow RestRequest, RestResponse, RestContext, RestHints, Query, RequestHeaders, ResponseHeaders, Cookies, dynamic/Json (Data), Stream (output stream), TextWriter (response)
            // 4) allow config types
            // 5) deserialize when binding
            // 6) bind to fields as well
            // 7) comprehensive logging
            // 8) try to bind everything, rather than stop at first error

            Debug.EnsureBlankLine();
            Debug.WriteLine("    * Env: {0}", Env.Select(kvp => String.Format("{0} = {1}", kvp.Key, kvp.Value)).StringJoin());
            Debug.WriteLine("    * Query: {0}", request.Query.Select(kvp => String.Format("{0} = {1}", kvp.Key, (String)kvp.Value)).StringJoin().Fluent(s => s.IsEmpty() ? "<empty>" : s));
            Debug.WriteLine("    * Data: {0}", ((Json)request.Data).ToCompactString().Fluent(s => s.IsEmpty() ? "<empty>" : s));
            // todo. after that log:
            // Write("    * field foo <= ")
            // do something, then write either:
            // 1) WriteLine("Data.foo") or
            // 2) WriteLine("FAIL") or even
            // 3) exception.... [will be traced by RestGateway]
        }

        public bool Validate()
        {
            // perform all deserializations here, so that not-null failures get detected
            throw new NotImplementedException();
        }

        public Object Invoke()
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            // todo. implement the following:
            // <full method signature> (without colon!)
            //     * Data.foo => parameter foo
            //     * Cookie.bar => field <full field signature>
            //     * RestRequest => parameter req
            //     ? FAILED TO BIND => parameter baz
            //     ? FAILED TO BIND => not-null field <full field signature>
            throw new NotImplementedException();
        }
    }
}