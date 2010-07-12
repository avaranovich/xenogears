using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace XenoGears.Reflection.Anonymous
{
    // Source code shamelessly copy/pasted from http://www.codeplex.com/interlinq
    // todo: rewrite this stuff by my own hands
    [DebuggerNonUserCode]
    internal class DynamicAssemblyHolder
    {
        private AssemblyBuilder _assembly;
        private static DynamicAssemblyHolder _current;
        private ModuleBuilder _module;

        private DynamicAssemblyHolder()
        {
        }

        private void Initialize()
        {
            var currentDomain = AppDomain.CurrentDomain;
            var name = new AssemblyName{Name = "Relinq.AnonymousTypes"};
#if DEBUG
            _assembly = currentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);
            _module = _assembly.GetDynamicModule("XenoGears.AnonymousTypes.Module") ??
                _assembly.DefineDynamicModule("XenoGears.AnonymousTypes.Module", "anonymous.dll");
#else
            _assembly = currentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            _module = _assembly.GetDynamicModule("XenoGears.AnonymousTypes.Module") ??
                _assembly.DefineDynamicModule("XenoGears.AnonymousTypes.Module");
#endif
        }

        public static DynamicAssemblyHolder Current
        {
            get
            {
                if (_current == null)
                {
                    lock (typeof(DynamicAssemblyHolder))
                    {
                        if (_current == null)
                        {
                            _current = new DynamicAssemblyHolder();
                            _current.Initialize();
                        }
                    }
                }

                return _current;
            }
        }

#if DEBUG
        public AssemblyBuilder Assembly
        {
            get { return _assembly; }
        }
#endif

        public ModuleBuilder ModuleBuilder
        {
            get
            {
                return _module;
            }
        }
    }
}
