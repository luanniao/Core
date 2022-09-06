using System;

namespace LuanNiao.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public class UnionType<T1, T2, T3, T4, T5, T6> : UnionType
    {
        private readonly T1 _v1;
        private readonly T2 _v2;
        private readonly T3 _v3;
        private readonly T4 _v4;
        private readonly T5 _v5;
        private readonly T6 _v6;


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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6>(T1 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6>(T2 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6>(T3 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6>(T4 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6>(T5 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6>(T6 t) => new(t);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UnionType<T1, T2, T3, T4, T5, T6> Switch(Action<T1> action1, Action<T2> action2, Action<T3> action3, Action<T4> action4, Action<T5> action5, Action<T6> action6)
        {
            if (action1 == null || action2 == null || action3 == null || action4 == null || action5 == null || action6 == null)
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
                default:
                    throw new ArgumentException();
            }
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


    }
}
