using System;

namespace LuanNiao.Core
{
    /// <summary>
    /// union type
    /// </summary>
    public abstract class UnionType
    {
        /// <summary>
        /// data
        /// </summary>
        public abstract Object Value { get; }

        /// <summary>
        /// hash compare
        /// </summary>
        /// <param name="ut1"></param>
        /// <param name="ut2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// hash not compare
        /// </summary>
        /// <param name="ut1"></param>
        /// <param name="ut2"></param>
        /// <returns></returns>
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
        /// <summary>
        /// is some type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Is(Type t) => Value.GetType() == t;

        /// <summary>
        /// get  value to string
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Value.ToString();

        /// <summary>
        /// get value's hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// equal with other data
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
