using BenchmarkDotNet.Running;
using LuanNiao.Service.Grapher;
using LuanNiao.Service.Grapher.Extends.Sqlite;
using LuanNiao.Service.History;
using LuanNiao.Service.History.Common;
using Microsoft.Data.Sqlite;
using SQLitePCL;
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

            
            //var instance = new SyncFileLogTest();
            //var dt = DateTime.Now.Ticks;
            //for (int i = 0; i < 1000_00; i++)
            //{
            //    instance.T1();
            //}
            //var res = DateTime.Now.Ticks - dt;
            //Console.WriteLine((double)res /(double) 1000_00);

            //BenchmarkRunner.Run<AsyncLogTest>();
            //BenchmarkRunner.Run<SyncLogTest>();
            BenchmarkRunner.Run<SyncFileLogTest>();
        }
    }
}
