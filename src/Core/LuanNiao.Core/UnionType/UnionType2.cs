using System;

namespace LuanNiao.Core
{
    /// <summary>
    /// union type
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class UnionType<T1, T2> : UnionType
    {
        private readonly T1 _v1;
        private readonly T2 _v2;
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

        /// <summary>
        /// helper method
        /// </summary>
        /// <param name="t"></param>
        public static implicit operator UnionType<T1, T2>(T2 t) => new UnionType<T1, T2>(t);
        /// <summary>
        /// helper method
        /// </summary>
        /// <param name="t"></param>
        public static implicit operator UnionType<T1, T2>(T1 t) => new UnionType<T1, T2>(t);

        /// <summary>
        /// switch two value
        /// </summary>
        /// <param name="action1"></param>
        /// <param name="action2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UnionType<T1, T2> Switch(Action<T1> action1, Action<T2> action2)
        {
            if (action1 == null || action2 == null)
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
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// get first data value
        /// </summary>
        /// <returns></returns>
        public T1 V1() => _v1;
        /// <summary>
        /// get second data value
        /// </summary>
        /// <returns></returns>
        public T2 V2() => _v2;

    }
}
