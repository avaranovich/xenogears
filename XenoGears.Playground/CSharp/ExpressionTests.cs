using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using NUnit.Framework;
using XenoGears.Playground.CSharp.Domain;
using XenoGears.Reflection;

namespace XenoGears.Playground.CSharp
{
    [TestFixture]
    public class ExpressionTests
    {
        [Test]
        public void TestBoxingToInterface()
        {
            Action<IFormattable> aifo = ifo => {};
            Expression<Action<DayOfWeek>> expr = e => aifo(e);
            Expression<Action<Enum>> expr2 = e => aifo(e);

            Assert.AreEqual(
                "e => Invoke(value(XenoGears.Playground.CSharp.ExpressionTests+<>c__DisplayClass2).aifo, Convert(e))", 
                expr.ToString());

            Assert.AreEqual(
                "e => Invoke(value(XenoGears.Playground.CSharp.ExpressionTests+<>c__DisplayClass2).aifo, e)", 
                expr2.ToString());
        }
        
        [Test]
        public void TestConditionalWithMethodGroup()
        {
            Func<bool> eh = () => true;
            Expression<Func<bool, Func<bool>>> expr = b => b ? eh : MethodGroupImpl;
            Expression<Func<bool, Func<bool>>> expr2 = b => b ? eh : () => true;

//            Assert.AreEqual(
//                "b => IIF(b, value(Playground.ExpressionTests+<>c__DisplayClass2).eh, "+
//                "Convert(CreateDelegate(System.Func`1[System.Boolean], value(Playground.ExpressionTests), Boolean MethodGroupImpl())))",
//                expr.ToString());
//
//            Assert.AreEqual(
//                "b => IIF(b, value(Playground.ExpressionTests+<>c__DisplayClass2).eh, () => True)",
//                expr2.ToString());
        }

        private bool MethodGroupImpl() { return true; }

        [Test]
        public void TestVarargsCall()
        {
            Expression<Action<ExpressionTests, double>> expr = (o, d) => 
                o.VarargsMethod(1, (int)1.0, (int)d);

            Assert.AreEqual("(o, d) => o.VarargsMethod(1, new [] {1, Convert(d)})", expr.ToString());
            Assert.IsTrue(((MethodCallExpression)expr.Body)
                .Arguments[1].NodeType == ExpressionType.NewArrayInit);
        }

        private void VarargsMethod(int i, params int[] ints){}

        [Test]
        public void TestMethodGroup()
        {
            Expression<Action<ExpressionTests>> expr = o => ((DMethodGroup)o.MethodGroup)(0);

            // expression tree actually contains a call to Delegate.CreateDelegate that 
            // gets handed already resolved signature as a MethodInfo constant expression.
            Assert.AreEqual(
                "o => Invoke(Convert(CreateDelegate(XenoGears.Playground.CSharp.ExpressionTests+DMethodGroup, o, Void MethodGroup(Int32))), 0)",
                expr.ToString());
        }

        public delegate void DMethodGroup(int i);
        public void MethodGroup(int i){}
        public void MethodGroup<T>(T i){}

        [Test]
        public void TestNewArrayBounds()
        {
            Expression<Func<int[]>> expr1 = () => new int[1];
            Assert.IsTrue(expr1.Body is NewArrayExpression);
            Assert.AreEqual(ExpressionType.NewArrayBounds, expr1.Body.NodeType);

            Expression<Func<int[,]>> expr2 = () => new int[1, 2];
            Assert.IsTrue(expr2.Body is NewArrayExpression);
            Assert.AreEqual(ExpressionType.NewArrayBounds, expr2.Body.NodeType);
        }

        [Test]
        public void TestNewArrayInit()
        {
            Expression<Func<int[]>> expr = () => new[] {1};
            Assert.IsTrue(expr.Body is NewArrayExpression);
            Assert.AreEqual(ExpressionType.NewArrayInit, expr.Body.NodeType);
        }

        [Test]
        public void TestNewWithMemberInits()
        {
            Expression<Func<TestC>> expr = () => new TestC()
                {
                    F1 = 0, 
                    F2 = new List<int>(){1, 2},
                    F3 = new TestC(){F1 = 0},
                    F4 = {0, 1},
                    F5 = {F1 = 0}
                };

            var b = (MemberInitExpression)expr.Body;
            Assert.IsTrue(b is MemberInitExpression);
            Assert.IsTrue(b.NewExpression is NewExpression);
            Assert.IsTrue(b.Bindings[0] is MemberAssignment);
            Assert.IsTrue(b.Bindings[1] is MemberAssignment);
            Assert.IsTrue(b.Bindings[2] is MemberAssignment);
            Assert.IsTrue(b.Bindings[3] is MemberListBinding);
            Assert.IsTrue(b.Bindings[4] is MemberMemberBinding);
        }

        public class TestC
        {
            public int F1 { get; set; }
            public List<int> F2 { get; set; }
            public TestC F3 { get; set; }
            public List<int> F4 { get; set; }
            public TestC F5 { get; set; }
        }

        [Test]
        public void TestListInit()
        {
            Expression<Func<int, List<int>>> expr = i => new List<int>{i, i + 1, i + 2};
            Assert.AreEqual("i => new List`1() {Void Add(Int32)(i), Void Add(Int32)((i + 1)), Void Add(Int32)((i + 2))}", expr.ToString());

            var b = (ListInitExpression)expr.Body;
            Assert.IsTrue(b is ListInitExpression);
            Assert.IsTrue(b.NewExpression is NewExpression);
            Assert.IsTrue(b.Initializers[0] is ElementInit);
        }

        [Test]
        public void TestInvokes()
        {
            var q1 =
                from c in (new C[0]).AsQueryable()
                let d = (DGetInt)c.GetInt
                select d();

            var q2 =
                from c in (new C[0]).AsQueryable()
                select c.D();

            var q3 = TestInvokesWithParam(new C().GetInt);
        }

        public Expression TestInvokesWithParam(DGetInt d)
        {
            return (
                from c in (new C[0]).AsQueryable()
                select d
            ).Expression;
        }

        public class C
        {
            public int GetInt() { return 0; }
            public DGetInt D;
        }
        public delegate int DGetInt();

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestUserDefinedConversion()
        {
            Expression<Action<B2>> expr = obj => Meth(obj);
            Assert.AreEqual("obj => value(XenoGears.Playground.CSharp.ExpressionTests).Meth(Convert(Convert(obj)))", expr.ToString());

            var @this = Expression.Constant(this);
            var b2 = Expression.Constant(new B2());
            var args = Expression.Convert(b2, typeof(int), typeof(B1).GetMethod("op_Implicit"));
            Expression.Call(@this, typeof(ExpressionTests).GetMethod("Meth", new Type[] { typeof(int) }), args);

            // the crash that will occur contradicts to C# spec: http://en.csharp-online.net/ECMA-334:_13.4.2_Evaluation_of_user-defined_conversions
            //
            // Once a most specific user-defined conversion operator has been identified, the actual execution of the userdefined conversion 
            // involves up to three steps: 
            // 1) First, if required, performing a standard conversion from the source type to the operand type of the userdefined conversion operator. 
            // 2) Next, invoking the user-defined conversion operator to perform the conversion. 
            // 3) Finally, if required, performing a standard conversion from the result type of the user-defined conversion operator to the target type.
        }

        private void Meth(int i){}
        public class B1 { public static implicit operator byte(B1 b1) { return 0; } }
        public class B2 : B1 { }

        [Test]
        public void TestUserDefinedIndexers()
        {
            Expression<Func<Company, int>> expr = c => c[2, 4];
            Assert.AreEqual("c => c.get_Item(2, 4)", expr.ToString());
        }

        [Test]
        public void TestArrayIndexes()
        {
            var ja = new int[1][];
            Expression<Func<int>> jagged = () => ja[1][2];
            Assert.IsTrue(Regex.IsMatch(
                jagged.ToString(),
                @"\(\) => value\(.*<>c__DisplayClass.?\)\.ja\[1\]\[2\]"));

            var ra1 = new int[1];
            Expression<Func<int>> rect1 = () => ra1[1];
            Assert.IsTrue(Regex.IsMatch(
                rect1.ToString(),
                @"\(\) => value\(.*<>c__DisplayClass.?\)\.ra1\[1\]"));

            var ra2 = new int[1,1];
            Expression<Func<int>> rect2viaindex = () => ra2[1,2];
            Assert.IsTrue(Regex.IsMatch(
                rect2viaindex.ToString(),
                @"\(\) => value\(.*<>c__DisplayClass.?\)\.ra2.Get\(1, 2\)"));

            // get is a method that belongs to the int[,] type, but not to the Array types
            // that's why we can no way call it from the code

            Expression<Func<int>> rect2viaget = () => (int)ra2.GetValue(1, 2);
            Assert.IsTrue(Regex.IsMatch(
                rect2viaget.ToString(),
                @"\(\) => Convert\(value\(.*<>c__DisplayClass.?\)\.ra2.GetValue\(1, 2\)\)"));
        }

        [Test]
        public void TestPropertyInvocation()
        {
            Expression<Func<ExpressionTests, String>> expr = obj => obj.TestProp;
            Assert.AreEqual(ExpressionType.MemberAccess, expr.Body.NodeType);
        }

        private String TestProp { get; set; }

        [Test]
        public void TestLocalMethodCall()
        {
            Expression<Action> expr = () => TestLocalMethodCall();
            Assert.AreEqual(ExpressionType.Call, expr.Body.NodeType);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMethodCallWithNumericPromotions()
        {
            Func<long, int> f = d => (int)d;
            Expression<Func<int, long>> natural = i => f(i);
            Assert.IsTrue(Regex.IsMatch(
                natural.ToString(),
                @"i => Convert\(Invoke\(value\(.*<>c__DisplayClass.?\)\.f, Convert\(i\)\)\)"));

            Expression.Invoke(Expression.Constant(f), Expression.Constant(1));
        }

        [Test]
        public void TestMethodCallWithInheritanceBasedConversions()
        {
            Func<Expression, BinaryExpression> f = e => null;
            Expression<Func<BinaryExpression, Expression>> natural = i => f(i);
//            Assert.IsTrue(Regex.IsMatch(
//                natural.ToString(),
//                @"i => Invoke\(value\(.*<>c__DisplayClass.?\)\.f,i\)"));

            Expression.Invoke(
                Expression.Constant(f), 
                Expression.Constant(Expression.Add(Expression.Constant(1), Expression.Constant(1))));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNullableLifted()
        {
            Expression<Func<int?, object>> natural = i => (int?)2 * 2;
            Assert.AreEqual("i => Convert((Convert(2) * Convert(2)))", natural.ToString());

            // failboat once again (see TestArithmetic)
            var left = Expression.Constant(2, typeof(int?));
            var right = Expression.Constant(2);
            Expression.Multiply(left, right);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestArithmetic()
        {
            // 5 converts here
            Expression<Func<int, long>> expr = i => (byte)i + (byte)i;
            Assert.AreEqual("i => Convert((Convert(Convert(i)) + Convert(Convert(i))))", expr.ToString());

            // lol @ MS.. if types of arithmetics are not equal, then multiply will crash
            // of course this complies to the spec, but not to the common sense of convenience
            Expression.Multiply(Expression.Constant((byte)2), Expression.Constant((byte)2));

            // this stuff works quite fine
//            Expression.Multiply(
//                Expression.Convert(Expression.Constant(2, typeof(byte)), typeof(int)), 
//                Expression.Convert(Expression.Constant(2, typeof(byte)), typeof(int)));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestStringPlusObject()
        {
            // the shiz below works fine and produces the following:
            // Expression.Add(Expression.Constant("s"), Expression.Constant(2));
            // with the method set to Concat. I guess it's the hardcoding of a compiler
            Expression<Func<String, String>> expr = s => "s" + 2;
            Assert.AreEqual("s => (\"s\" + Convert(2))", expr.ToString());

            // tho the following will crash: 2 requires boxing. omfg to be honest.
            var concat = typeof(String).GetMethod("Concat", new Type[] { typeof(Object), typeof(Object) });
            Expression.Add(Expression.Constant("s"), Expression.Constant(2), concat);
        }

        [Test]
        public void TestMethodBasedExpressions()
        {
            Expression<Func<Test, Test, int>> expr = (to1, to2) => to1 + to2;
            Assert.IsNotNull(((BinaryExpression)expr.Body).Method);
        }

        private class Test
        {
            public static int operator +(Test t1, Test t2)
            {
                return 0;
            }
        }

//        Expression<Func<IEnumerable<Pew>, IEnumerable<int>>> expr = pews =>
//            from p in pews
//            let pp = 0
//            where p.Out(out pp)
//            select pp;

        private class Pew
        {
            // it's prohibited to use ref parameters in expressions
            public bool Ref(ref int a)
            {
                return false;
            }

            // it makes no sense to me to use side-effects in LINQ that is stateless by nature
            // what's more you can't use introduced variables (via Let) as targets for out
            public bool Out(out int a)
            {
                a = (int)DateTime.Now.Ticks;
                return true;
            }
        }
        
        public class StaticTest
        {
            public static bool PewBool() { return true; }
            public static void PewVoid() { }
        }

        [Test]
        public void TestExpressionTypeProp()
        {
            Expression callPewBool = Expression.Call(typeof(StaticTest), "PewBool", new Type[0]);
            Assert.IsTrue(typeof(bool).SameMetadataToken(callPewBool.Type));

            Expression callPewVoid = Expression.Call(typeof(StaticTest), "PewVoid", new Type[0]);
            Assert.IsTrue(typeof(void).SameMetadataToken(callPewVoid.Type));
        }
    }
}