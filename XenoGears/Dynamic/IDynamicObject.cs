using System;
using System.Dynamic;

namespace XenoGears.Dynamic
{
    public interface IDynamicObject : IDynamicMetaObjectProvider
    {
        Object BinaryOperation(BinaryOperationBinder binder, Object arg);
        Object Convert(ConvertBinder binder);
        Object CreateInstance(CreateInstanceBinder binder, Object[] args);
        void DeleteIndex(DeleteIndexBinder binder, Object[] indexes);
        void DeleteMember(DeleteMemberBinder binder);
        Object GetIndex(GetIndexBinder binder, Object[] indexes);
        Object GetMember(GetMemberBinder binder);
        Object Invoke(InvokeBinder binder, Object[] args);
        Object InvokeMember(InvokeMemberBinder binder, Object[] args);
        void SetIndex(SetIndexBinder binder, Object[] indexes, Object value);
        void SetMember(SetMemberBinder binder, Object value);
        Object UnaryOperation(UnaryOperationBinder binder);
    }
}