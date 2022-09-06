using System;

namespace LuanNiao.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public class UnionType<T1, T2, T3, T4, T5, T6, T7> : UnionType
    {
        private readonly T1 _v1;
        private readonly T2 _v2;
        private readonly T3 _v3;
        private readonly T4 _v4;
        private readonly T5 _v5;
        private readonly T6 _v6;
        private readonly T7 _v7;


        private readonly int _whichOne;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7>(T1 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7>(T2 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7>(T3 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7>(T4 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7>(T5 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7>(T6 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7>(T7 t) => new(t);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UnionType<T1, T2, T3, T4, T5, T6, T7> Switch(Action<T1> action1, Action<T2> action2, Action<T3> action3, Action<T4> action4, Action<T5> action5, Action<T6> action6, Action<T7> action7)
        {
            if (action1 == null || action2 == null || action3 == null || action4 == null || action5 == null || action6 == null || action7 == null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException();
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
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
            return _whichOne switch
            {
                1 => _v1,
                2 => _v2,
                3 => _v3,
                4 => _v4,
                5 => _v5,
                6 => _v6,
                7 => _v7,
                _ => throw new ArgumentException(),
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T1 V1() => _v1;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T2 V2() => _v2;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T3 V3() => _v3;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T4 V4() => _v4;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T5 V5() => _v5;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T6 V6() => _v6;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T7 V7() => _v7;


    }
}
