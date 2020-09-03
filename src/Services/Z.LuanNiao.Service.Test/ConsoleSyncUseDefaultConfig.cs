using LuanNiao.Service.Grapher;
using LuanNiao.Service.History;
using LuanNiao.Service.History.Common;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Z.LuanNiao.Service.Test
{
    [Collection("同步测试 默认配置")]
    public class ConsoleSyncUseDefaultConfig
    {
        public ConsoleSyncUseDefaultConfig()
        { 
            var op = new GrapherOptions()
            {
                SourceName = Constants.LOGGER_EVENT_SOURCE_NAME,
            };
            Grapher.Init(new[] { op }); 
        }

        [Fact(DisplayName = "追踪级别")]
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

        [Fact(DisplayName = "调试级别")]
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

        [Fact(DisplayName = "信息级别")]
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

        [Fact(DisplayName = "警告级别")]
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

        [Fact(DisplayName = "错误级别")]
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

        [Fact(DisplayName = "灾难级别")]
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
