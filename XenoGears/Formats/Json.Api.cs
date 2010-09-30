using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Assertions;
using XenoGears.Dynamic;
using XenoGears.Functional;

namespace XenoGears.Formats
{
    public partial class Json : BaseDictionary<dynamic, dynamic>, IDynamicMetaObjectProvider
    {
        internal Object _my_primitive = null;
        internal OrderedDictionary<String, Json> _my_complex = new OrderedDictionary<String, Json>();
        internal State _my_state = 0;
        internal enum State { Primitive = 1, Object, Array }

        internal readonly Json _wrappee;
        internal Object _primitive { get { return _wrappee != null ? _wrappee._primitive : _my_primitive; } }
        internal OrderedDictionary<String, Json> _complex { get { return _wrappee != null ? _wrappee._complex : _my_complex; } }
        internal State _state { get { return _wrappee != null ? _wrappee._state : _my_state; } set { if (_wrappee != null) _wrappee._state = value; else _my_state = value; } }

        public bool IsPrimitive { get { return _state == State.Primitive; } }
        public bool IsComplex { get { return IsObject || IsArray; } }
        public bool IsObject { get { return _state == State.Object; } }
        public bool IsArray { get { return _state == State.Array; } }

        #region Dynamic keys/values

        private String ImportKey(Object key)
        {
            if (key == null) throw AssertionHelper.Fail();
            if (key is String) { IsObject.AssertTrue(); return key.ToString(); }
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

        private Object ExportKey(String key)
        {
            if (key == null) throw AssertionHelper.Fail();
            return IsArray ? int.Parse(key) : (Object)key;
        }

        private Json ImportValue(Object value)
        {
            if (value is Json) return (Json)value;
            return new Json(value);
        }

        private Object ExportValue(Json value)
        {
            return value;
        }

        #endregion

        #region Dynamic proxy

        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression expression) { return new JsonProxy(expression, this); }
        [DebuggerNonUserCode] private class JsonProxy : SimpleMetaObject
        {
            private Json Json { get { return Value.AssertCast<Json>().AssertNotNull(); } }
            public JsonProxy(Expression expression, Object proxee) : base(expression, proxee) {}

            [DebuggerNonUserCode]
            public override IEnumerable<String> GetDynamicMemberNames()
            {
                if (Json.IsPrimitive) { return Seq.Empty<String>(); }
                else if (Json.IsObject) { return Json.Keys.Cast<String>(); }
                else if (Json.IsArray) { return Seq.Empty<String>(); }
//                else throw AssertionHelper.Fail();
                else { return Seq.Empty<String>(); }
            }

            public override Object Convert(ConvertBinder binder)
            {
                if (binder.Type == typeof(Object)) return Json;
                if (binder.Type == typeof(Json)) return Json;
                return Json.Deserialize(binder.Type);
            }

            [DebuggerNonUserCode]
            public override Object GetMember(GetMemberBinder binder)
            {
                var default_bind = typeof(Json).GetProperty(binder.Name) != null;
                if (default_bind) throw Fallback();

                return Json[binder.Name];
            }

            [DebuggerNonUserCode]
            public override void SetMember(SetMemberBinder binder, Object value)
            {
                var default_bind = typeof(Json).GetProperty(binder.Name) != null;
                if (default_bind) throw Fallback();

                Json[binder.Name] = value;
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

            // todo. wtf does the commented code crash?!
//            var imported = ImportKey(key);

            var containsKey = _complex.ContainsKey(ImportKey((Object)key));
            if (containsKey)
            {
                value = ExportValue(_complex[ImportKey((Object)key)]);
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
            // todo. wtf dynamic ain't work here as well
            _complex[ImportKey((Object)key)] = ImportValue((Object)value);
        }

        public override bool Remove(dynamic key)
        {
            IsComplex.AssertTrue();
            // todo. wtf dynamic ain't work here as well
            return _complex.Remove(ImportKey((Object)key));
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
            // todo. wtf dynamic ain't work here as well
            return _complex.ContainsKey(ImportKey((Object)key));
        }

        public bool ContainsValue(dynamic value)
        {
            IsObject.AssertTrue();
            // todo. wtf dynamic ain't work here as well
            return IndexOf(ImportValue((Object)value)) != -1;
        }

        public override void Add(dynamic key, dynamic value)
        {
            IsObject.AssertTrue();
            // todo. wtf dynamic ain't work here as well
            _complex.Add(ImportKey((Object)key), ImportValue((Object)value));
        }

        #endregion

        #region Array-specific API

        public void Add(dynamic value)
        {
            IsArray.AssertTrue();
            var max_index = _complex.Keys.Select(s => int.Parse(s)).MaxOrDefault(-1);
            // todo. wtf dynamic ain't work here as well
            _complex.Add(ImportKey(max_index + 1), ImportValue((Object)value));
        }

        public bool Contains(dynamic value)
        {
            IsArray.AssertTrue();
            return IndexOf(ImportValue(value)) != -1;
        }

        public int IndexOf(dynamic value)
        {
            IsArray.AssertTrue();
            // todo. wtf dynamic ain't work here as well
            return _complex.IndexOf(kvp => Equals(kvp.Value, ImportValue((Object)value)));
        }

        public void Insert(int index, dynamic value)
        {
            IsArray.AssertTrue();

            var before = _complex.Where(kvp => int.Parse(kvp.Key) < index).ToDictionary();
            // todo. wtf dynamic ain't work here as well
            var inserted = new KeyValuePair<String, Json>(ImportKey(index), ImportValue((Object)value)).MkArray().ToDictionary();
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

        public static bool operator !=(Json json, Json value) { return !(json == value); }
        public static bool operator ==(Json json, Json value) { return Equals(json, value); }

        public override bool Equals(Object obj)
        {
            var other = obj as Json;
            if (other == null) return false;

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
