using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using NUnit.Framework;
using XenoGears.Assertions;
using XenoGears.Formats;
using XenoGears.Functional;
using XenoGears.Reflection;
using XenoGears.Reflection.Generics;

namespace XenoGears.Playground.Dynamic
{
    [TestFixture]
    public class DotNetFrameworkTests
    {
        private class Foo : DynamicObject
        {
            private readonly Object _foo;
            public Foo(Object foo) { _foo = foo; }

            public override DynamicMetaObject GetMetaObject(Expression parameter)
            {
                var @default = base.GetMetaObject(parameter);
                return new DynamicFoo(@default);
            }

            #region Boilerplate

            private class DynamicFoo : DynamicMetaObject
            {
                private readonly DynamicMetaObject _default;
                public DynamicFoo(DynamicMetaObject @default)
                    : base(@default.Expression, @default.Restrictions, @default.Value)
                {
                    _default = @default;
                }

                public override DynamicMetaObject BindConvert(ConvertBinder binder)
                {
                    var result = _default.BindConvert(binder);
                    return result;
                }

                public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
                {
                    throw new NotImplementedException();
                }

                public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
                {
                    throw new NotImplementedException();
                }

                public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
                {
                    throw new NotImplementedException();
                }

                public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
                {
                    throw new NotImplementedException();
                }

                public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
                {
                    throw new NotImplementedException();
                }

                public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
                {
                    throw new NotImplementedException();
                }

                public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
                {
                    throw new NotImplementedException();
                }

                public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
                {
                    throw new NotImplementedException();
                }

                public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
                {
                    throw new NotImplementedException();
                }

                public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
                {
                    throw new NotImplementedException();
                }

                public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
                {
                    throw new NotImplementedException();
                }

                public override IEnumerable<string> GetDynamicMemberNames()
                {
                    throw new NotImplementedException();
                }
            }

            #endregion

            public override bool TryConvert(ConvertBinder binder, out Object result)
            {
                if (binder.ReturnType == _foo.GetType()) { result = _foo; return true; }
                if (binder.ReturnType == typeof(String)) { result = _foo.ToString(); return true; }
                if (binder.ReturnType.SameMetadataToken(typeof(IEnumerable<>)))
                {
                    var t = binder.ReturnType.XGetGenericArguments().AssertSingle();
                    (t == _foo.GetType()).AssertTrue();
                    var arr = Array.CreateInstance(t, 1);
                    arr.SetValue(_foo, 0);
                    result = arr;
                    return true;
                }

                result = null; return false;
            }
        }

        [Test, ExpectedException(typeof(InvalidCastException))]
        public void TestCasts()
        {
            dynamic foo = new Foo(10);
//            var foo_int = (int)foo;
//            var foo_string = (String)foo;
            var foo_ints = ((IEnumerable<int>)foo).ToReadOnly();
//            var foo_bar = (DateTime)foo;
        }
    }
}
