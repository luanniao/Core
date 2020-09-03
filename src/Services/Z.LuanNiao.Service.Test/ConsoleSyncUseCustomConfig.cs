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
    [Collection("ͬ������ �û��Զ�������")]
    public class ConsoleSyncUseCustomConfig
    {
        public ConsoleSyncUseCustomConfig()
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
        }

        [Fact(DisplayName = "׷�ټ���")]
        public void Trace()
        {
            lock (this)
            {
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter();
                write.SetData(data);
                Console.SetOut(write);
                Historiographer.Instance.Trace(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "���Լ���")]
        public void Debug()
        {
            lock (this)
            {
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter();
                write.SetData(data);
                Console.SetOut(write);
                Historiographer.Instance.Debug(Guid.NewGuid(), data); 
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "��Ϣ����")]
        public void Info()
        {
            lock (this)
            {
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter();
                write.SetData(data);
                Console.SetOut(write);
                Historiographer.Instance.Info(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "���漶��")]
        public void Warning()
        {
            lock (this)
            {
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter();
                write.SetData(data);
                Console.SetOut(write);
                Historiographer.Instance.Warning(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "���󼶱�")]
        public void Error()
        {
            lock (this)
            {
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter();
                write.SetData(data);
                Console.SetOut(write);
                Historiographer.Instance.Error(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "���Ѽ���")]
        public void Critical()
        {
            lock (this)
            {
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter();
                write.SetData(data);
                Console.SetOut(write);
                Historiographer.Instance.Critical(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }


    }
}
