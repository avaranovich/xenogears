using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using XenoGears.Reflection;
using XenoGears.Reflection.Attributes;

namespace XenoGears.Playground.Reflection.Generic
{
    [TestFixture]
    public class GenericsTests
    {
        public class IsEnumAttribute : Attribute{}

        [Test]
        public void TestGenericParametersAttributes()
        {
            var m = MethodBase.GetCurrentMethod().DeclaringType.GetMethod("M");
            var etype = m.GetParameters()[0].ParameterType;
            var etypedef = m.GetGenericArguments().Where(arg => arg.Name == etype.Name).Single();

            Assert.IsTrue(etype.HasAttr<IsEnumAttribute>());
            Assert.IsTrue(etypedef.HasAttr<IsEnumAttribute>());
        }

        public void M<[IsEnum] E>(E t){}

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestConstructingNullable()
        {
            var t = typeof(IEnumerable<>).GetGenericArguments()[0];
            typeof(Nullable<>).MakeGenericType(t); // OOOOOOOOOOOOOOOOOOOOOPS!!!
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestCSharpSpec_Section_25_7_4()
        {
            Foo<int>.Bar1(7); //succeeds
            Foo<int>.Bar2(7); //crashes
        }

        class Foo<T>
        {
            // compile-time error
//            public static long Bar0(T t) { return (long)t; }

            public static long Bar1(T t) { return (long)(int)(object)t; }
            public static long Bar2(T t) { return (long)(object)t; }
        }

        // compilation error -> there don't exist generic field or properties
//        private class GenericProps
//        {
//            P Prop<P>{get{return default(P);}}
//        }

        [Test]
        public void TestInnerGenericTypes()
        {
            var keys = typeof(Dictionary<,>.KeyCollection);
            Assert.IsTrue(keys.IsGenericType);
            Assert.AreEqual(2, keys.GetGenericArguments().Length);
        }

        [Test]
        public void TestGenericTypeArgs4()
        {
            // Checking whether Ts from two nearby signatures from the same class are different
            // Answer: yes

            var t_enumerable = typeof(Enumerable);
            var where1 = t_enumerable.GetMethods().Where(mi => mi.Name == "Where").Take(1).Single();
            var where2 = t_enumerable.GetMethods().Where(mi => mi.Name == "Where").Skip(1).Single();

            Assert.AreNotEqual(
                where1.GetGenericArguments()[0].MetadataToken,
                where2.GetGenericArguments()[0].MetadataToken);
        }

        [Test]
        public void TestGenericTypeArgs3()
        {
            // Checking whether it is possible to construct generics with the use of 
            // constrained parameters from other generics and preserve the constraints
            // Answer: yes

            var t_nullable = typeof(Nullable<>);
            var constrainedArg = t_nullable.GetGenericArguments()[0];

            Assert.AreEqual(1, constrainedArg.GetGenericParameterConstraints().Length);

            var tlist_genericdef = typeof(List<>);
            var tlist = tlist_genericdef.MakeGenericType(constrainedArg);
            var tlist_arg = tlist.GetGenericArguments()[0];

            Assert.AreEqual(1, tlist_arg.GetGenericParameterConstraints().Length);
        }

        [Test]
        public void TestGenericTypeArgs2()
        {
            // Checking if it is possible to construct generics with the use of parameters from other generics
            // Answer: fuck YEAH

            var mcomparer = typeof(IComparer<>).GetMethod("Compare");
            var tlist_genericdef = typeof(List<>);
            var mlist = tlist_genericdef.GetMethod("Add");

            // T from IComparer<T>.Compare(T, T) and T from List<T>.Add(T)
            // those two types are actually different ones!

            Assert.AreNotEqual(
                mlist.GetParameters()[0].ParameterType.MetadataToken,
                mcomparer.GetParameters()[0].ParameterType.MetadataToken);

            // Despite of both T's being IsGenericTypeParameter types
            // you can use those in calls to MakeGenericType/MakeGenericMethod

            Assert.IsTrue(mcomparer.GetParameters()[0].ParameterType.IsGenericParameter);
            Assert.IsTrue(tlist_genericdef.IsGenericTypeDefinition);

            var tlist_closed = tlist_genericdef.MakeGenericType(mcomparer.GetParameters()[0].ParameterType);

            // Forged types are (surprisingly) not generic type definitions
            // Tho you can't instantiate such forged types

            Assert.IsFalse(tlist_closed.IsGenericTypeDefinition);

            var argExCaught = false;
            try { Activator.CreateInstance(tlist_closed); }
            catch (ArgumentException) { argExCaught = true; }
            finally { Assert.IsTrue(argExCaught); }

            // Actually, you can also use these T's to construct methods, i.e. these types
            // are NORMAL TYPES in type system of .NET

            var mlist_closed = tlist_closed.GetMethod("Add");

            Assert.AreEqual(
                mlist_closed.GetParameters()[0].ParameterType.MetadataToken,
                mcomparer.GetParameters()[0].ParameterType.MetadataToken);
        }

        [Test]
        public void TestGenericTypeArgs1()
        {
            // Checking whether different "T"s in open generics are actually different types
            // Answer: yes, they are even for inherited methods

            var mi1 = typeof(List<>).GetMethod("Add");
            var mi2 = typeof(IComparer<>).GetMethod("Compare");
            var mi3 = typeof(ICollection<>).GetMethod("Add");

            var t1 = mi1.GetParameters()[0].ParameterType;
            var t2 = mi2.GetParameters()[0].ParameterType;
            var t3 = mi3.GetParameters()[0].ParameterType;

            Assert.AreEqual(t1.Name, t2.Name);
            Assert.AreEqual(t1.Name, t3.Name);
            Assert.AreEqual(t2.Name, t3.Name);

            Assert.AreNotEqual(t1.MetadataToken, t2.MetadataToken);
            Assert.AreNotEqual(t1.MetadataToken, t3.MetadataToken);
            Assert.AreNotEqual(t2.MetadataToken, t3.MetadataToken);

            // Checking whether "T"s yielded by different GetMethod calls are different
            // Answer: no, seems they're cached

            var mi22 = typeof(IComparer<>).GetMethod("Compare");
            var t22 = mi22.GetParameters()[0].ParameterType;

            Assert.AreEqual(t22.Name, t2.Name);
            Assert.AreEqual(t22.MetadataToken, t2.MetadataToken);

            // Checking whether generic methods inherit "T"s from their class definitions
            // Answer: actually yes

            var ilist = typeof(ICollection<>);
            var t_fromclass = ilist.GetGenericArguments()[0];
            var ilistadd = ilist.GetMethod("Add");
            var t_frommethod = ilistadd.GetParameters()[0].ParameterType;

            Assert.AreEqual(t_fromclass.Name, t_frommethod.Name);
            Assert.AreEqual(t_fromclass.MetadataToken, t_frommethod.MetadataToken);

            // Checking whether all of those types are generic parameters
            // Answer: yes, all of those

            Assert.IsTrue(t1.IsGenericParameter);
            Assert.IsTrue(t2.IsGenericParameter);
            Assert.IsTrue(t3.IsGenericParameter);
            Assert.IsTrue(t22.IsGenericParameter);
            Assert.IsTrue(t_fromclass.IsGenericParameter);
            Assert.IsTrue(t_frommethod.IsGenericParameter);
        }

        [Test]
        public void TestGenericConstraints2()
        {
            // Checking whether parameter types bear information about constraints
            // Answer: yeah they do, tho in a weird way

            var cons = typeof(GenericType<,,>);

            var attr1 = cons.GetGenericArguments()[0].GenericParameterAttributes;
            var attr2 = cons.GetGenericArguments()[1].GenericParameterAttributes;
            var attr3 = cons.GetGenericArguments()[2].GenericParameterAttributes;

            Assert.AreEqual(attr1, GenericParameterAttributes.ReferenceTypeConstraint |
                GenericParameterAttributes.DefaultConstructorConstraint);
            Assert.AreEqual(attr2, GenericParameterAttributes.NotNullableValueTypeConstraint |
                GenericParameterAttributes.DefaultConstructorConstraint);
            Assert.AreEqual(attr3, GenericParameterAttributes.None);

            var inh1 = cons.GetGenericArguments()[0].GetGenericParameterConstraints();
            var inh2 = cons.GetGenericArguments()[1].GetGenericParameterConstraints();
            var inh3 = cons.GetGenericArguments()[2].GetGenericParameterConstraints();

            Assert.AreEqual(0, inh1.Length); //[0]
            Assert.AreEqual(1, inh2.Length); //{ValueType}
            Assert.AreEqual(1, inh3.Length); //{IComparable`1}
            Assert.AreEqual(
                cons.GetGenericArguments()[1].MetadataToken,
                inh3[0].GetGenericArguments()[0].MetadataToken);

        }

        private class GenericType<T1, T2, T3>
            where T1 : class, new()
            where T2 : struct
            where T3 : IComparable<T2>
        {
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGenericConstraints1()
        {
            // Checking whether you can construct a closed generic with type args that contradict the constraints
            // Answer: no

            var nullable = typeof(Nullable<>);
            var nullableString = nullable.MakeGenericType(typeof(String));
        }

        public class GenericClass<T1>
        {
            public IEnumerable<T2> GenericMethod<T2>(KeyValuePair<T1, T2> kvp, T2 someStuff)
            {
                return null;
            }
        }

        [Test]
        public void TestGenericMethods()
        {
            var knownType = typeof(GenericClass<int>);
            var mi = knownType.GetMethod("GenericMethod");
        }

        public class BaseBaseBase { }
        public class BaseBaseGeneric<T> : BaseBaseBase { }
        public class BaseGeneric<T> : BaseBaseGeneric<T> { }
        public class Target : BaseGeneric<int> { }

        [Test]
        public void TestInheritanceAndGenerics()
        {
            var baseOpenGeneric = typeof(IEnumerable<>);
            var baseClosedGeneric = typeof(IEnumerable<int>);
            var inheritedType = typeof(List<int>);

            Assert.IsFalse(baseOpenGeneric.IsAssignableFrom(inheritedType));
            Assert.IsTrue(baseClosedGeneric.IsAssignableFrom(inheritedType));

            Assert.IsFalse(typeof(BaseBaseGeneric<>).IsAssignableFrom(typeof(Target)));
            Assert.IsTrue(typeof(BaseBaseGeneric<int>).IsAssignableFrom(typeof(Target)));
            Assert.IsFalse(typeof(BaseBaseGeneric<>).IsAssignableFrom(typeof(BaseGeneric<>)));

            Assert.IsTrue(typeof(BaseBaseBase).IsAssignableFrom(typeof(Target)));
            Assert.IsTrue(typeof(BaseBaseBase).IsAssignableFrom(typeof(BaseBaseGeneric<>)));
            Assert.IsTrue(typeof(BaseBaseBase).IsAssignableFrom(typeof(BaseBaseGeneric<int>)));
        }

        [Test]
        public void TestGenericArguments()
        {
            var openGeneric = typeof(KeyValuePair<,>);
            // the following leads to compilation error (unnoticed by my Resharper btw)
//            var halfOpenGeneric = typeof(KeyValuePair<int,>);
            var closedGeneric = typeof(KeyValuePair<int, int>);
            Assert.IsTrue(openGeneric.SameMetadataToken(closedGeneric));

            Assert.IsTrue(openGeneric.IsGenericType);
            Assert.IsTrue(openGeneric.IsGenericTypeDefinition);
            Assert.IsTrue(openGeneric.GetGenericArguments().Count() == 2);
            Assert.IsTrue(openGeneric.GetGenericArguments().ToArray()[0].Name == "TKey");

            Assert.IsTrue(closedGeneric.IsGenericType);
            Assert.IsFalse(closedGeneric.IsGenericTypeDefinition);
            Assert.IsTrue(closedGeneric.GetGenericArguments().Count() == 2);
            Assert.IsTrue(closedGeneric.GetGenericArguments().ToArray()[0].Name == "Int32");
        }

        public class BaseGeneric1<Type> { }
        public class Generic1<T> : BaseGeneric<T> { }

        [Test]
        public void TestGenericTypeNames()
        {
            var child = typeof(Generic1<>);
            var @base = child.BaseType;

            Assert.AreEqual("T", child.GetGenericArguments()[0].Name);
            Assert.AreEqual("T", @base.GetGenericArguments()[0].Name);
            Assert.AreNotEqual("T", typeof(BaseGeneric1<>).GetGenericArguments()[0].Name);
        }
    }
}