using System;

namespace LuanNiao.Core
{
    public abstract class UnionType
    {
        public abstract Object Value { get; }


        public static bool operator ==(UnionType ut1, UnionType ut2)
        {
            if (ut1 is null || ut2 is null)
                return false;
            if (ut1.Value.GetType().IsValueType && ut2.Value.GetType().IsValueType)
            {
                return ut1.Value.GetHashCode() == ut2.Value.GetHashCode();
            }
            return ut1.Value == ut2.Value;
        }

        public static bool operator !=(UnionType ut1, UnionType ut2)
        {
            if (ut1 is not null && ut2 is null)
            {
                return true;
            }
            else if (ut1 is null && ut2 is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Is(Type t) => Value.GetType() == t;

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (obj is UnionType ut)
            {
                return ut.Value.Equals(Value);
            }
            return Value.Equals(obj);
        }
    }
}
