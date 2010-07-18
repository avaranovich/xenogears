using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace XenoGears.Reflection.Emit2
{
    [DebuggerNonUserCode]
    public static class Codegen
    {
        private static readonly Object _unitsLock = new Object();
        private static readonly Dictionary<Tuple<String, String>, CodegenUnit> _units = new Dictionary<Tuple<String, String>, CodegenUnit>();
        public static CodegenUnits Units { get { return new CodegenUnits(); } }

        [DebuggerNonUserCode]
        public class CodegenUnits
        {
            public CodegenUnit this[String name]
            {
                get
                {
                    // segregate codegen units not only by name but also by test id
                    // so that multiple tests run at once in R# won't share the same codegen unit
                    var key = Tuple.New(name, UnitTest.PersistentId);

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
