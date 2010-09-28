using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Dynamic
{
    [DebuggerNonUserCode]
    public class SimpleMetaObject : DynamicMetaObject
    {
        // if set to true, the SMO will wrap all unhandled exceptions in BindException
        // sometimes this might be useful, e.g. when underlying domain logic must not throw
        // sometimes this might not be useful, since it will also wrap domain exceptions
        public bool WrapExceptions { get; set; }

        public override IEnumerable<String> GetDynamicMemberNames() { return Seq.Empty<String>(); }
        public SimpleMetaObject(Expression expression, Object proxee) : base(expression, BindingRestrictions.Empty, proxee) { }

        protected FallbackException Fallback() { return new FallbackException(); }
        public virtual Object BinaryOperation(BinaryOperationBinder binder, Object arg) { throw Fallback(); }
        public virtual Object Convert(ConvertBinder binder){ throw Fallback(); }
        public virtual Object CreateInstance(CreateInstanceBinder binder, Object[] args){ throw Fallback(); }
        public virtual void DeleteIndex(DeleteIndexBinder binder, Object[] indexes){ throw Fallback(); }
        public virtual void DeleteMember(DeleteMemberBinder binder){ throw Fallback(); }
        public virtual Object GetIndex(GetIndexBinder binder, Object[] indexes){ throw Fallback(); }
        public virtual Object GetMember(GetMemberBinder binder){ throw Fallback(); }
        public virtual Object Invoke(InvokeBinder binder, Object[] args){ throw Fallback(); }
        public virtual Object InvokeMember(InvokeMemberBinder binder, Object[] args){ throw Fallback(); }
        public virtual void SetIndex(SetIndexBinder binder, Object[] indexes, Object value){ throw Fallback(); }
        public virtual void SetMember(SetMemberBinder binder, Object value){ throw Fallback(); }
        public virtual Object UnaryOperation(UnaryOperationBinder binder){ throw Fallback(); }

        #region BindXXX (emit expression tree shims that route dynamic dispatch to DispatchXXX)

        public sealed override DynamicMetaObject BindConvert(ConvertBinder binder)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindConvert(binder).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchConvert(binder);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchConvert", BF.PrivateInstance),
                    Expression.Constant(binder))),
                // 2. return dispatchResult.Success ? dispatchResult.Result : `fallback`;
                Expression.Condition(
                    Expression.Property(dispatchResult, "Success"),
                    Expression.Convert(Expression.Property(dispatchResult, "Result"), binder.ReturnType),
                    fallback,
                    binder.ReturnType));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindGetMember(binder).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchGetMember(binder);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchGetMember", BF.PrivateInstance),
                    Expression.Constant(binder))),
                // 2. return dispatchResult.Success ? dispatchResult.Result : `fallback`;
                Expression.Condition(
                    Expression.Property(dispatchResult, "Success"),
                    Expression.Convert(Expression.Property(dispatchResult, "Result"), binder.ReturnType),
                    fallback,
                    binder.ReturnType));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindSetMember(binder, value).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchSetMember(binder, value);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchSetMember", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression.Convert(value.Expression, typeof(Object)))),
                // 2. if (dispatchResult.Success) return value;
                //    else `fallback`;
                Expression.Condition(
                    Expression.Property(dispatchResult, "Success"),
                    Expression.Convert(value.Expression, typeof(Object)),
                    fallback,
                    binder.ReturnType));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindDeleteMember(binder).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchDeleteMember(binder);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchDeleteMember", BF.PrivateInstance),
                    Expression.Constant(binder))),
                // 2. if (!dispatchResult.Success) `fallback`;
                Expression.IfThen(
                    Expression.Not(Expression.Property(dispatchResult, "Success")),
                    fallback));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindGetIndex(binder, indexes).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchGetIndex(binder, indexes);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchGetIndex", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), indexes.Select(index => index.Expression)))),
                // 2. return dispatchResult.Success ? dispatchResult.Result : `fallback`;
                Expression.Condition(
                    Expression.Property(dispatchResult, "Success"),
                    Expression.Convert(Expression.Property(dispatchResult, "Result"), binder.ReturnType),
                    fallback,
                    binder.ReturnType));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindSetIndex(binder, indexes, value).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchSetIndex(binder, indexes, value);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchSetIndex", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), indexes.Select(index => index.Expression)),
                    Expression.Convert(value.Expression, typeof(Object)))),
                // 2. if (dispatchResult.Success) return value;
                //    else `fallback`;
                Expression.Condition(
                    Expression.Property(dispatchResult, "Success"),
                    Expression.Convert(value.Expression, typeof(Object)),
                    fallback,
                    binder.ReturnType));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindDeleteIndex(binder, indexes).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchDeleteIndex(binder, indexes);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchDeleteIndex", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), indexes.Select(index => index.Expression)))),
                // 2. if (!dispatchResult.Success) `fallback`;
                Expression.IfThen(
                    Expression.Not(Expression.Property(dispatchResult, "Success")),
                    fallback));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindInvokeMember(binder, args).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchInvokeMember(binder, args);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchInvokeMember", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), args.Select(arg => Expression.Convert(arg.Expression, typeof(Object)))))),
                // 2. return dispatchResult.Success ? dispatchResult.Result : `fallback`;
                Expression.Condition(
                    Expression.Property(dispatchResult, "Success"),
                    Expression.Convert(Expression.Property(dispatchResult, "Result"), binder.ReturnType),
                    fallback,
                    binder.ReturnType));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindInvoke(binder, args).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchInvoke(binder, args);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchInvoke", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), args.Select(arg => Expression.Convert(arg.Expression, typeof(Object)))))),
                // 2. return dispatchResult.Success ? dispatchResult.Result : `fallback`;
                Expression.Condition(
                    Expression.Property(dispatchResult, "Success"),
                    Expression.Convert(Expression.Property(dispatchResult, "Result"), binder.ReturnType),
                    fallback,
                    binder.ReturnType));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindCreateInstance(binder, args).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchCreateInstance(binder, args);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchCreateInstance", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), args.Select(arg => Expression.Convert(arg.Expression, typeof(Object)))))),
                // 2. return dispatchResult.Success ? dispatchResult.Result : `fallback`;
                Expression.Condition(
                    Expression.Property(dispatchResult, "Success"),
                    Expression.Convert(Expression.Property(dispatchResult, "Result"), binder.ReturnType),
                    fallback,
                    binder.ReturnType));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindUnaryOperation(binder).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchUnaryOperation(binder);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchUnaryOperation", BF.PrivateInstance),
                    Expression.Constant(binder))),
                // 2. return dispatchResult.Success ? dispatchResult.Result : `fallback`;
                Expression.Condition(
                    Expression.Property(dispatchResult, "Success"),
                    Expression.Convert(Expression.Property(dispatchResult, "Result"), binder.ReturnType),
                    fallback,
                    binder.ReturnType));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
        {
            var fallback = ((Func<Expression>)(() =>
            {
                try { return base.BindBinaryOperation(binder, arg).Expression; }
                catch (Exception ex) { return Expression.Throw(Expression.Constant(ex)); }
            }))();

            var dispatchResult = Expression.Parameter(typeof(DispatchResult), "dispatchResult");
            var shim = Expression.Block(dispatchResult.MkArray(),
                // 1. var dispatchResult = DispatchBinaryOperation(binder, arg);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchBinaryOperation", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression.Convert(arg.Expression, typeof(Object)))),
                // 2. return dispatchResult.Success ? dispatchResult.Result : `fallback`;
                Expression.Condition(
                    Expression.Property(dispatchResult, "Success"),
                    Expression.Convert(Expression.Property(dispatchResult, "Result"), binder.ReturnType),
                    fallback,
                    binder.ReturnType));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        #endregion

        #region DispatchXXX (boilerplate logic that connects dynamic dispatch requests to virtual methods of the class)

        [DebuggerNonUserCode] private class DispatchResult { public bool Success { get; set; } public Object Result { get; set; } }
        private DispatchResult Succeed(Object result = null) { return new DispatchResult {Success = true, Result = result}; }
        private DispatchResult Fail() { return new DispatchResult { Success = false }; }

        private DispatchResult DispatchBinaryOperation(BinaryOperationBinder binder, Object arg)
        {
            try
            {
                try
                {
                    var result = BinaryOperation(binder, arg);
                    if (result is FallbackException) throw (FallbackException)result;
                    return Succeed(result);
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) return Succeed(dynamic_object.BinaryOperation(binder, arg));
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    bind_args.Add("arg", arg);
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private DispatchResult DispatchConvert(ConvertBinder binder)
        {
            try
            {
                try
                {
                    var result = Convert(binder);
                    if (result is FallbackException) throw (FallbackException)result;
                    return Succeed(result);
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) return Succeed(dynamic_object.Convert(binder));
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                } 
            }
        }

        private DispatchResult DispatchCreateInstance(CreateInstanceBinder binder, Object[] args)
        {
            try
            {
                try
                {
                    var result = CreateInstance(binder, args);
                    if (result is FallbackException) throw (FallbackException)result;
                    return Succeed(result);
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) return Succeed(dynamic_object.CreateInstance(binder, args));
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    bind_args.Add("args", args);
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private DispatchResult DispatchDeleteIndex(DeleteIndexBinder binder, Object[] indexes)
        {
            try
            {
                try
                {
                    DeleteIndex(binder, indexes);
                    return Succeed();
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) { dynamic_object.DeleteIndex(binder, indexes); return Succeed(); }
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    bind_args.Add("indexes", indexes);
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private DispatchResult DispatchDeleteMember(DeleteMemberBinder binder)
        {
            try
            {
                try
                {
                    DeleteMember(binder);
                    return Succeed();
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) { dynamic_object.DeleteMember(binder); return Succeed(); }
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private DispatchResult DispatchGetIndex(GetIndexBinder binder, Object[] indexes)
        {
            try
            {
                try
                {
                    var result = GetIndex(binder, indexes);
                    if (result is FallbackException) throw (FallbackException)result;
                    return Succeed(result);
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) return Succeed(dynamic_object.GetIndex(binder, indexes));
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    bind_args.Add("indexes", indexes);
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private DispatchResult DispatchGetMember(GetMemberBinder binder)
        {
            try
            {
                try
                {
                    var result = GetMember(binder);
                    if (result is FallbackException) throw (FallbackException)result;
                    return Succeed(result);
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) return Succeed(dynamic_object.GetMember(binder));
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private DispatchResult DispatchInvoke(InvokeBinder binder, Object[] args)
        {
            try
            {
                try
                {
                    var result = Invoke(binder, args);
                    if (result is FallbackException) throw (FallbackException)result;
                    return Succeed(result);
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) return Succeed(dynamic_object.Invoke(binder, args));
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    bind_args.Add("args", args);
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private DispatchResult DispatchInvokeMember(InvokeMemberBinder binder, Object[] args)
        {
            try
            {
                try
                {
                    var result = InvokeMember(binder, args);
                    if (result is FallbackException) throw (FallbackException)result;
                    return Succeed(result);
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) return Succeed(dynamic_object.InvokeMember(binder, args));
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    bind_args.Add("args", args);
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private DispatchResult DispatchSetIndex(SetIndexBinder binder, Object[] indexes, Object value)
        {
            try
            {
                try
                {
                    SetIndex(binder, indexes, value);
                    return Succeed();
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) { dynamic_object.SetIndex(binder, indexes, value); return Succeed(); }
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    bind_args.Add("indexes", indexes);
                    bind_args.Add("value", value);
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private DispatchResult DispatchSetMember(SetMemberBinder binder, Object value)
        {
            try
            {
                try
                {
                    SetMember(binder, value);
                    return Succeed();
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) { dynamic_object.SetMember(binder, value); return Succeed(); }
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    bind_args.Add("value", value);
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private DispatchResult DispatchUnaryOperation(UnaryOperationBinder binder)
        {
            try
            {
                try
                {
                    var result = UnaryOperation(binder);
                    if (result is FallbackException) throw (FallbackException)result;
                    return Succeed(result);
                }
                catch (FallbackException)
                {
                    var dynamic_object = Value as IDynamicObject;
                    if (dynamic_object != null) return Succeed(dynamic_object.UnaryOperation(binder));
                    else throw;
                }
            }
            catch (FallbackException)
            {
                return Fail();
            }
            catch (Exception ex)
            {
                if (WrapExceptions)
                {
                    var bind_args = new OrderedDictionary<String, Object>();
                    throw new BindException(binder, bind_args, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion
    }
}
