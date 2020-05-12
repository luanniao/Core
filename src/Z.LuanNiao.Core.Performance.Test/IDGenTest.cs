using BenchmarkDotNet.Attributes;
using LuanNiao.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z.LuanNiao.Core.Performance.Test
{
    [MemoryDiagnoser]
    public class IDGenTest
    {
        [Benchmark]
        public void CreateID()
        {
            IDGen.GetInstance().NextId();
        }
    }
}
