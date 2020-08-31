using System;
using System.Collections.Generic;
using System.Text;

namespace LuanNiao.Core
{
    public class UnionType<T1, T2, T3, T4, T5, T6,T7> : UnionType
    {
        private readonly T1 _v1;
        private readonly T2 _v2;
        private readonly T3 _v3;
        private readonly T4 _v4;
        private readonly T5 _v5;
        private readonly T6 _v6;
        private readonly T7 _v7;


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
        private UnionType(T4 data)
        {
            _v4 = data;
            _whichOne = 4;
        }
        private UnionType(T5 data)
        {
            _v5 = data;
            _whichOne = 5;
        }
        private UnionType(T6 data)
        {
            _v6 = data;
            _whichOne = 6;
        }
        private UnionType(T7 data)
        {
            _v7 = data;
            _whichOne = 7;
        }
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6,T7>(T1 t) => new UnionType<T1, T2, T3, T4, T5, T6,T7>(t);
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6,T7>(T2 t) => new UnionType<T1, T2, T3, T4, T5, T6,T7>(t);
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6,T7>(T3 t) => new UnionType<T1, T2, T3, T4, T5, T6,T7>(t);
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6,T7>(T4 t) => new UnionType<T1, T2, T3, T4, T5, T6,T7>(t);
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6,T7>(T5 t) => new UnionType<T1, T2, T3, T4, T5, T6,T7>(t);
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6,T7>(T6 t) => new UnionType<T1, T2, T3, T4, T5, T6,T7>(t);
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6,T7>(T7 t) => new UnionType<T1, T2, T3, T4, T5, T6,T7>(t);

        public UnionType<T1, T2, T3, T4, T5, T6,T7> Switch(Action<T1> action1, Action<T2> action2, Action<T3> action3, Action<T4> action4, Action<T5> action5, Action<T6> action6, Action<T7> action7)
        {
            if (action1 == null || action2 == null || action3 == null || action4 == null || action5 == null || action6 == null || action7 == null)
            {
                throw new ArgumentNullException();
            }
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
                case 4:
                    action4(_v4);
                    break;
                case 5:
                    action5(_v5);
                    break;
                case 6:
                    action6(_v6);
                    break;
                case 7:
                    action7(_v7);
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
                case 4:
                    return _v4;
                case 5:
                    return _v5;
                case 6:
                    return _v6;
                case 7:
                    return _v7;
                default:
                    throw new ArgumentException();
            }
        }


        public T1 V1() => _v1;
        public T2 V2() => _v2;
        public T3 V3() => _v3;
        public T4 V4() => _v4;
        public T5 V5() => _v5;
        public T6 V6() => _v6;
        public T7 V7() => _v7;


    }
}
