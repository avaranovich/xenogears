using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Formats
{
    // todo.
    // 3) accessing keys/values and such for primitives throws an exception immediately

    public partial class Json : BaseDictionary<String, dynamic>, IDynamicMetaObjectProvider
    {
        private readonly Object _primitive;
        private readonly Dictionary<String, Json> _complex;
        public bool IsPrimitive { get { return _complex == null; } }
        public bool IsComplex { get { return _complex != null; } }

        #region Dictionary implementation

        public override int Count
        {
            get
            {
                IsComplex.AssertTrue();
                return _complex.Count();
            }
        }

        public override IEnumerator<KeyValuePair<String, dynamic>> GetEnumerator()
        {
            IsComplex.AssertTrue();
            return _complex.Cast<String, dynamic>().GetEnumerator();
        }

        public override bool ContainsKey(String key)
        {
            IsComplex.AssertTrue();
            return _complex.ContainsKey(key);
        }

        public override bool TryGetValue(String key, out dynamic value)
        {
            IsComplex.AssertTrue();
            if (ContainsKey(key))
            {
                value = Get(key);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public override bool IsReadOnly
        {
            get { return IsPrimitive; }
        }

        public override void Add(String key, dynamic value)
        {
            IsComplex.AssertTrue();
        }

        public override bool Remove(String key)
        {
            IsComplex.AssertTrue();
            return _complex.Remove(key);
        }

        public override void Clear()
        {
            IsComplex.AssertTrue();
            _complex.Clear();
        }

        protected override void SetValue(String key, dynamic value)
        {
            IsComplex.AssertTrue();
            Set(key, value);
        }

        #endregion

        #region Dynamic implementation

        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter) { return new DynamicProxy(this).GetMetaObject(parameter); }
        private class DynamicProxy : DynamicObject
        {
            private readonly Json _json;
            public DynamicProxy(Json json) { _json = json; }
        }

        private dynamic Get(String key)
        {
            throw new NotImplementedException();
        }

        private void Set(String key, dynamic value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
