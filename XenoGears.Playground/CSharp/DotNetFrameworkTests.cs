using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;
using NUnit.Framework;
using XenoGears.Playground.CSharp.Domain;
using XenoGears.Functional;
using XenoGears.Reflection;
using XenoGears.Assertions;
using XenoGears.Reflection.Attributes;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Playground.CSharp
{
    [TestFixture]
    public class DotNetFrameworkTests
    {
        private class A
        {
            private int Ftn()
            {
                return 1;
            }
        }

        [Test]
        public void TestCreateDelegate()
        {
            var @delegate = (Func<int>)System.Delegate.CreateDelegate(
                typeof(Func<int>), new A(), typeof(A).GetMethod("Ftn", BF.All));
            Assert.AreEqual(1, @delegate());
        }

        [Test]
        public void TestInterfaceMapping()
        {
            var map = typeof(MultipleInterfaces).GetInterfaceMap(typeof(I));
            map.InterfaceMethods.AssertCount(0);

            var map2 = typeof(MultipleInterfaces).GetInterfaceMap(typeof(I2));
            map2.InterfaceMethods.AssertCount(1);
        }

        private interface I : I1, I2 {}
        private interface I1 { void Method1(); }
        private interface I2 : I21 { void Method2(); }
        private interface I21 { void Method21(); }
        private class MultipleInterfaces : I
        {
            void I1.Method1() { throw new NotImplementedException(); }
            void I21.Method21() { throw new NotImplementedException(); }
            void I2.Method2() { throw new NotImplementedException(); }
        }

        [Test]
        public void TestWaitWithExitContext()
        {
            const int count = 5;
            var thres = count;
            var mre = new ManualResetEvent(false);
            var sync = new Object();
            var finished = 0;

            var threads = 1.UpTo(count).Select(_ => new Thread(() =>
            {
                lock (sync)
                {
                    if (thres-- > 0)
                    {
                        // this ain't work because exitContext processes only CBOs
                        // thus this code will deadlock all threads
                        mre.WaitOne(Timeout.Infinite, true);
                    }
                    else
                    {
                        mre.Set();
                    }
                }

                Interlocked.Increment(ref finished);
            })).ToArray();

            threads.ForEach(t => t.Start());
            Thread.Sleep(1000);

            Assert.AreEqual(0, finished);
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestInvokeMethodInfoAndReturnVoid()
        {
            var current_method = MethodInfo.GetCurrentMethod();
            var recursion_level = new StackTrace().GetFrames().Count(f => f.GetMethod() == current_method) - 1;
            if (recursion_level > 0)
            {
                return;
            }
            else
            {
                var meth = MethodInfo.GetCurrentMethod();
                var ret = meth.Invoke(this, null);
                ret.AssertNull();
            }
        }

        [Test]
        public void TestThisDelegateVsExtension()
        {
            var t = new TestThisDelegateVsExtensionClass();
            Assert.AreEqual(-1, t.Count());
        }

        public class TestThisDelegateVsExtensionClass : List<int>
        {
            public new Func<int> Count = () => -1;
        }

        [Test]
        public void TestIncrementor()
        {
            Func<int, Func<int>> incFactory = i => () => i++;
            var inc = incFactory(0);
            Assert.AreEqual(0, inc());
            Assert.AreEqual(1, inc());
            Assert.AreEqual(2, inc());
            Assert.AreEqual(3, inc());
        }

        [Test]
        public void TestLambdaOfFunctionTypeAndTypeParamsInference()
        {
            // compilation error, both times
//            Sig2(() => (() => 0));
//            Sig2(() => MethodGroup);
        }

        private R Sig2<R>(Func<Func<R>> func) { return func()(); }

        [Test]
        public void TestInferMethodGroupContributingTypeParamsInference()
        {
            // compilation error
//            Sig(MethodGroup);
        }

        private R Sig<R>(Func<R> func) { return func(); }
        private int MethodGroup() { return 0; }

        [Test]
        public void TestConditional()
        {
            EventHandler eh = (o, e) => {};
            var cond = DateTime.Now.Ticks % 2 == 0 ? eh : EventHandlerImpl;
            var cond2 = DateTime.Now.Ticks % 2 == 0 ? eh : (o, e) => {};
        }

        private void EventHandlerImpl(Object o, EventArgs args){}

        [Test]
        public void TestPredefinedEquality()
        {
            // cannot apply operator
//            var eqStructs = new S() == new S();
            var eqClasses = new C() == new C();
        }

        public struct S { }
        public class C { }

        [Test]
        public void TestNullableIsValueType()
        {
            Assert.IsTrue(typeof(Nullable<>).IsValueType);
            Assert.IsFalse(typeof(Nullable<>).IsNonNullableValueType());
        }

        [Test]
        public void TestArrayIsArray()
        {
            Assert.IsFalse(typeof(Array).IsArray);
        }

        [Test]
        public void TestOperatorsAndIndexersHiding()
        {
//            var r = new AA() + new AB();
            // ambiguous invocation

            var res = new AB()[1, 2];
            var res2 = new AB()[1];
            // works fine
        }

        public class AA
        {
            public void instance() { }
            public static void @static() { }
            public static AA operator +(AA aa, AB ab) { return null; }
            public AA this[int index] { get { return null; } }
            public AA this[int index, int index2] { get { return null; } }
        }

        public class AB : AA
        {
            public new void instance() { }
            public new static void @static() { }
            // can't declare operator as newslot
            public static AA operator +(AA aa, AB ab) { return null; }
            public new AA this[int index] { get { return null; } }
        }

        public class BaseAttribute : Attribute { }
        public class DerivedAttribute : BaseAttribute { }
        [Derived] public class MyClass { }

        [Test]
        public void TestRefParameters()
        {
            var p = typeof(DotNetFrameworkTests).GetMethod("Method").GetParameters()[0];
            Assert.IsTrue(p.ParameterType.IsByRef);
        }

        public void Method(ref int @ref) { }

        private void TestArrayParametersOverload(int[][] jagged) { }
        private void TestArrayParametersOverload(int[,] rect) { }

        [Test]
        public void TestConstFields()
        {
//            var fi = typeof(EcmaScriptV3Lexer).GetField("TRY");
//            Assert.IsTrue(fi.IsLiteral);
        }

        [Test]
        public void TestLinqVariableCapture()
        {
            ParameterizedQuery(0);
        }

        private void ParameterizedQuery(int i)
        {
            var s = "s";
            var ds = new[] { new { i = 0, s = "s" }, new { i = 0, s = "s" }, new { i = 1, s = "s" } };

            var q = from at in ds where at.i == i && at.s == s select at;
            var q1c = q.Count();

            i = 1;
            var q2c = q.Count();

            Assert.AreEqual(2, q1c);
            Assert.AreEqual(1, q2c);
        }

        [Test]
        public void TestArrayMethods()
        {
            var ownArrayMethods = typeof(bool[]).GetMethods().Where(mi => mi.DeclaringType == typeof(bool[]));
            Assert.AreEqual(3, ownArrayMethods.Count());
            Assert.AreEqual("Void Set(Int32, Boolean)", ownArrayMethods.ElementAt(0).ToString());
            Assert.AreEqual("Boolean& Address(Int32)", ownArrayMethods.ElementAt(1).ToString());
            Assert.AreEqual("Boolean Get(Int32)", ownArrayMethods.ElementAt(2).ToString());
        }

        // invalid base type
        //        public class TestArrayInheritance : int[]{}

        [Test]
        public void TestUnaryVsPrimary()
        {
            Expression<Func<Company, bool>> expr = c => !c.Employees;
            Assert.AreEqual("c => Not(Convert(c.Employees))", expr.ToString());
        }

        [Test]
        public void TestCharEquality()
        {
            Expression<Func<char, char, bool>> expr = (c1, c2) => c1 == c2;
            Assert.AreEqual("(c1, c2) => (Convert(c1) = Convert(c2))", expr.ToString());
        }

        [Test]
        public void TestSupaIndexer()
        {
            Assert.IsTrue(typeof(SupaIndexer).HasAttr<DefaultMemberAttribute>());

            Expression<Func<String, char>> expr1 = s => s[0];
            Assert.AreEqual("s => s.get_Chars(0)", expr1.ToString());

            Expression<Func<SupaIndexer, int>> expr2 = s => s[0];
            Assert.AreEqual("s => s.get_MyIndexer(0)", expr2.ToString());
        }

        public class SupaIndexer
        {
            [IndexerName("MyIndexer")]
            public int this[int pew]
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            // compilation error
//            public int MyIndexer { get; set; }

            // compilation error
            // Two indexers have different names; 
            // the IndexerName attribute must be used with the same name on every indexer within a type

//            public int this[string pew]
//            {
//                get { throw new NotImplementedException(); }
//                set { throw new NotImplementedException(); }
//            }
        }

        [Test]
        public void TestVarargsMethods()
        {
            var mi = this.GetType().GetMethod("Varargs", new Type[] { typeof(int[]) });
            Assert.IsNotNull(mi);
        }

        public void Varargs(params int[] ies) { }

        [Test]
        public void TestDelegateInvocationWithImplicitConversion()
        {
            var d = new Delegate(Meth);
            int i = d(new CDel());
        }

        public class CDel { public static implicit operator string(CDel cdel) { return null; } }
        public delegate byte Delegate(string s);
        public byte Meth(string m) { return 0; }

        [Test]
        public void TestFuncVsExpressionMethodResolution()
        {
//            Meth(i => i);
//            Meth((int i) => i);
            // ambiguous invocation both times
        }

        private void Meth(Func<int, int> func) { }
        private void Meth(Expression<Func<int, int>> expr) { }

        [Test]
        public void TestEnumAddition()
        {
            var r1 = E1.v11 + 1;
            Assert.AreEqual(E1.v12, r1);
        }

        public enum E1 { v11, v12 }
        public enum E2 { v21, v22 }

        [Test]
        public void TestMethodResolutionWithUserDefinedCasts()
        {
//            var c3 = new C3();
//            // C3 -> C2 -> B2 -> B1 -> A1 -> A0
//            Meth(c3); 
            // compile-time error!
        }

        public class A0 { }
        public class A1 : A0 { }
        public class B1 { public static implicit operator A1(B1 b1) { return null; } }
        public class B2 : B1 { }
        public class C2 { public static implicit operator B2(C2 c2) { return null; } }
        public class C3 : C2 { }

        private void Meth(A0 a0) { }

        [Test]
        public void TestMethodResolutionWithLifted()
        {
            var foo = new Foo<int>();
            var nullable = (int?)0;

            int? woot2 = foo * nullable;
            Assert.AreEqual("int? *(Foo<T>?, int?)", Foo<int>.OpResult);

            int? woot1 = foo + nullable;
            Assert.AreEqual("operator +", Foo<int>.OpResult);
        }

        public struct Foo<T>
        {
            public static String OpResult;

            public static int operator +(Foo<T> foo, int i)
            {
                OpResult = "operator +";
                return 0;
            }

            public static int? op_Addition(Foo<T>? foo, int? i)
            {
                OpResult = "op_Addition";
                return 0;
            }

            public static int operator *(Foo<T> foo, int i)
            {
                OpResult = "int *(Foo<T>, int)";
                return 0;
            }

            public static int? operator *(Foo<T>? foo, int? i)
            {
                OpResult = "int? *(Foo<T>?, int?)";
                return 0;
            }
        }

        [Test]
        public void TestMethodResolution()
        {
            // 25.1.7 Overloading in generic classes
            // Methods, constructors, indexers, and operators within a generic class declaration can be overloaded. 
            // While signatures as declared must be unique, it is possible that substitution of type arguments 
            // results in identical signatures. 
            // In such a situation, overload resolution will pick the most specific one (14.4.2.2).
            // 14.4.2.2 Better function member
            // If one of MP and MQ is non-generic, but the other is generic, then the non-generic is better.
            var b = new FooBar<int>();
            Assert.AreEqual("Foo(int)", b.Foo(0));
            Assert.AreEqual("Bar(int)", b.Bar(0));
            Assert.AreEqual("Bar2(T)", b.Bar2(0));

            // 14.4.2.2 Better function member
            // Otherwise, if one of MP and MQ is applicable in its non-expanded form (or has no params array) and the
            // other is applicable only in its expanded form (and has a params array), 
            // then the non-expanded method is better.
            Assert.AreEqual("Foobar(int)", b.Foobar(0));

            // 14.4.2.2 Better function member
            // Otherwise, if the numbers of parameters K in MP and L in MQ are different, 
            // then the method with more parameters is better. 
            // This can only occur if both methods have params arrays 
            // and are only applicable in their expanded forms.
            Assert.AreEqual("Foobar2(int, params)", b.Foobar2(0));
        }

        private class FooBar<T>
        {
            public String Foo(T t) { return "Foo(T)"; }
            public String Foo(int i) { return "Foo(int)"; }

            public String Bar<U>(U t) { return "Bar<U>(U)"; }
            public String Bar(int d) { return "Bar(int)"; }

            public String Bar2<U>(U t) { return "Bar2<U>(U)"; }
            public String Bar2(T d) { return "Bar2(T)"; }

            public String Foobar(int d) { return "Foobar(int)"; }
            public String Foobar(params int[] d) { return "Foobar(params)"; }

            public String Foobar2(int d1, params int[] d2) { return "Foobar2(int, params)"; }
            public String Foobar2(params int[] d) { return "Foobar2(params)"; }
        }

        [Test]
        public void TestDecimal()
        {
            var d = 2m;
            var c = TypeDescriptor.GetConverter(d.GetType());
            var s = c.ConvertTo(d, typeof(String));
            Assert.AreEqual("2", s);
        }

        private class TestOps1
        {
            // the line below causes compilation errors
//            public static int operator +(Source1 op1, TestOps2 op2)
//            {
//                return 0;
//            }
        }

        private class TestOps2
        {
            public static int operator +(TestOps1 op1, TestOps2 op2)
            {
                return 0;
            }
        }

        [Test]
        public void TestIndirectCoercion()
        {
            // the line below causes compilation errors
//            TargetForCoercion tar = new Source1();
            TargetForCoercion tar = (Source2)new Source1();
        }

        private class Source1
        {
            public static implicit operator Source2(Source1 s) { return null; }
        }

        private class Source2
        {
            public static implicit operator TargetForCoercion(Source2 s) { return null; }
        }

        private class TargetForCoercion
        {

        }

        [Test]
        public void TestDelegatesCompatibility()
        {
            // compilation error...
            //            Func<int, int> pureFunc = p1 => 2 * p1;
            //            UseFuncWannabe((FuncWannabe)pureFunc);

            // compilation error - haha lol, .NET fails in the same way as I do
            //            FuncWannabe fw = p1 => 2 * p1;
            //            Enumerable.Range(1, 3).Select(fw);
        }

        private delegate int FuncWannabe(int p1);
        private void UseFuncWannabe(FuncWannabe wannabe) { }

        [Test]
        public void TestYieldReturnOverride()
        {
            var y1 = new YieldDerivedNaive().Yield();
            Assert.AreEqual(1, y1.Count());
            Assert.AreEqual(2, y1.ElementAt(0));

            var y2 = new YieldDerivedAdvanced().Yield();
            Assert.AreEqual(2, y2.Count());
            Assert.AreEqual(1, y2.ElementAt(0));
            Assert.AreEqual(2, y2.ElementAt(1));
        }

        public class YieldBase
        {
            public virtual IEnumerable<int> Yield()
            {
                yield return 1;
            }
        }

        public class YieldDerivedNaive : YieldBase
        {
            public override IEnumerable<int> Yield()
            {
                base.Yield();
                yield return 2;
            }
        }

        public class YieldDerivedAdvanced : YieldBase
        {
            public override IEnumerable<int> Yield()
            {
                foreach (var i in base.Yield())
                {
                    yield return i;
                }

                yield return 2;
            }
        }

        public class GenericInheritance<T, U>
            where T : List<T>
            where U : List<T>, T
        {

        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGenericConv2()
        {
            var gc3 = typeof(GenericConv3<>);
            var gc3base = gc3.BaseType;

            var dyna = new DynamicMethod("op_Implicit", gc3base, new Type[] { gc3 }, gc3);
            // will crash because gc3 can't host this method - it has open generic parameters

            Assert.IsTrue(dyna.IsStatic);
            Assert.AreEqual("op_Implicit", dyna.Name);
            Assert.AreEqual(gc3base.MetadataToken, dyna.ReturnType.MetadataToken);
            Assert.AreEqual(gc3.MetadataToken, dyna.GetParameters()[0].ParameterType.MetadataToken);
        }

        private class GenericConv3<T> : GenericConv2<T, int> { }

        [Test]
        public void TestGenericConv()
        {
            GenericConv1<int> gc1;
            gc1 = new GenericConv2<double, int>();
        }

        private class GenericConv1<T> { }
        private class GenericConv2<T1, T2>
        {
            public static implicit operator T1(GenericConv2<T1, T2> o)
            {
                return default(T1);
            }

            public static implicit operator GenericConv1<int>(GenericConv2<T1, T2> o)
            {
                return null;
            }
        }

        [Test]
        public void TestNullableConvertible()
        {
            Assert.IsTrue(TypeDescriptor.GetConverter(typeof(bool?)).CanConvertTo(typeof(String)));
            Assert.IsTrue(TypeDescriptor.GetConverter(typeof(bool?)).CanConvertFrom(typeof(String)));
        }

        private class TestIndices
        {
            public int this[int index] { get { return 0; } }
            public int this[int index, int index2] { get { return 0; } }
        }

        [Test]
        public void TestStackTrace()
        {
            var trace = from frame in new StackTrace().GetFrames()
                        select new { Class = frame.GetMethod().DeclaringType.Name, Method = frame.GetMethod().Name };

            Assert.AreEqual(
                new { Class = "DotNetFrameworkTests", Method = "TestStackTrace" },
                trace.ElementAt(0));
        }

        [Test]
        public void TestPrio()
        {
            Assert.AreEqual(false, true ? 2 == 2 ? false : true : false);
            Assert.AreEqual(false, 2 + 3 * 2 == 10);
            Assert.AreEqual(true, (2 + 3) * 2 == 10);
        }

        private int outerX = 2;

        [Test]
        public void TestDelegateAndClosures()
        {
            var innerX = 2;

            Func<int, int> del = x => x * innerX + outerX;
            Assert.AreEqual(6, del(2));
        }

        [Test]
        public void TestAsQueryable()
        {
            var toQueryable = typeof(Queryable).GetMethods().Where(mi => mi.Name == "AsQueryable");
            Assert.AreEqual(2, toQueryable.Count());
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestConversion()
        {
            var tc = TypeDescriptor.GetConverter(typeof(byte));
            tc.ConvertFromInvariantString("65535");
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestConversion2()
        {
            var tc = TypeDescriptor.GetConverter(typeof(Company));
            tc.ConvertFromInvariantString("null");
        }

        [Test]
        public void TestAttributes()
        {
            Assert.IsTrue(typeof(MyClass).HasAttr<BaseAttribute>());
        }

        [Test]
        public void TestParameterInfo()
        {
            var regularParameterType = typeof(ICollection<>).GetMethod("CopyTo").GetParameters()[1].ParameterType;
            var genericParameterType = typeof(ICollection<>).GetMethod("Add").GetParameters()[0].ParameterType;

            Assert.IsNotNull(regularParameterType.FullName);
            Assert.IsNull(genericParameterType.FullName);
        }
    }
}
