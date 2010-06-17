using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace XenoGears.Reflection.Anonymous
{
    [DebuggerDisplay(@"\{ c = {c}, e = {e} }", Type="<Anonymous Type>"), CompilerGenerated]
//    internal sealed class <>f__AnonymousType0<<c>j__TPar, <e>j__TPar>
    internal sealed class SampleAnonymousType<T0, T1>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//        private readonly <c>j__TPar <c>i__Field;
        private readonly T0 _field0;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//        private readonly <e>j__TPar <e>i__Field;
        private readonly T1 _field1;

        [DebuggerHidden]
//        public <>f__AnonymousType0(<c>j__TPar c, <e>j__TPar e)
        public SampleAnonymousType(T0 field0, T1 field1)
        {
            this._field0 = field0;
            this._field1 = field1;
        }

        [DebuggerHidden]
//        public override bool Equals(object value)
//        {
//            var type = value as <>f__AnonymousType0<<c>j__TPar, <e>j__TPar>;
//            return (((type != null) && EqualityComparer<<c>j__TPar>.Default.Equals(this.<c>i__Field, type.<c>i__Field)) && EqualityComparer<<e>j__TPar>.Default.Equals(this.<e>i__Field, type.<e>i__Field));
//        }

        public override bool Equals(object value)
        {
            var other = value as SampleAnonymousType<T0, T1>;
            return
                other != null && 
                    EqualityComparer<T0>.Default.Equals(_field0, other._field0) && 
                        EqualityComparer<T1>.Default.Equals(_field1, other._field1);
        }

        [DebuggerHidden]
//        public override int GetHashCode()
//        {
//            int num = -716976488;
//            num = (-1521134295 * num) + EqualityComparer<<c>j__TPar>.Default.GetHashCode(this.<c>i__Field);
//            return ((-1521134295 * num) + EqualityComparer<<e>j__TPar>.Default.GetHashCode(this.<e>i__Field));
//        }

        public override int GetHashCode()
        {
            int num = 0x10cee8ad;
            num = (-1521134295 * num) + EqualityComparer<T0>.Default.GetHashCode(_field0);
            return (-1521134295 * num) + EqualityComparer<T1>.Default.GetHashCode(_field1);
        }

        [DebuggerHidden]
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            builder.Append("{ c = ");
//            builder.Append(this.<c>i__Field);
//            builder.Append(", e = ");
//            builder.Append(this.<e>i__Field);
//            builder.Append(" }");
//            return builder.ToString();
//        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("{ F1 = ");
            builder.Append(_field0);
            builder.Append(", F2 = ");
            builder.Append(_field1);
            builder.Append(" }");
            return builder.ToString();
        }

//        public <c>j__TPar c
//        {
//            get
//            {
//                return this.<c>i__Field;
//            }
//        }
//
//        public <e>j__TPar e
//        {
//            get
//            {
//                return this.<e>i__Field;
//            }
//        }

        public T0 Field0
        {
            get
            {
                return _field0;
            }
        }

        public T1 Field1
        {
            get
            {
                return _field1;
            }
        }
    }
}
