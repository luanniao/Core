using LuanNiao.JsonConverterExtends;
using NUnit.Framework;
using System;
using System.Text.Json.Serialization;

namespace Z.LuanNiao.Extends.Test.JsonConverter
{
    [TestFixture]
    public class DateTime2StringTest
    {

        public class DateTimeTest
        {
            [DateTime2StringAtrribute("yyyy-MM-dd HH:mm:ss")]
            public DateTime D1 { get; set; } = DateTime.Now;
            [DateTime2StringAtrribute("yyyy-MM-dd HH:mm:ss")]
            public DateTime? D2 { get; set; } = DateTime.Now;
            [DateTime2StringAtrribute("yyyy-MM-dd HH:mm:ss")]
            public DateTime? D3 { get; set; }
        }


        [Test]
        public void Attribute()
        {
            var x = new DateTimeTest();
            var info = System.Text.Json.JsonSerializer.Serialize(x);
            Assert.NotNull(info);
            var data=System.Text.Json.JsonSerializer.Deserialize<DateTimeTest>(info);
            Assert.NotNull(data);
        }
    }
}