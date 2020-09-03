using LuanNiao.Service.Grapher;
using LuanNiao.Service.History;
using LuanNiao.Service.History.Common;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Z.LuanNiao.Service.Test
{
    [Collection("ͬ������ Ĭ������")]
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
