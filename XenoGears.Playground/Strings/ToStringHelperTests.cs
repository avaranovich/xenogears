using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Strings;

namespace XenoGears.Playground.Strings
{
    [TestFixture]
    public class ToStringHelperTests
    {
        public Type C { get { return typeof(C<,,,>); } }
        public FieldInfo C_f1 { get { return C.GetField("_f1", BF.All); } }
        public FieldInfo C_f2 { get { return C.GetField("_f2", BF.All); } }
        public ConstructorInfo C_C { get { return C.GetConstructors(BF.All).Single(c => c.Params().Count() == 2); } }
        public MethodInfo C_M1 { get { return C.GetMethods(BF.All).Single(m => m.Name == "M1"); } }
        public MethodInfo C_M2 { get { return C.GetMethods(BF.All).Single(m => m.Name == "M2"); } }

        [Test]
        public void ToCSharp_Terse()
        {
            Assert.AreEqual("C`4", C.GetCSharpRef(ToCSharpOptions.Terse));
            Assert.AreEqual("C`4", C.GetCSharpDecl(ToCSharpOptions.Terse));
            Assert.AreEqual("Func`2 C`4::_f1", C_f1.GetCSharpRef(ToCSharpOptions.Terse));
            Assert.AreEqual("static Func`2 _f1", C_f1.GetCSharpDecl(ToCSharpOptions.Terse));
            Assert.AreEqual("int C`4::_f2", C_f2.GetCSharpRef(ToCSharpOptions.Terse));
            Assert.AreEqual("int _f2", C_f2.GetCSharpDecl(ToCSharpOptions.Terse));
            Assert.AreEqual("C`4::.ctor(T2 a1, ref int f2)", C_C.GetCSharpRef(ToCSharpOptions.Terse));
            Assert.AreEqual(".ctor(T2 a1, ref int f2)", C_C.GetCSharpDecl(ToCSharpOptions.Terse));
            Assert.AreEqual("MR C`4::M1(T1 a1, MT2 a2, Func`2 a3)", C_M1.GetCSharpRef(ToCSharpOptions.Terse));
            Assert.AreEqual("MR M1(T1 a1, MT2 a2, Func`2 a3)", C_M1.GetCSharpDecl(ToCSharpOptions.Terse));
            Assert.AreEqual("int C`4::M2(out DateTime a1, T2 a2)", C_M2.GetCSharpRef(ToCSharpOptions.Terse));
            Assert.AreEqual("static int M2(out DateTime a1, T2 a2)", C_M2.GetCSharpDecl(ToCSharpOptions.Terse));
        }

        [Test]
        public void ToCSharp_Informative()
        {
            Assert.AreEqual("C<T1, T2, R, T4>", C.GetCSharpRef(ToCSharpOptions.Informative));
            Assert.AreEqual("C<T1, T2, R, T4>", C.GetCSharpDecl(ToCSharpOptions.Informative));
            Assert.AreEqual("Func<T1, Action<R, T2>> C`4::_f1", C_f1.GetCSharpRef(ToCSharpOptions.Informative));
            Assert.AreEqual("static Func<T1, Action<R, T2>> _f1", C_f1.GetCSharpDecl(ToCSharpOptions.Informative));
            Assert.AreEqual("int C`4::_f2", C_f2.GetCSharpRef(ToCSharpOptions.Informative));
            Assert.AreEqual("int _f2", C_f2.GetCSharpDecl(ToCSharpOptions.Informative));
            Assert.AreEqual("C`4::.ctor(T2 a1, ref int f2)", C_C.GetCSharpRef(ToCSharpOptions.Informative));
            Assert.AreEqual(".ctor(T2 a1, ref int f2)", C_C.GetCSharpDecl(ToCSharpOptions.Informative));
            Assert.AreEqual("MR C`4::M1<MT1, MT2, MR>(T1 a1, MT2 a2, Func<R, MR> a3)" + Environment.NewLine +
            "    where MT1 : MT2, T2, T1" + Environment.NewLine +
            "    where MR : class, T1, IConvertible", C_M1.GetCSharpRef(ToCSharpOptions.Informative));
            Assert.AreEqual("MR M1<MT1, MT2, MR>(T1 a1, MT2 a2, Func<R, MR> a3)" + Environment.NewLine +
            "    where MT1 : MT2, T2, T1" + Environment.NewLine +
            "    where MR : class, T1, IConvertible", C_M1.GetCSharpDecl(ToCSharpOptions.Informative));
            Assert.AreEqual("int C`4::M2(out DateTime a1, T2 a2)", C_M2.GetCSharpRef(ToCSharpOptions.Informative));
            Assert.AreEqual("static int M2(out DateTime a1, T2 a2)", C_M2.GetCSharpDecl(ToCSharpOptions.Informative));
        }

        [Test]
        public void ToCSharp_InformativeWithNamespaces()
        {
            Assert.AreEqual("XenoGears.Playground.Strings.C<T1, T2, R, T4>", C.GetCSharpRef(ToCSharpOptions.InformativeWithNamespaces));
            Assert.AreEqual("XenoGears.Playground.Strings.C<T1, T2, R, T4>", C.GetCSharpDecl(ToCSharpOptions.InformativeWithNamespaces));
            Assert.AreEqual("System.Func<T1, System.Action<R, T2>> C`4::_f1", C_f1.GetCSharpRef(ToCSharpOptions.InformativeWithNamespaces));
            Assert.AreEqual("static System.Func<T1, System.Action<R, T2>> _f1", C_f1.GetCSharpDecl(ToCSharpOptions.InformativeWithNamespaces));
            Assert.AreEqual("int C`4::_f2", C_f2.GetCSharpRef(ToCSharpOptions.InformativeWithNamespaces));
            Assert.AreEqual("int _f2", C_f2.GetCSharpDecl(ToCSharpOptions.InformativeWithNamespaces));
            Assert.AreEqual("C`4::.ctor(T2 a1, ref int f2)", C_C.GetCSharpRef(ToCSharpOptions.InformativeWithNamespaces));
            Assert.AreEqual(".ctor(T2 a1, ref int f2)", C_C.GetCSharpDecl(ToCSharpOptions.InformativeWithNamespaces));
            Assert.AreEqual("MR C`4::M1<MT1, MT2, MR>(T1 a1, MT2 a2, System.Func<R, MR> a3)" + Environment.NewLine +
            "    where MT1 : MT2, T2, T1" + Environment.NewLine +
            "    where MR : class, T1, System.IConvertible", C_M1.GetCSharpRef(ToCSharpOptions.InformativeWithNamespaces));
            Assert.AreEqual("MR M1<MT1, MT2, MR>(T1 a1, MT2 a2, System.Func<R, MR> a3)" + Environment.NewLine +
            "    where MT1 : MT2, T2, T1" + Environment.NewLine +
            "    where MR : class, T1, System.IConvertible", C_M1.GetCSharpDecl(ToCSharpOptions.InformativeWithNamespaces));
            Assert.AreEqual("int C`4::M2(out System.DateTime a1, T2 a2)", C_M2.GetCSharpRef(ToCSharpOptions.InformativeWithNamespaces));
            Assert.AreEqual("static int M2(out System.DateTime a1, T2 a2)", C_M2.GetCSharpDecl(ToCSharpOptions.InformativeWithNamespaces));
        }

        [Test]
        public void ToCSharp_ForCodegen()
        {
            Assert.AreEqual("global::XenoGears.Playground.Strings.C<T1, T2, R, T4>", C.GetCSharpRef(ToCSharpOptions.ForCodegen));
            Assert.AreEqual("global::XenoGears.Playground.Strings.C<" +
                "[global::XenoGears.Playground.Strings.A1] T1, " +
                "T2, " +
                "[global::XenoGears.Playground.Strings.A1] R, " +
                "T4>;", C.GetCSharpDecl(ToCSharpOptions.ForCodegen));
            Assert.AreEqual("global::System.Func<T1, global::System.Action<R, T2>> C`4::_f1", C_f1.GetCSharpRef(ToCSharpOptions.ForCodegen));
            Assert.AreEqual("protected static global::System.Func<T1, global::System.Action<R, T2>> _f1;", C_f1.GetCSharpDecl(ToCSharpOptions.ForCodegen));
            Assert.AreEqual("int C`4::_f2", C_f2.GetCSharpRef(ToCSharpOptions.ForCodegen));
            Assert.AreEqual("private int _f2;", C_f2.GetCSharpDecl(ToCSharpOptions.ForCodegen));
            Assert.AreEqual("C`4::C(T2 a1, ref int f2)", C_C.GetCSharpRef(ToCSharpOptions.ForCodegen));
            Assert.AreEqual("[global::XenoGears.Playground.Strings.A5] " +
                "public C(" +
                "[global::XenoGears.Playground.Strings.A4] T2 a1, " +
                "[global::XenoGears.Playground.Strings.A3] ref int f2);", C_C.GetCSharpDecl(ToCSharpOptions.ForCodegen));
            Assert.AreEqual("MR C`4::M1<MT1, MT2, MR>(T1 a1, MT2 a2, global::System.Func<R, MR> a3)" + Environment.NewLine +
            "    where MT1 : MT2, T2, T1" + Environment.NewLine +
            "    where MR : class, T1, global::System.IConvertible", C_M1.GetCSharpRef(ToCSharpOptions.ForCodegen));
            Assert.AreEqual("[global::XenoGears.Playground.Strings.A5] " +
                "[return: global::XenoGears.Playground.Strings.A2] " + 
                "public MR M1<" +
                "[global::XenoGears.Playground.Strings.A1] MT1, " +
                "MT2, " +
                "MR" +
                ">(" +
                "[global::XenoGears.Playground.Strings.A3] T1 a1, " +
                "MT2 a2, " +
                "[global::XenoGears.Playground.Strings.A4] global::System.Func<R, MR> a3)" + Environment.NewLine +
                "    where MT1 : MT2, T2, T1" + Environment.NewLine +
                "    where MR : class, T1, global::System.IConvertible;", C_M1.GetCSharpDecl(ToCSharpOptions.ForCodegen));
            Assert.AreEqual("int C`4::M2(out global::System.DateTime a1, T2 a2)", C_M2.GetCSharpRef(ToCSharpOptions.ForCodegen));
            Assert.AreEqual("internal static int M2(" +
                "[global::System.Runtime.InteropServices.OutAttribute] " +
                "out global::System.DateTime a1, T2 a2);", C_M2.GetCSharpDecl(ToCSharpOptions.ForCodegen));
        }

        [Test]
        public void ToCSharp_ForVerbose()
        {
            Assert.AreEqual("XenoGears.Playground.Strings.C`4<T1, T2, R, T4>", C.GetCSharpRef(ToCSharpOptions.Verbose));
            Assert.AreEqual("XenoGears.Playground.Strings.C`4<" +
                "[XenoGears.Playground.Strings.A1] T1, " +
                "T2, " +
                "[XenoGears.Playground.Strings.A1] R, " +
                "T4>", C.GetCSharpDecl(ToCSharpOptions.Verbose));
            Assert.AreEqual("System.Func`2<T1, System.Action`2<R, T2>> C`4::_f1", C_f1.GetCSharpRef(ToCSharpOptions.Verbose));
            Assert.AreEqual("protected static System.Func`2<T1, System.Action`2<R, T2>> C`4::_f1", C_f1.GetCSharpDecl(ToCSharpOptions.Verbose));
            Assert.AreEqual("int C`4::_f2", C_f2.GetCSharpRef(ToCSharpOptions.Verbose));
            Assert.AreEqual("private int C`4::_f2", C_f2.GetCSharpDecl(ToCSharpOptions.Verbose));
            Assert.AreEqual("C`4::.ctor(T2 a1, ref int f2)", C_C.GetCSharpRef(ToCSharpOptions.Verbose));
            Assert.AreEqual("[XenoGears.Playground.Strings.A5] " +
                "public C`4::.ctor(" +
                "[XenoGears.Playground.Strings.A4] T2 a1, " +
                "[XenoGears.Playground.Strings.A3] ref int f2)", C_C.GetCSharpDecl(ToCSharpOptions.Verbose));
            Assert.AreEqual("MR C`4::M1<MT1, MT2, MR>(T1 a1, MT2 a2, System.Func`2<R, MR> a3)" + Environment.NewLine +
            "    where MT1 : MT2, T2, T1" + Environment.NewLine +
            "    where MR : class, T1, System.IConvertible", C_M1.GetCSharpRef(ToCSharpOptions.Verbose));
            Assert.AreEqual("[XenoGears.Playground.Strings.A5] " +
                "[return: XenoGears.Playground.Strings.A2] " +
                "public MR C`4::M1<" +
                "[XenoGears.Playground.Strings.A1] MT1, " +
                "MT2, " +
                "MR" +
                ">(" +
                "[XenoGears.Playground.Strings.A3] T1 a1, " +
                "MT2 a2, " +
                "[XenoGears.Playground.Strings.A4] System.Func`2<R, MR> a3)" + Environment.NewLine +
                "    where MT1 : MT2, T2, T1" + Environment.NewLine +
                "    where MR : class, T1, System.IConvertible", C_M1.GetCSharpDecl(ToCSharpOptions.Verbose));
            Assert.AreEqual("int C`4::M2(out System.DateTime a1, T2 a2)", C_M2.GetCSharpRef(ToCSharpOptions.Verbose));
            Assert.AreEqual("internal static int C`4::M2(" +
                "[System.Runtime.InteropServices.OutAttribute] " +
                "out System.DateTime a1, T2 a2)", C_M2.GetCSharpDecl(ToCSharpOptions.Verbose));
        }

        [Test]
        public void ToCSharp_CustomAttributeData()
        {
            var m_cad = C.GetMethods(BF.All).Single(m => m.Name == "M3");
            Assert.AreEqual("[global::XenoGears.Playground.Strings.A6("+
                "0.25f, "+
                "typeof(global::XenoGears.Playground.Strings.A6), "+
                "Prop1 = \"\\r\\xdпривет\", "+
                "Prop4 = false, "+
                "Prop3 = 3u)] public void M3()", 
                m_cad.GetCSharpDecl(ToCSharpOptions.ForCodegen));
        }
    }

    [AttributeUsage(AttributeTargets.GenericParameter)]
    public class A1 : Attribute {}

    [AttributeUsage(AttributeTargets.ReturnValue)]
    public class A2 : Attribute {}

    [AttributeUsage(AttributeTargets.Parameter)]
    public class A3 : Attribute {}

    [AttributeUsage(AttributeTargets.Parameter)]
    public class A4 : Attribute {}

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
    public class A5 : Attribute {}

    [AttributeUsage(AttributeTargets.Method)]
    public class A6 : Attribute
    {
        public String Prop1 { get; set; }
        public uint Prop3 { get; set; }
        public bool Prop4 { get; set; }

        public A6(float prop5, Type prop6)
        {
        }
    }

    internal class C<[A1] T1, T2, [A1] R, T4>
        where R : struct
        where T2 : class, T1, IConvertible
    {
        protected static Func<T1, Action<R, T2>> _f1 = null;
        private int _f2;
        [A5] public C([A4] T2 a1, [A3] ref int f2) { _f2 = f2; }
        [return: A2] [A5] public MR M1<[A1] MT1, MT2, MR>([A3] T1 a1, MT2 a2, [A4] Func<R, MR> a3) 
            where MT1 : MT2, T2, T1 where MR : class, T1, IConvertible { throw new NotImplementedException(); }
        internal static int M2(out DateTime a1, T2 a2) { throw new NotImplementedException(); }
        [A6(0.25f, typeof(A6), Prop1 = "\r\xdпривет", Prop4 = false, Prop3 = 3u)] public void M3() { throw new NotImplementedException(); }
    }
}
