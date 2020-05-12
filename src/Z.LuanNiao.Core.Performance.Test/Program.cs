using BenchmarkDotNet.Running;
using LuanNiao.Core.NetTools;
using System;

namespace Z.LuanNiao.Core.Performance.Test
{
    internal class Program
    {
        private static void Main(string[] _)
        {
            //BenchmarkRunner.Run<CRC16Test>();
            //BenchmarkRunner.Run<SourceQueueTest>();
            BenchmarkRunner.Run<IDGenTest>();
        }
    }
}
