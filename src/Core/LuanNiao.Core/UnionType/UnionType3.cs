using System;

namespace LuanNiao.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public class UnionType<T1, T2, T3> : UnionType
    {
        private readonly T1 _v1;
        private readonly T2 _v2;
        private readonly T3 _v3;
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3>(T1 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3>(T2 t) => new(t);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static implicit operator UnionType<T1, T2, T3>(T3 t) => new(t);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UnionType<T1, T2, T3> Switch(Action<T1> action1, Action<T2> action2, Action<T3> action3)
        {
            if (action1 == null || action2 == null || action3 == null)
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

    }
}
