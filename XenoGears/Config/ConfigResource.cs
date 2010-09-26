using System;
using System.Diagnostics;
using System.Linq;
using XenoGears.Config.Codebase;
using XenoGears.Formats;
using XenoGears.Functional;
using XenoGears.Web.Rest.Annotations;

namespace XenoGears.Config
{
    [RestResource(Allow = RestMethods.Get | RestMethods.Post)]
    [DebuggerNonUserCode]
    public static class ConfigResource
    {
        [RestResource(Uri = "/config", Allow = RestMethods.Get)]
        public static Json Get()
        {
            var configs = ConfigRegistry.All;
            return new Json(configs.Keys.ToDictionary(name => name, Get));
        }

        [RestResource(Uri = "/config/{name}", Allow = RestMethods.Get)]
        public static Json Get(String name)
        {
            return new Json(AppConfig.Get(name));
        }

        [RestResource(Uri = "/config", Allow = RestMethods.Put)]
        public static void Put(Json json)
        {
            json.Keys.ForEach(name => Put(name, json[name]));
        }

        [RestResource(Uri = "/config/{name}", Allow = RestMethods.Put)]
        public static void Put(String name, Json json)
        {
            AppConfig.Put(name, json);
        }

        [RestResource(Uri = "/config", Allow = RestMethods.Merge)]
        public static void Merge(Json json)
        {
            json.Keys.ForEach(name => Merge(name, json[name]));
        }

        [RestResource(Uri = "/config/{name}", Allow = RestMethods.Merge)]
        public static void Merge(String name, Json json)
        {
            AppConfig.Merge(name, json);
        }
    }
}