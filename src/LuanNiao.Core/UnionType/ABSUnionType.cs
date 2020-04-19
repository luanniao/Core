using System;

namespace LuanNiao.Core
{
    public abstract class UnionType
    {
        public abstract Object Value { get; }


        public static bool operator ==(UnionType ut1, UnionType ut2)
        {
            if (ReferenceEquals(null, ut1) || ReferenceEquals(null, ut2))
                return false;
            if (ut1.Value.GetType().IsValueType && ut2.Value.GetType().IsValueType)
            {
                return ut1.Value.GetHashCode() == ut2.Value.GetHashCode();
            }
            return ut1.Value == ut2.Value;
        }

        public static bool operator !=(UnionType ut1, UnionType ut2)
        {
            if (ReferenceEquals(null, ut1) || ReferenceEquals(null, ut2))
                return false;
            if (ut1.Value.GetType().IsValueType && ut2.Value.GetType().IsValueType)
            {
                return ut1.Value.GetHashCode() != ut2.Value.GetHashCode();
            }
            return ut1.Value != ut2.Value;
        }
        public bool Is(Type t) => Value.GetType() == t;

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (obj is UnionType ut)
            {
                return ut.Value.Equals(Value);
            }
            return Value.Equals(obj);
        }
    }
}
