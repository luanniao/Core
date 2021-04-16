using BenchmarkDotNet.Running;
using LuanNiao.Core.NetTools;
using System;

namespace Z.LuanNiao.Core.Performance.Test
{
    internal class Program
    {
        private static void Main()
        {
            //BenchmarkRunner.Run<CRC16Test>();
            //BenchmarkRunner.Run<SourceQueueTest>();
            {
                _ = BenchmarkRunner.Run<IDGenTest>();
            }
        }
    }
}
