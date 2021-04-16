using BenchmarkDotNet.Attributes;
using LuanNiao.JsonConverterExtends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.LuanNiao.JsonConverterExtends.Performance.Test
{

    public class asd : IUseLNJsonExtends
    {
        public string MyProperty { get; set; } = "zxczxc";
        public string MyProperty1 { get; set; } = "z请问恶趣味恶趣味adasdsa@#!@#";
    }
    [MemoryDiagnoser]
    public class JsonTest
    {
        private asd _target = null;
        [GlobalSetup]
        public void Setup()
        {
            _target = new asd();
        }
        [Benchmark]
        public void TestHighCompress()
        {
            _ = _target.GetBytes(JsonCompressLevel.High);
        }
        [Benchmark]
        public void TestNormalCompress()
        {
            _target.GetBytes(JsonCompressLevel.Normal);
        }

        [Benchmark]
        public void TestNoneCompress()
        {
            _target.GetBytes(JsonCompressLevel.None);
        }
    }
}
