using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using XenoGears.Assertions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Reflection.Simple;

namespace XenoGears.Dynamic
{
    [DebuggerNonUserCode]
    public class SimpleMetaObject : DynamicMetaObject
    {
        // if set to true, the SMO will wrap all unhandled exceptions in BindException
        // sometimes this might be useful, e.g. when underlying domain logic must not throw
        // sometimes this might not be useful, since it will also wrap domain exceptions
        public bool WrapExceptions { get; set; }

        public new Object Value { get { return base.Value; } set { this.Set("_value", value); } }
        public SimpleMetaObject(Expression expression, Object value) : base(expression, BindingRestrictions.Empty, value) { }

        protected FallbackException Fallback() { return new FallbackException(); }
        public override IEnumerable<String> GetDynamicMemberNames() { throw AssertionHelper.Fail(); }
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
                // 1. var dispatchResult = DispatchConvert(binder, `expression`);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchConvert", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression)),
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
                // 1. var dispatchResult = DispatchGetMember(binder, `expression`);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchGetMember", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression)),
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
                // 1. var dispatchResult = DispatchSetMember(binder, `expression`, value);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchSetMember", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression,
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
                // 1. var dispatchResult = DispatchDeleteMember(binder, `expression`);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchDeleteMember", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression)),
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
                // 1. var dispatchResult = DispatchGetIndex(binder, `expression`, indexes);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchGetIndex", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression,
                    Expression.NewArrayInit(typeof(Object), indexes.Select(index => Expression.Convert(index.Expression, typeof(Object)))))),
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
                // 1. var dispatchResult = DispatchSetIndex(binder, `expression`, indexes, value);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchSetIndex", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression,
                    Expression.NewArrayInit(typeof(Object), indexes.Select(index => Expression.Convert(index.Expression, typeof(Object)))),
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
                // 1. var dispatchResult = DispatchDeleteIndex(binder, `expression`, indexes);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchDeleteIndex", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression,
                    Expression.NewArrayInit(typeof(Object), indexes.Select(index => Expression.Convert(index.Expression, typeof(Object)))))),
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
                // 1. var dispatchResult = DispatchInvokeMember(binder, `expression`, args);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchInvokeMember", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression,
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
                // 1. var dispatchResult = DispatchInvoke(binder, `expression`, args);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchInvoke", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression,
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
                // 1. var dispatchResult = DispatchCreateInstance(binder, `expression`, args);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchCreateInstance", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression,
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
                // 1. var dispatchResult = DispatchUnaryOperation(binder, `expression`);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchUnaryOperation", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression)),
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
                // 1. var dispatchResult = DispatchBinaryOperation(binder, `expression`, arg);
                Expression.Assign(dispatchResult, Expression.Call(
                    Expression.Constant(this),
                    typeof(SimpleMetaObject).GetMethod("DispatchBinaryOperation", BF.PrivateInstance),
                    Expression.Constant(binder),
                    Expression,
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

        private DispatchResult DispatchBinaryOperation(BinaryOperationBinder binder, Object arg1, Object arg2)
        {
            var old_value = Value;
            Value = arg1;

            try
            {
                try
                {
                    try
                    {
                        var result = BinaryOperation(binder, arg2);
                        if (result is FallbackException) throw (FallbackException)result;
                        return Succeed(result);
                    }
                    catch (FallbackException)
                    {
                        var dynamic_object = Value as IDynamicObject;
                        if (dynamic_object != null) return Succeed(dynamic_object.BinaryOperation(binder, arg2));
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
                        bind_args.Add("arg1", arg1);
                        bind_args.Add("arg2", arg2);
                        throw new BindException(binder, bind_args, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Value = old_value;
            }
        }

        private DispatchResult DispatchConvert(ConvertBinder binder, Object target)
        {
            var old_value = Value;
            Value = target;

            try
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
                        bind_args.Add("target", target);
                        throw new BindException(binder, bind_args, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Value = old_value;
            }
        }

        private DispatchResult DispatchCreateInstance(CreateInstanceBinder binder, Object target, Object[] args)
        {
            var old_value = Value;
            Value = target;

            try
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
                        bind_args.Add("target", target);
                        bind_args.Add("args", args);
                        throw new BindException(binder, bind_args, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Value = old_value;
            }
        }

        private DispatchResult DispatchDeleteIndex(DeleteIndexBinder binder, Object @this, Object[] indexes)
        {
            var old_value = Value;
            Value = @this;

            try
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
                        bind_args.Add("this", @this);
                        bind_args.Add("indexes", indexes);
                        throw new BindException(binder, bind_args, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Value = old_value;
            }
        }

        private DispatchResult DispatchDeleteMember(DeleteMemberBinder binder, Object @this)
        {
            var old_value = Value;
            Value = @this;

            try
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
                        bind_args.Add("this", @this);
                        throw new BindException(binder, bind_args, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Value = old_value;
            }
        }

        private DispatchResult DispatchGetIndex(GetIndexBinder binder, Object @this, Object[] indexes)
        {
            var old_value = Value;
            Value = @this;

            try
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
                        bind_args.Add("this", @this);
                        bind_args.Add("indexes", indexes);
                        throw new BindException(binder, bind_args, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Value = old_value;
            }
        }

        private DispatchResult DispatchGetMember(GetMemberBinder binder, Object @this)
        {
            var old_value = Value;
            Value = @this;

            try
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
                        bind_args.Add("this", @this);
                        throw new BindException(binder, bind_args, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Value = old_value;
            }
        }

        private DispatchResult DispatchInvoke(InvokeBinder binder, Object @this, Object[] args)
        {
            var old_value = Value;
            Value = @this;

            try
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
                        bind_args.Add("this", @this);
                        bind_args.Add("args", args);
                        throw new BindException(binder, bind_args, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Value = old_value;
            }
        }

        private DispatchResult DispatchInvokeMember(InvokeMemberBinder binder, Object @this, Object[] args)
        {
            var old_value = Value;
            Value = @this;

            try
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
                        bind_args.Add("this", @this);
                        bind_args.Add("args", args);
                        throw new BindException(binder, bind_args, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Value = old_value;
            }
        }

        private DispatchResult DispatchSetIndex(SetIndexBinder binder, Object @this, Object[] indexes, Object value)
        {
            var old_value = Value;
            Value = @this;

            try
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
                        bind_args.Add("this", @this);
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
            finally
            {
                Value = old_value;
            }
        }

        private DispatchResult DispatchSetMember(SetMemberBinder binder, Object @this, Object value)
        {
            var old_value = Value;
            Value = @this;

            try
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
                        bind_args.Add("this", @this);
                        bind_args.Add("value", value);
                        throw new BindException(binder, bind_args, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Value = old_value;
            }
        }

        private DispatchResult DispatchUnaryOperation(UnaryOperationBinder binder, Object arg1)
        {
            var old_value = Value;
            Value = arg1;

            try
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
                        bind_args.Add("arg1", arg1);
                        throw new BindException(binder, bind_args, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Value = old_value;
            }
        }

        #endregion
    }
}
