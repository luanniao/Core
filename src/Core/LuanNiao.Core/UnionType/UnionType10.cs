using System;

namespace LuanNiao.Core
{
    /// <summary>
    /// UnionType  
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    public class UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : UnionType
    {
        private readonly T1 _v1;
        private readonly T2 _v2;
        private readonly T3 _v3;
        private readonly T4 _v4;
        private readonly T5 _v5;
        private readonly T6 _v6;
        private readonly T7 _v7;
        private readonly T8 _v8;
        private readonly T9 _v9;
        private readonly T10 _v10;


        private readonly int _whichOne;

        /// <summary>
        /// get raw value
        /// </summary>
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
        private UnionType(T8 data)
        {
            _v8 = data;
            _whichOne = 8;
        }
        private UnionType(T9 data)
        {
            _v9 = data;
            _whichOne = 9;
        }
        private UnionType(T10 data)
        {
            _v10 = data;
            _whichOne = 10;
        }
        /// <summary>
        /// helper method
        /// </summary>

        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 t) => new(t);
        /// <summary>
        /// helper method
        /// </summary>

        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T2 t) => new(t);
        /// <summary>
        /// helper method
        /// </summary>

        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T3 t) => new(t);
        /// <summary>
        /// helper method
        /// </summary>

        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T4 t) => new(t);
        /// <summary>
        /// helper method
        /// </summary>

        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T5 t) => new(t);
        /// <summary>
        /// helper method
        /// </summary>

        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T6 t) => new(t);
        /// <summary>
        /// helper method
        /// </summary>

        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T7 t) => new(t);
        /// <summary>
        /// helper method
        /// </summary>

        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T8 t) => new(t);
        /// <summary>
        /// helper method
        /// </summary>

        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T9 t) => new(t);
        /// <summary>
        /// helper method
        /// </summary>

        public static implicit operator UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T10 t) => new(t);

        /// <summary>
        /// construct method
        /// </summary>
        /// <param name="action1"></param>
        /// <param name="action2"></param>
        /// <param name="action3"></param>
        /// <param name="action4"></param>
        /// <param name="action5"></param>
        /// <param name="action6"></param>
        /// <param name="action7"></param>
        /// <param name="action8"></param>
        /// <param name="action9"></param>
        /// <param name="action10"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UnionType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Switch(Action<T1> action1, Action<T2> action2, Action<T3> action3, Action<T4> action4, Action<T5> action5, Action<T6> action6, Action<T7> action7, Action<T8> action8, Action<T9> action9, Action<T10> action10)
        {
            if (action1 == null || action2 == null || action3 == null || action4 == null || action5 == null || action6 == null || action7 == null || action8 == null || action9 == null || action10 == null)
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
                case 8:
                    action8(_v8);
                    break;
                case 9:
                    action9(_v9);
                    break;
                case 10:
                    action10(_v10);
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
                8 => _v8,
                9 => _v9,
                10 => _v10,
                _ => throw new ArgumentException(),
            };
        }


        /// <summary>
        /// helper method
        /// </summary> 
        public T1 V1() => _v1;
        /// <summary>
        /// helper method
        /// </summary> 
        public T2 V2() => _v2;
        /// <summary>
        /// helper method
        /// </summary> 
        public T3 V3() => _v3;
        /// <summary>
        /// helper method
        /// </summary> 
        public T4 V4() => _v4;
        /// <summary>
        /// helper method
        /// </summary> 
        public T5 V5() => _v5;
        /// <summary>
        /// helper method
        /// </summary>

        public T6 V6() => _v6;
        /// <summary>
        /// helper method
        /// </summary>

        public T7 V7() => _v7;
        /// <summary>
        /// helper method
        /// </summary>

        public T8 V8() => _v8;
        /// <summary>
        /// helper method
        /// </summary>

        public T9 V9() => _v9;
        /// <summary>
        /// helper method
        /// </summary>

        public T10 V10() => _v10;


    }
}
