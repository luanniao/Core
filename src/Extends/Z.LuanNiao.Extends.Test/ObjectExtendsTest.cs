﻿using System;
using System.Text;
using System.Text.Json;

using LuanNiao.JsonConverterExtends;

using NUnit.Framework;

using static LuanNiao.JsonConverterExtends.CommonSerializerOptions;

namespace Z.LuanNiao.Extends.Test
{
    [TestFixture]
    public class ObjectExtendsTest
    {
        public class asd : IUseLNJsonExtends
        {
            public string MyProperty { get; set; } = "zxczxc";
            public string MyProperty1 { get; set; } = "z请问恶趣味恶趣味adasdsa@#!@#";
            [TimeSpan2String]
            public TimeSpan T1 { get; set; }
        }
        private readonly asd _test = new asd();
        [Test]
        public void NormalOperationTest()
        {
            var json = _test.GetBytes(JsonCompressLevel.Normal);
            var obj = json.GetObject<asd>(JsonCompressLevel.Normal);
            Assert.AreEqual(obj.MyProperty, _test.MyProperty);
            Assert.AreEqual(obj.MyProperty1, _test.MyProperty1);
        }
        [Test]
        public void HighCompressOperationTest()
        {
            var json = _test.GetBytes(JsonCompressLevel.High);
            var obj = json.GetObject<asd>(JsonCompressLevel.High);
            Assert.AreEqual(obj.MyProperty, _test.MyProperty);
            Assert.AreEqual(obj.MyProperty1, _test.MyProperty1);
        }

        [Test]
        public void OptionsTest()
        {
            _ = System.Text.Json.JsonSerializer.Serialize(_test, CamelCase);
            _ = System.Text.Json.JsonSerializer.Serialize(_test, CamelCaseChinese);
            _ = System.Text.Json.JsonSerializer.Serialize(_test, ChineseOptions);
            _ = System.Text.Json.JsonSerializer.Serialize(_test);
        }

        [Test]
        public void GetBytesTest()
        {
            var data = _test.GetBytes();
            var obj = data.GetObject<asd>();
            Assert.AreEqual(obj.MyProperty, _test.MyProperty);
            Assert.AreEqual(obj.MyProperty1, _test.MyProperty1);
        }

        [Test]
        public void TimeSpanTest()
        {
            asd t = new asd();
            t.T1 = new TimeSpan(1, 2, 3);
            var tt = JsonSerializer.Serialize(t);
            var t2 = JsonSerializer.Deserialize<asd>(tt);

        }
    }
}
