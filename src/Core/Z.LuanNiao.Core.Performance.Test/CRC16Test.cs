using BenchmarkDotNet.Attributes;
using LuanNiao.Core.NetTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.LuanNiao.Core.Performance.Test
{
    [MemoryDiagnoser]
    public class CRC16Test
    {
        public static byte[] TestData = null;
        public CRC16Test()
        {
            StringBuilder sb = new StringBuilder();
            while (sb.Length < UInt16.MaxValue)
            {
                sb.Append(Guid.NewGuid().ToString());
            }
            TestData = Encoding.UTF8.GetBytes(sb.ToString());
        }
        [Benchmark]
        public void CalcData()
        {
            CRC16IBM.GetCRC16(TestData);
        }

    }
}
