using System;
using System.Diagnostics;
using NUnit.Framework;
using XenoGears.Functional;
using System.Linq;
using XenoGears.Reflection.Attributes;
using XenoGears.Strings;
using XenoGears.Reflection;
using XenoGears.Reflection.Generics;

namespace XenoGears.Playground
{
    [TestFixture]
    public class MetaTests
    {
        [Test]
        public void EnsureEverythingInXenoGearsIsMarkedWithDebuggerNonUserCode()
        {
            var asm = typeof(EnumerableExtensions).Assembly;
            var types = asm.GetTypes().Where(t => !t.IsInterface).ToReadOnly();
            var failed_types = types
                .Where(t => !t.HasAttr<DebuggerNonUserCodeAttribute>())
                .Where(t => !t.IsCompilerGenerated())
                .Where(t => !t.Name.Contains("<>"))
                .Where(t => !t.Name.Contains("__StaticArrayInit"))
                .Where(t => !t.IsEnum)
                .Where(t => !t.IsDelegate())
                .ToReadOnly();

            if (failed_types.IsNotEmpty())
            {
                Trace.WriteLine(String.Format("{0} types in XenoGears aren't marked with [DebuggerNonUserCode]:", failed_types.Count()));
                var messages = failed_types.Select(t => t.GetCSharpRef(ToCSharpOptions.InformativeWithNamespaces));
                messages.OrderDescending().ForEach(message => Trace.WriteLine(message));
                Assert.Fail();
            }
        }
    }
}
