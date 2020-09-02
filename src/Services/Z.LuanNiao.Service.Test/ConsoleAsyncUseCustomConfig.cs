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
    public class ConsoleAsyncUseCustomConfig
    {
        public ConsoleAsyncUseCustomConfig()
        {
            var op = new GrapherOptions()
            {
                SourceName = Constants.LOGGER_EVENT_SOURCE_NAME,
                Arguments = null,
                Keywords = EventKeywords.All,
                Level = EventLevel.LogAlways,
                AsyncSettings = new Dictionary<EventLevel, bool>() { { EventLevel.LogAlways, false } },
                OutPutsSettings = new Dictionary<EventLevel, GrapherOutput>() {
                     { EventLevel.LogAlways, GrapherOutput.Console }
                 },
                EventKeywordsDescription = new Dictionary<EventKeywords, string>() {
                     {Historiographer.Keywords.LUANNIAO_HISTORY,nameof(Historiographer.Keywords.LUANNIAO_HISTORY)}
                 }
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
