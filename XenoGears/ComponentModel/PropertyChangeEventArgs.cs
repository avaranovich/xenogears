using System;
using System.Collections.Generic;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Strings;

namespace XenoGears.ComponentModel
{
    [DebuggerNonUserCode]
    public class PropertyChangeEventArgs : EventArgs
    {
        private readonly Guid _id = Guid.NewGuid();
        public Guid Id { get { return _id; } }

        private bool _correlationIdSet = false;
        private Guid _correlationId;
        public Guid CorrelationId
        {
            get
            {
                _correlationIdSet.AssertTrue();
                return _correlationId;
            }
            set
            {
                _correlationIdSet.AssertFalse();
                _correlationId = value;
                _correlationIdSet = true;
            }
        }

        public String PropertyName { get; private set; }
        private readonly bool _valuesAreProvided = true;
        public bool ValuesAreProvided { get { return _valuesAreProvided; } }
        public Object OldValue { get; private set; }
        public Object NewValue { get; private set; }
        private readonly bool _tagIsProvided = true;
        public bool TagIsProvided { get { return _tagIsProvided; } }
        public Object Tag { get; private set; }

        public PropertyChangeEventArgs(String propertyName)
        {
            PropertyName = propertyName;
            _valuesAreProvided = false;
            _tagIsProvided = false;
        }

        public PropertyChangeEventArgs(String propertyName, Object tag)
            : this(propertyName)
        {
            Tag = tag;
            _tagIsProvided = true;
        }

        public PropertyChangeEventArgs(String propertyName, Object oldValue, Object newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
            _valuesAreProvided = true;
            _tagIsProvided = false;
        }

        public PropertyChangeEventArgs(String propertyName, Object oldValue, Object newValue, Object tag)
            : this(propertyName, oldValue, newValue)
        {
            Tag = tag;
            _tagIsProvided = true;
        }

        public override String ToString()
        {
            var parts = new List<String>();
            parts.Add(PropertyName);
            if (_valuesAreProvided) parts.Add(String.Format("+[{0}], -[{1}]", NewValue.ToInvariantString() ?? "null", OldValue.ToInvariantString() ?? "null"));
            if (_tagIsProvided) parts.Add(Tag.ToInvariantString());
            return parts.StringJoin(": ");
        }
    }
}