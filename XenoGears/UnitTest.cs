using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears
{
    [DebuggerNonUserCode]
    public static class UnitTest
    {
        private static readonly Dictionary<Object, Object> _context = new Dictionary<Object, Object>();
        public static Dictionary<Object, Object> Context { get { return _context; } }

        public static MethodBase CurrentTest
        {
            get
            {
                var from_ctx = Context.GetOrDefault("Current Test") as MethodBase;
                if (from_ctx != null) return from_ctx;

                var frames = new StackTrace().GetFrames().Select(f => f.GetMethod()).ToArray();
                var suspect1 = frames.Nth(-4);
                var insideResharper45 = suspect1.DeclaringType.Assembly.GetName().FullName.Contains("JetBrains");
                if (insideResharper45)
                {
                    var ourFirstFrame = frames.Reverse().First(mb =>
                    {
                        var asm = mb.DeclaringType.Assembly.GetName().FullName;
                        return !(asm.Contains("JetBrains") || asm.Contains("System") || asm.Contains("mscorlib"));
                    });

                    return ourFirstFrame.AssertCast<MethodInfo>();
                }
                else
                {
                    var suspect2 = frames.Nth(-7);
                    var insideNUnitRunner = suspect2.DeclaringType.Assembly.GetName().FullName.Contains("nunit.core");
                    if (insideNUnitRunner)
                    {
                        var ourFirstFrame = frames.Reverse().First(mb =>
                        {
                            var asm = mb.DeclaringType.Assembly.GetName().FullName;
                            return !(asm.Contains("nunit.core") || asm.Contains("System") || asm.Contains("mscorlib"));
                        });

                        return ourFirstFrame.AssertCast<MethodInfo>();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static Type CurrentFixture
        {
            get
            {
                var from_ctx = Context.GetOrDefault("Current Fixture") as Type;
                if (from_ctx != null) return from_ctx;
                return CurrentTest == null ? null : CurrentTest.DeclaringType;
            }
        }

        public static String TransientId
        {
            get
            {
                if (UnitTest.CurrentTest != null)
                {
                    return Context.GetOrDefault("Transient Id").AssertCast<String>();
                }
                else
                {
                    return null;
                }
            }
        }

        public static String PersistentId
        {
            get
            {
                if (UnitTest.CurrentTest != null)
                {
                    var id = "Unit Test=" + UnitTest.CurrentTest.Name;
                    id += (", Test Fixture=" + UnitTest.CurrentFixture.Name);
                    var ctx_keys = Context.Keys.Except("Current Test", "Current Fixture");
                    if (ctx_keys.IsNotEmpty()) id += ", ";
                    id += ctx_keys.Select(key => key + "=" + Context[key]).StringJoin(", ");
                    return id;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}