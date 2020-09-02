using BenchmarkDotNet.Running;
using LuanNiao.Service.Grapher;
using LuanNiao.Service.History;
using LuanNiao.Service.History.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace Z.LuanNiao.Service.Performance.Test
{
    public class Program
    {
        public static void Main(string[] _)
        {

            BenchmarkRunner.Run<AsyncLogTest>();
            BenchmarkRunner.Run<SyncLogTest>();
        }
    }
}
