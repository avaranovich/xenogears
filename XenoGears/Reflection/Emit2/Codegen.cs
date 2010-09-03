using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace XenoGears.Reflection.Emit2
{
    [DebuggerNonUserCode]
    public static class Codegen
    {
        private static readonly Object _unitsLock = new Object();
        private static readonly Dictionary<Tuple<Object, String>, CodegenUnit> _units = new Dictionary<Tuple<Object, String>, CodegenUnit>();
        public static CodegenUnits Units { get { return new CodegenUnits(); } }

        [DebuggerNonUserCode]
        public class CodegenUnits
        {
            public CodegenUnit this[String name]
            {
                get { return this[new AssemblyName(name)]; }
            }

            public CodegenUnit this[String name, StrongNameKeyPair key]
            {
                get { return this[new AssemblyName(name){KeyPair = key}]; }
            }

            public CodegenUnit this[StrongNameKeyPair key, String name]
            {
                get { return this[new AssemblyName(name){KeyPair = key}]; }
            }
            
            public CodegenUnit this[AssemblyName name]
            {
                get
                {
                    // segregate codegen units not only by name but also by test id
                    // so that multiple tests run at once in R# won't share the same codegen unit
                    var key = Tuple.New((Object)name, UnitTest.PersistentId);

                    lock (_unitsLock)
                    {
                        if (!_units.ContainsKey(key))
                        {
                            _units.Add(key, new CodegenUnit(name));
                        }

                        return _units[key];
                    }
                }
            }

            internal void Dispose(CodegenUnit unit)
            {
                lock (_unitsLock)
                {
                    var key = _units.Single(kvp => kvp.Value == unit).Key;
                    _units.Remove(key);
                }
            }
        }
    }
}
