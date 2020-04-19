using System;
using System.Collections.Generic;
using System.Text;

namespace LuanNiao.Core
{
    public class UnionType<T1, T2, T3> : UnionType
    {
        private readonly T1 _v1;
        private readonly T2 _v2;
        private readonly T3 _v3;
        private readonly int _whichOne;

        public override object Value => GetData();


        private UnionType(T1 data)
        {
            _v1 = data;
            _whichOne = 1;
        }
        private UnionType(T2 data)
        {
            _v2 = data;
            _whichOne = 2;
        }
        private UnionType(T3 data)
        {
            _v3 = data;
            _whichOne = 3;
        }

        public static implicit operator UnionType<T1, T2, T3>(T1 t) => new UnionType<T1, T2, T3>(t);
        public static implicit operator UnionType<T1, T2, T3>(T2 t) => new UnionType<T1, T2, T3>(t);
        public static implicit operator UnionType<T1, T2, T3>(T3 t) => new UnionType<T1, T2, T3>(t);

        public UnionType<T1, T2, T3> Switch(Action<T1> action1, Action<T2> action2, Action<T3> action3)
        {
            switch (_whichOne)
            {
                case 1:
                    action1(_v1);
                    break;
                case 2:
                    action2(_v2);
                    break;
                case 3:
                    action3(_v3);
                    break;
                default:
                    break;
            }
            return this;
        }

        private object GetData()
        {
            switch (_whichOne)
            {
                case 1:
                    return _v1;
                case 2:
                    return _v2;
                case 3:
                    return _v3;
                default:
                    throw new ArgumentException();
            }
        }

        public static bool operator ==(UnionType<T1, T2, T3> ut1, UnionType<T1, T2, T3> ut2)
        {
            if (ut1.Value.GetType().IsValueType && ut2.Value.GetType().IsValueType)
            {
                return ut1.Value.GetHashCode() == ut2.Value.GetHashCode();
            }
            return ut1.Value == ut2.Value;
        }

        public static bool operator !=(UnionType<T1, T2, T3> ut1, UnionType<T1, T2, T3> ut2)
        {
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
