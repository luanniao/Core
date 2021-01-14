using BenchmarkDotNet.Running;
using System;

namespace Z.LuanNiao.JsonConverterExtends.Performance.Test
{
    public class Program
    {
        public static void Main(string[] _)
        {
            BenchmarkRunner.Run<JsonTest>();
        }
    }
}
