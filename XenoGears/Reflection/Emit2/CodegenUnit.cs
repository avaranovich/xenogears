using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace XenoGears.Reflection.Emit2
{
    // note. this API is abandoned in favor of Truesight: http://code.google.com/p/truesight/
    // but for now I'll leave these two classes here, since they expose very useful functionality

    [DebuggerNonUserCode]
    public class CodegenUnit : IDisposable
    {
        private readonly AssemblyBuilder _asm;
        private readonly ModuleBuilder _mod;
        public ModuleBuilder Module { get { return _mod; } }

        private readonly Dictionary<Object, Object> _context = new Dictionary<Object, Object>();
        public Dictionary<Object, Object> Context { get { return _context; } }

        internal CodegenUnit(String unitName)
        {
            var asmName = new AssemblyName(unitName);
            var fileName = asmName.Name + ".dll";
            var pdbName = asmName + ".pdb";

            // so that we can run multiple tests at once and not lose the info
            if (UnitTest.Current != null)
            {
                fileName = asmName + ", " + UnitTest.PersistentId + ".dll";
                pdbName = asmName + ".pdb";
            }

            try
            {
#if TRACE
                _asm = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
                _mod = _asm.DefineDynamicModule(fileName, true);

                // Mark generated code as debuggable.
                // See http://blogs.msdn.com/jmstall/archive/2005/02/03/366429.aspx for explanation.
                var daCtor = typeof(DebuggableAttribute).GetConstructor(new []{typeof(DebuggableAttribute.DebuggingModes)});
                var daBuilder = new CustomAttributeBuilder(daCtor, new object[] { 
                    DebuggableAttribute.DebuggingModes.DisableOptimizations | 
                    DebuggableAttribute.DebuggingModes.Default });
                _asm.SetCustomAttribute(daBuilder);

                // Mark generated code as non-user code.
                // See http://stackoverflow.com/questions/1423733/how-to-tell-if-a-net-assembly-is-dynamic for explanation.
                var cgCtor = typeof(CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes);
                var cgBuilder = new CustomAttributeBuilder(cgCtor, new object []{});
                _asm.SetCustomAttribute(cgBuilder);

                var hasAlreadyBeenDumped = false;
                Action dumpAssembly = () =>
                {
                    if (!hasAlreadyBeenDumped && _asm != null)
                    {
                        try
                        {
                            // todo. before dumping make sure that all types are completed or else the dump will simply crash
                            // this is a complex task, but it needs to be resolved for generic case

                            _asm.Save(fileName);
                        }
                        catch (Exception ex)
                        {
                            var trace = String.Format("Codegen unit '{0}' has failed to dump the asm:{1}{2}",
                                unitName, Environment.NewLine, ex);
                            Trace.WriteLine(trace);

                            SafetyTools.SafeDo(() => 
                            {
                                File.WriteAllText(fileName, trace);
                                File.Delete(pdbName);
                            });
                        }
                        finally
                        {
                            hasAlreadyBeenDumped = true;
                        }
                    }
                };
                _dumpAssembly = dumpAssembly;

                // do not use DomainUnload here because it never gets fired for default domain
                // however, we need not to neglect because R#'s unit-test runner never exits process
                AppDomain.CurrentDomain.DomainUnload += (o, e) => dumpAssembly();
                AppDomain.CurrentDomain.ProcessExit += (o, e) => dumpAssembly();
#else
                _asm = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
                _asm.SetCustomAttribute(new CustomAttributeBuilder(typeof(CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes), new object []{}));
                _mod = _asm.DefineDynamicModule(fileName, false);
#endif
            }
            catch (Exception ex)
            {
#if TRACE
                var trace = String.Format("Codegen unit '{0}' has failed to initialize:{1}{2}",
                    unitName, Environment.NewLine, ex);
                Trace.WriteLine(trace);

                SafetyTools.SafeDo(() =>
                {
                    File.WriteAllText(fileName, trace);
                    File.Delete(pdbName);
                });
#endif
                throw;
            }
        }

        private Action _dumpAssembly;
        public void ForceDump()
        {
            _dumpAssembly();
        }

        // todo. implicitly codegen the disposition check in factory
        // by the way this check should also be injected into child objects (but how we detect them?)

        public void Dispose()
        {
            Codegen.Units.Dispose(this);
        }
    }
}