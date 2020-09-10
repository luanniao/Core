using LuanNiao.Service.Grapher;
using LuanNiao.Service.History;
using LuanNiao.Service.History.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Z.LuanNiao.Service.Test
{
    [Collection("ConsolePrint")]
    public class ConsoleSyncUseCustomConfig
    {
        private readonly ITestOutputHelper _testOutput;
        public ConsoleSyncUseCustomConfig(ITestOutputHelper testOutput)
        {
            this._testOutput = testOutput;
            var op = new GrapherOptions()
            {
                SourceName = Constants.LOGGER_EVENT_SOURCE_NAME,
                Arguments = null,
                Keywords = EventKeywords.All,
                Level = EventLevel.LogAlways,
                AsyncSettings = new Dictionary<EventLevel, bool>() { { EventLevel.LogAlways, false } },
                OutPutsSettings = new Dictionary<EventLevel, GrapherOutput>() {
                     { EventLevel.LogAlways, GrapherOutput.File }
                 },
                EventKeywordsDescription = new Dictionary<EventKeywords, string>() {
                     {Historiographer.Keywords.LUANNIAO_HISTORY,nameof(Historiographer.Keywords.LUANNIAO_HISTORY)}
                 }
            };
            Grapher.Init(new[] { op });
            Grapher.ResetConfig(new[] { op });
        }


        [Fact(DisplayName = "追踪级别", Timeout = 1000)]
        public void Trace()
        {
            lock (this)
            {
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.TextWriter = write;
                Historiographer.Instance.Trace(Guid.NewGuid(), data); 
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "调试级别", Timeout = 1000)]
        public void Debug()
        {
            lock (this)
            {
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.TextWriter = write;
                Historiographer.Instance.Debug(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "信息级别", Timeout = 1000)]
        public void Info()
        {
            lock (this)
            {
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.TextWriter = write;
                Historiographer.Instance.Info(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "警告级别", Timeout = 1000)]
        public void Warning()
        {
            lock (this)
            {
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.TextWriter = write;
                Historiographer.Instance.Warning(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "错误级别", Timeout = 1000)]
        public void Error()
        {
            lock (this)
            {
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.TextWriter = write;
                Historiographer.Instance.Error(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "灾难级别", Timeout = 1000)]
        public void Critical()
        {
            lock (this)
            {
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.TextWriter = write;
                Historiographer.Instance.Critical(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }


    }
}
