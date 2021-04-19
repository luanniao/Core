using BenchmarkDotNet.Running;
using System;

namespace Z.LuanNiao.JsonConverterExtends.Performance.Test
{
    public class Program
    {
        public static void Main()
        {
            _ = BenchmarkRunner.Run<JsonTest>();
        }
    }
}
