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
    public partial class Json : BaseDictionary<dynamic, dynamic>, IDynamicMetaObjectProvider
    {
        private Object _my_primitive = null;
        private OrderedDictionary<String, Json> _my_complex = new OrderedDictionary<String, Json>();
        protected State _my_state = 0;
        protected enum State { Primitive = 1, Object, Array }

        private readonly Json _wrappee;
        private Object _primitive { get { return _wrappee != null ? _wrappee._primitive : _my_primitive; } }
        private OrderedDictionary<String, Json> _complex { get { return _wrappee != null ? _wrappee._complex : _my_complex; } }
        private State _state { get { return _wrappee != null ? _wrappee._state : _my_state; } set { if (_wrappee != null) _wrappee._state = value; else _my_state = value; } }

        public bool IsPrimitive { get { return _state == State.Primitive; } }
        public bool IsComplex { get { return IsObject || IsArray; } }
        public bool IsObject { get { return _state == State.Object; } }
        public bool IsArray { get { return _state == State.Array; } }

        #region Dynamic keys/values

        private String ImportKey(dynamic key)
        {
            if (key == null) throw AssertionHelper.Fail();
            if (key is String) { IsObject.AssertTrue(); return key; }
            if (key is sbyte) { IsArray.AssertTrue(); return key.ToString(); }
            if (key is byte) { IsArray.AssertTrue(); return key.ToString(); }
            if (key is short) { IsArray.AssertTrue(); return key.ToString(); }
            if (key is ushort) { IsArray.AssertTrue(); return key.ToString(); }
            if (key is int) { IsArray.AssertTrue(); return key.ToString(); }
            if (key is uint) { IsArray.AssertTrue(); return key.ToString(); }
            if (key is long) { IsArray.AssertTrue(); return key.ToString(); }
            if (key is ulong) { IsArray.AssertTrue(); return key.ToString(); }
            throw AssertionHelper.Fail();
        }

        private dynamic ExportKey(String key)
        {
            if (key == null) throw AssertionHelper.Fail();
            return IsArray ? int.Parse(key) : (dynamic)key;
        }

        private Json ImportValue(dynamic value)
        {
            return new Json(value);
        }

        private dynamic ExportValue(Json value)
        {
            return value;
        }

        #endregion

        #region Dynamic proxy

        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter) { return new DynamicJson(this).GetMetaObject(parameter); }
        private class DynamicJson : DynamicObject
        {
            private readonly Json _json;
            public DynamicJson(Json json) { _json = json; }

            public override IEnumerable<String> GetDynamicMemberNames()
            {
                var keys = _json.Keys.Cast<String>();
                return _json.IsArray ? keys.OrderBy(key => int.Parse(key)) : keys;
            }

            // todo. this won't work for interfaces => need to hack Microsoft.CSharp for that
            public override bool TryConvert(ConvertBinder binder, out Object result)
            {
                if (binder.Type == typeof(Json))
                {
                    result = _json;
                    return true;
                }

                result = _json.Deserialize(binder.Type);
                return true;
            }

            public override bool TryGetMember(GetMemberBinder binder, out Object result)
            {
                result = _json[binder.Name];
                return true;
            }

            public override bool TrySetMember(SetMemberBinder binder, Object value)
            {
                value = _json.ImportValue(value);
                _json[binder.Name] = value;
                return true;
            }
        }

        #endregion

        #region Collection API

        public override bool IsReadOnly
        {
            get { return IsPrimitive; }
        }

        public override int Count
        {
            get
            {
                if (_complex == null) return 0;
                return _complex.Count();
            }
        }

        public override IEnumerator<KeyValuePair<dynamic, dynamic>> GetEnumerator()
        {
            if (_complex == null) return Seq.Empty<KeyValuePair<dynamic, dynamic>>().GetEnumerator();
            return _complex.ToDictionary(kvp => ExportKey(kvp.Key), kvp => ExportValue(kvp.Value)).GetEnumerator();
        }

        public override bool TryGetValue(dynamic key, out dynamic value)
        {
            IsComplex.AssertTrue();
            if (ContainsKey(ImportKey(key)))
            {
                value = ExportValue(_complex[ImportKey(key)]);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        protected override void SetValue(dynamic key, dynamic value)
        {
            IsComplex.AssertTrue();
            _complex[ImportKey(key)] = ImportValue(value);
        }

        public override bool Remove(dynamic key)
        {
            IsComplex.AssertTrue();
            return _complex.Remove(ImportKey(key));
        }

        public override void Clear()
        {
            IsComplex.AssertTrue();
            _complex.Clear();
        }

        #region Object-specific API

        public override bool ContainsKey(dynamic key)
        {
            IsObject.AssertTrue();
            return _complex.ContainsKey(ImportKey(key));
        }

        public bool ContainsValue(dynamic value)
        {
            IsObject.AssertTrue();
            return IndexOf(ImportValue(value)) != -1;
        }

        public override void Add(dynamic key, dynamic value)
        {
            IsObject.AssertTrue();
            _complex.Add(ImportKey(key), ImportValue(value));
        }

        #endregion

        #region Array-specific API

        public void Add(dynamic value)
        {
            IsArray.AssertTrue();
            var max_index = _complex.Keys.Select(s => int.Parse(s)).MaxOrDefault(-1);
            Add(max_index + 1, value);
        }

        public bool Contains(dynamic value)
        {
            IsArray.AssertTrue();
            return IndexOf(ImportValue(value)) != -1;
        }

        public int IndexOf(dynamic value)
        {
            IsArray.AssertTrue();
            return _complex.IndexOf(kvp => Equals(kvp.Value, ImportValue(value)));
        }

        public void Insert(int index, dynamic value)
        {
            IsArray.AssertTrue();

            var before = _complex.Where(kvp => int.Parse(kvp.Key) < index).ToDictionary();
            var inserted = new KeyValuePair<String, Json>(ImportKey(index), ImportValue(value)).MkArray().ToDictionary();
            var after = _complex.Where(kvp => int.Parse(kvp.Key) >= index).ToDictionary(kvp => (int.Parse(kvp.Key) + 1).ToString(), kvp => kvp.Value);

            _complex.Clear();
            _complex.AddElements(before);
            _complex.AddElements(inserted);
            _complex.AddElements(after);
        }

        public void RemoveAt(int index)
        {
            IsArray.AssertTrue();
            _complex.Remove(ImportKey(index));
        }

        #endregion

        #endregion

        #region Equality boilerplate

        public static bool operator !=(Object value, Json json) { return !(value == json); }
        public static bool operator ==(Object value, Json json) { return Equals(json, value); }
        public static bool operator !=(Json json, Object value) { return !(json == value); }
        public static bool operator ==(Json json, Object value) { return Equals(json, value); }
        public static bool operator !=(Json json, Json value) { return !(json == value); }
        public static bool operator ==(Json json, Json value) { return Equals(json, value); }

        public override bool Equals(Object obj)
        {
            if (!(obj is Json)) return Equals(new Json(obj));

            var other = obj as Json;
            if (this.IsPrimitive || other.IsPrimitive)
            {
                if (this.IsPrimitive ^ other.IsPrimitive) return false;
                return Equals(this._primitive, other._primitive);
            }
            else if (this.IsArray || other.IsArray)
            {
                if (this.IsArray ^ other.IsArray) return false;
                return Seq.Equal(this._complex.OrderBy(kvp => int.Parse(kvp.Key)), other._complex.OrderBy(kvp => int.Parse(kvp.Key)));
            }
            else if (this.IsObject || other.IsObject)
            {
                if (this.IsObject ^ other.IsObject) return false;
                return Seq.Equal(this._complex.OrderBy(kvp => kvp.Key), other._complex.OrderBy(kvp => kvp.Key));
            }
            else if (this.IsComplex || other.IsComplex)
            {
                this._complex.AssertEmpty();
                other._complex.AssertEmpty();
                return true;
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        public override int GetHashCode()
        {
            var state = IsPrimitive ? 1 : IsArray ? 2 : IsObject ? 3 : IsComplex ? 4 : ((Func<int>)(() => { throw AssertionHelper.Fail(); }))();
            var ordered = (_complex ?? new OrderedDictionary<String, Json>()).OrderBy(kvp => IsArray ? (Object)int.Parse(kvp.Key) : kvp.Key);
            return ordered.Fold(state, (curr, kvp) => curr ^ kvp.GetHashCode());
        }

        #endregion
    }
}
