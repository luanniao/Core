using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using static LuanNiao.JsonConverterExtends.Constants;
using LuanNiao.JsonConverterExtends;

namespace Z.LuanNiao.Extends.Test
{
    [TestFixture]
    public class ObjectExtendsTest
    {
        public class asd:IUseLNJsonExtends
        {
            public string MyProperty { get; set; } = "zxczxc";
            public string MyProperty1 { get; set; } = "z请问恶趣味恶趣味adasdsa@#!@#";
        }
        private readonly asd _test = new asd(); 
        [Test]
        public void NormalOperationTest()
        {
            var json=_test.GetBytesWithCompress();
            var obj = json.GetObjectWithCompress<asd>();
            Assert.AreEqual(obj.MyProperty, _test.MyProperty);
            Assert.AreEqual(obj.MyProperty1, _test.MyProperty1);
        }
        [Test]
        public void HighCompressOperationTest()
        {
            var json = _test.GetBytesWithHightLevelCompress();
            var obj = json.GetObjectWithCompress<asd>();
            Assert.AreEqual(obj.MyProperty, _test.MyProperty);
            Assert.AreEqual(obj.MyProperty1, _test.MyProperty1);
        }

        [Test]
        public void OptionsTest()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(_test, CamelCase);
            var json1 = System.Text.Json.JsonSerializer.Serialize(_test, CamelCaseChinese);
            var json2 = System.Text.Json.JsonSerializer.Serialize(_test, ChineseOptions); 
            var json3 = System.Text.Json.JsonSerializer.Serialize(_test);
        }

    }
}
