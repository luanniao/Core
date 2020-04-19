using System;
using System.Collections.Generic;
using System.Text;

namespace LuanNiao.Core
{
    public class UnionType<T1, T2> : UnionType
    {
        private readonly T1 _v1;
        private readonly T2 _v2;
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

        public static implicit operator UnionType<T1, T2>(T2 t) => new UnionType<T1, T2>(t);
        public static implicit operator UnionType<T1, T2>(T1 t) => new UnionType<T1, T2>(t);

        public UnionType<T1, T2> Switch(Action<T1> action1, Action<T2> action2)
        {
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
 
    }
}
