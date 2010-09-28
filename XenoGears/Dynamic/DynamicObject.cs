using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace XenoGears.Dynamic
{
    public class DynamicObject : IDynamicObject
    {
        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new SimpleMetaObject(parameter, this);
        }

        protected FallbackException Fallback()
        {
            return new FallbackException();
        }

        public Object BinaryOperation(BinaryOperationBinder binder, Object arg)
        {
            throw Fallback();
        }

        public Object Convert(ConvertBinder binder)
        {
            throw Fallback();
        }

        public Object CreateInstance(CreateInstanceBinder binder, Object[] args)
        {
            throw Fallback();
        }

        public void DeleteIndex(DeleteIndexBinder binder, Object[] indexes)
        {
            throw Fallback();
        }

        public void DeleteMember(DeleteMemberBinder binder)
        {
            throw Fallback();
        }

        public Object GetIndex(GetIndexBinder binder, Object[] indexes)
        {
            throw Fallback();
        }

        public Object GetMember(GetMemberBinder binder)
        {
            throw Fallback();
        }

        public Object Invoke(InvokeBinder binder, Object[] args)
        {
            throw Fallback();
        }

        public Object InvokeMember(InvokeMemberBinder binder, Object[] args)
        {
            throw Fallback();
        }

        public void SetIndex(SetIndexBinder binder, Object[] indexes, Object value)
        {
            throw Fallback();
        }

        public void SetMember(SetMemberBinder binder, Object value)
        {
            throw Fallback();
        }

        public Object UnaryOperation(UnaryOperationBinder binder)
        {
            throw Fallback();
        }
    }
}