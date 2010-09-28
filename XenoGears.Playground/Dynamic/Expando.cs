using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using XenoGears.Assertions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Dynamic;
using XenoGears.Functional;
using XenoGears.Strings;

namespace XenoGears.Playground.Dynamic
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy("System.Collections.Generic.Mscorlib_DictionaryDebugView`2,mscorlib,Version=2.0.0.0,Culture=neutral,PublicKeyToken=b77a5c561934e089")]
    internal class Expando : OrderedDictionary<String, Object>, IDynamicObject
    {
        public Expando() { }
        public Expando(IEnumerable<KeyValuePair<String, Object>> dictionary) { (dictionary ?? Seq.Empty<KeyValuePair<String, Object>>()).ForEach(kvp => this.Add(kvp.Key, kvp.Value)); }
        public override String ToString() { return this.Select(kvp => String.Format("{0} = {1}", kvp.Key, kvp.Value)).StringJoin(); }

        public static Expando operator !(Expando e)
        {
            if (e == null) return null;
            var result = new Expando();
            e.Keys.ForEach(k => result.Add(new String(k.Reverse().ToArray()), e[k]));
            return result;
        }

        public static Expando operator +(Expando e1, Expando e2)
        {
            if (e1 == null && e2 == null) return null;
            e1 = e1 ?? new Expando();
            e2 = e2 ?? new Expando();
            var result = new Expando(e1);
            e2.Keys.ForEach(k => result.Add(k, e2[k]));
            return result;
        }

        public static Expando operator -(Expando e1, Expando e2)
        {
            e1 = e1 ?? new Expando();
            e2 = e2 ?? new Expando();
            var result = new Expando(e1);
            e2.Keys.ForEach(k => result.Remove(k));
            return result;
        }

        public Object Invoke(InvokeBinder binder, Object[] args)
        {
            var map = args.ToDictionary((arg, i) => binder.CallInfo.ArgumentNames[i], arg => arg);
            var result = new Expando();
            this.Keys.ForEach(k =>
            {
                var v = this[k];
                var s = (v as String).Extract(@"^\$(?<key>.*)$");
                if (s != null && map.ContainsKey(s)) v = map[s];
                result.Add(k, v);
            });

            return result;
        }

        #region All other methods of IDynamicObject throw FallbackException

        [DebuggerNonUserCode]
        public Object BinaryOperation(BinaryOperationBinder binder, Object arg)
        {
            throw new FallbackException();
        }

        [DebuggerNonUserCode]
        public Object Convert(ConvertBinder binder)
        {
            throw new FallbackException();
        }

        [DebuggerNonUserCode]
        public Object CreateInstance(CreateInstanceBinder binder, Object[] args)
        {
            throw new FallbackException();
        }

        [DebuggerNonUserCode]
        public void DeleteIndex(DeleteIndexBinder binder, Object[] indexes)
        {
            throw new FallbackException();
        }

        [DebuggerNonUserCode]
        public void DeleteMember(DeleteMemberBinder binder)
        {
            throw new FallbackException();
        }

        [DebuggerNonUserCode]
        public Object GetIndex(GetIndexBinder binder, Object[] indexes)
        {
            throw new FallbackException();
        }

        [DebuggerNonUserCode]
        public Object GetMember(GetMemberBinder binder)
        {
            throw new FallbackException();
        }

        [DebuggerNonUserCode]
        public Object InvokeMember(InvokeMemberBinder binder, Object[] args)
        {
            throw new FallbackException();
        }

        [DebuggerNonUserCode]
        public void SetIndex(SetIndexBinder binder, Object[] indexes, Object value)
        {
            throw new FallbackException();
        }

        [DebuggerNonUserCode]
        public void SetMember(SetMemberBinder binder, Object value)
        {
            throw new FallbackException();
        }

        [DebuggerNonUserCode]
        public Object UnaryOperation(UnaryOperationBinder binder)
        {
            throw new FallbackException();
        }

        #endregion

        public DynamicMetaObject GetMetaObject(Expression expression) { return new ExpandoProxy(expression, this); }
        private class ExpandoProxy : SimpleMetaObject
        {
            public Expando proxee { get { return Value.AssertCast<Expando>().AssertNotNull(); } }
            public ExpandoProxy(Expression expression, Object proxee) : base(expression, proxee) {}
            public override IEnumerable<String> GetDynamicMemberNames() { return proxee.Keys; }

            [DebuggerNonUserCode]
            public override Object BinaryOperation(BinaryOperationBinder binder, Object arg)
            {
                var e1 = proxee;
                var e2 = arg.AssertCast<Expando>();
                if (binder.Operation == ExpressionType.Add) return e1 + e2;
                else throw Fallback();
            }

            [DebuggerNonUserCode]
            public override Object Convert(ConvertBinder binder)
            {
                if (binder.Type == typeof(String)) return proxee.Select(kvp => String.Format("{0} = {1}", kvp.Key, kvp.Value == null ? "null" : kvp.Value.ToInvariantString())).StringJoin();
                else throw Fallback();
            }

            public override Object CreateInstance(CreateInstanceBinder binder, Object[] args)
            {
                throw new NotImplementedException();
            }

            public override void DeleteIndex(DeleteIndexBinder binder, Object[] indexes)
            {
                throw new NotImplementedException();
            }

            public override void DeleteMember(DeleteMemberBinder binder)
            {
                throw new NotImplementedException();
            }

            public override Object GetIndex(GetIndexBinder binder, Object[] indexes)
            {
                var key = indexes.AssertSingle().AssertCast<String>();
                return proxee[key];
            }

            public override Object GetMember(GetMemberBinder binder)
            {
                var key = binder.Name;
                return proxee[key];
            }

            public override Object Invoke(InvokeBinder binder, Object[] args)
            {
                return Fallback();
            }

            public override Object InvokeMember(InvokeMemberBinder binder, Object[] args)
            {
                var key = binder.Name;
                if (proxee.ContainsKey(key))
                {
                    var member = proxee[key].AssertCast<Delegate>().AssertNotNull();
                    return member.DynamicInvoke(args);
                }
                else
                {
                    return Fallback();
                }
            }

            public override void SetIndex(SetIndexBinder binder, Object[] indexes, Object value)
            {
                var key = indexes.AssertSingle().AssertCast<String>();
                proxee[key] = value;
            }

            public override void SetMember(SetMemberBinder binder, Object value)
            {
                var key = binder.Name;
                proxee[key] = value;
            }

            public override Object UnaryOperation(UnaryOperationBinder binder)
            {
                if (binder.Operation == ExpressionType.Not) return !proxee;
                else throw Fallback();
            }
        }
    }
}