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
        private readonly Object _primitive;
        private readonly OrderedDictionary<String, Json> _complex;
        private bool? _isobject = null;
        public bool IsPrimitive { get { return _complex == null; } }
        public bool IsComplex { get { return _complex != null; } }
        public bool IsObject { get { return _complex != null && (_isobject ?? false); } }
        public bool IsArray { get { return _complex != null && (_isobject ?? false); } }

        #region Dynamic keys/values

        private String ImportKey(dynamic key)
        {
            if (key == null) throw AssertionHelper.Fail();
            if (key is String) return key;
            if (key is sbyte) { (_isobject == true).AssertFalse(); _isobject = false; return key.ToString(); }
            if (key is byte) { (_isobject == true).AssertFalse(); _isobject = false; return key.ToString(); }
            if (key is short) { (_isobject == true).AssertFalse(); _isobject = false; return key.ToString(); }
            if (key is ushort) { (_isobject == true).AssertFalse(); _isobject = false; return key.ToString(); }
            if (key is int) { (_isobject == true).AssertFalse(); _isobject = false; return key.ToString(); }
            if (key is uint) { (_isobject == true).AssertFalse(); _isobject = false; return key.ToString(); }
            if (key is long) { (_isobject == true).AssertFalse(); _isobject = false; return key.ToString(); }
            if (key is ulong) { (_isobject == true).AssertFalse(); _isobject = false; return key.ToString(); }
            throw AssertionHelper.Fail();
        }

        private dynamic ExportKey(String key)
        {
            if (key == null) throw AssertionHelper.Fail();
            return (_isobject ?? true) ? (dynamic)key : int.Parse(key);
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
                _json.IsComplex.AssertTrue();
                return _json.Keys.Cast<String>();
            }

            public override bool TryConvert(ConvertBinder binder, out Object result)
            {
                throw new NotImplementedException();
            }

            public override bool TryGetMember(GetMemberBinder binder, out Object result)
            {
                throw new NotImplementedException();
            }

            public override bool TrySetMember(SetMemberBinder binder, Object value)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Dictionary implementation

        public override int Count
        {
            get
            {
                IsComplex.AssertTrue();
                return _complex.Count();
            }
        }

        public override IEnumerator<KeyValuePair<dynamic, dynamic>> GetEnumerator()
        {
            IsComplex.AssertTrue();
            return _complex.ToDictionary(kvp => ExportKey(kvp.Key), kvp => ExportValue(kvp.Value)).GetEnumerator();
        }

        public override bool ContainsKey(dynamic key)
        {
            IsComplex.AssertTrue();
            return _complex.ContainsKey(ImportKey(key));
        }

        public bool ContainsValue(dynamic value)
        {
            return IndexOf(ImportValue(value)) != -1;
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

        public override bool IsReadOnly
        {
            get { return IsPrimitive; }
        }

        public override void Add(dynamic key, dynamic value)
        {
            IsComplex.AssertTrue();
            _complex.Add(ImportKey(key), ImportValue(value));
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

        protected override void SetValue(dynamic key, dynamic value)
        {
            IsComplex.AssertTrue();
            _complex[ImportKey(key)] = ImportValue(value);
        }

        #endregion

        #region Partial list implementation

        public void Add(dynamic value)
        {
            IsArray.AssertTrue();
            var max_index = _complex.Keys.Select(s => int.Parse(s)).MaxOrDefault(-1);
            Add(max_index + 1, value);
        }

        public bool Contains(dynamic value)
        {
            // note. this method does also work in dictionary mode
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

        #region Equality

        public static bool operator !=(dynamic value, Json json) { return !(value == json); }
        public static bool operator ==(dynamic value, Json json) { return json == value; }
        public static bool operator !=(Json json, dynamic value) { return !(json == value); }
        public static bool operator ==(Json json, dynamic value) { return json == new Json(value); }
        public static bool operator !=(Json json, Json value) { return !(json == value); }
        public static bool operator ==(Json json, Json value)
        {
            return Equals(json, new Json(value));
        }

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
            throw new NotImplementedException();
        }

        #endregion
    }
}
