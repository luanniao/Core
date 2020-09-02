using LuanNiao.Service.Grapher;
using LuanNiao.Service.History;
using LuanNiao.Service.History.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Z.LuanNiao.Service.Test
{
    public class ConsoleAsyncUseDefaultConfig
    {
        public ConsoleAsyncUseDefaultConfig()
        {
            var op = new GrapherOptions()
            {
                SourceName = Constants.LOGGER_EVENT_SOURCE_NAME,
            };
            Grapher.Init(new[] { op });
            Memory<char> _buffer = new Memory<char>(new char[1024 * 1024]);
        }

        [Fact]
        public void Trace()
        {
            var data = Guid.NewGuid().ToString();
            using var write = new CustomWriter();
            write.SetData(data);
            Console.SetOut(write);
            Historiographer.Instance.Trace(Guid.NewGuid(), data);
            Assert.True(write.Result);
        }


    }
}
