using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Dynamic
{
    public abstract class DynamicProxy : DynamicMetaObject
    {
        public override IEnumerable<String> GetDynamicMemberNames() { return Seq.Empty<String>(); }
        protected DynamicProxy(Expression expression, Object proxee) : base(expression, BindingRestrictions.Empty, proxee) { }

        protected FallbackException Fallback() { throw new FallbackException(); }
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

        #region Boilerplate redirects from BindXXX to XXX

        public sealed override DynamicMetaObject BindConvert(ConvertBinder binder)
        {
            var fallback = base.BindConvert(binder);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("Convert", BF.PublicInstance),
                    Expression.Constant(binder)),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            var fallback = base.BindGetMember(binder);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("GetMember", BF.PublicInstance),
                    Expression.Constant(binder)),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            var fallback = base.BindSetMember(binder, value);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("SetMember", BF.PublicInstance),
                    Expression.Constant(binder),
                    value.Expression),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
        {
            var fallback = base.BindDeleteMember(binder);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("DeleteMember", BF.PublicInstance),
                    Expression.Constant(binder)),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
        {
            var fallback = base.BindGetIndex(binder, indexes);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("GetIndex", BF.PublicInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), indexes.Select(index => index.Expression))),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
        {
            var fallback = base.BindSetIndex(binder, indexes, value);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("SetIndex", BF.PublicInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), indexes.Select(index => index.Expression)),
                    value.Expression),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));

        }

        public sealed override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
        {
            var fallback = base.BindDeleteIndex(binder, indexes);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("DeleteIndex", BF.PublicInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), indexes.Select(index => index.Expression))),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            var fallback = base.BindInvokeMember(binder, args);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("InvokeMember", BF.PublicInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), args.Select(arg => arg.Expression))),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
        {
            var fallback = base.BindInvoke(binder, args);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("Invoke", BF.PublicInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), args.Select(arg => arg.Expression))),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
        {
            var fallback = base.BindCreateInstance(binder, args);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("CreateInstance", BF.PublicInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), args.Select(arg => arg.Expression))),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
        {
            var fallback = base.BindUnaryOperation(binder);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("UnaryOperation", BF.PublicInstance),
                    Expression.Constant(binder)),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public sealed override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
        {
            var fallback = base.BindBinaryOperation(binder, arg);
            var shim = Expression.TryCatch(
                Expression.Call(
                    Expression.Constant(this),
                    this.GetType().GetMethod("BinaryOperation", BF.PublicInstance),
                    Expression.Constant(binder),
                    Expression.NewArrayInit(typeof(Object), arg.Expression)),
                Expression.Catch(typeof(FallbackException),
                    fallback.Expression
                ));
            return new DynamicMetaObject(shim, BindingRestrictions.GetTypeRestriction(Expression, LimitType));

        }

        #endregion
    }
}
