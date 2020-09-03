using LuanNiao.Service.Grapher;
using LuanNiao.Service.History;
using LuanNiao.Service.History.Common;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Z.LuanNiao.Service.Test
{
    [Collection("ConsolePrint")]
    public class ConsoleSyncUseDefaultConfig
    {
        private readonly ITestOutputHelper _testOutput;
        public ConsoleSyncUseDefaultConfig(ITestOutputHelper testOutput)
        {
            this._testOutput = testOutput;
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
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.textWriter = write;
                Historiographer.Instance.Trace(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "���Լ���")]
        public void Debug()
        {
            lock (this)
            {
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.textWriter = write;
                Historiographer.Instance.Debug(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "��Ϣ����")]
        public void Info()
        {
            lock (this)
            {
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.textWriter = write;
                Historiographer.Instance.Info(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "���漶��")]
        public void Warning()
        {
            lock (this)
            {
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.textWriter = write;
                Historiographer.Instance.Warning(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "���󼶱�")]
        public void Error()
        {
            lock (this)
            {
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.textWriter = write;
                Historiographer.Instance.Error(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }

        [Fact(DisplayName = "���Ѽ���")]
        public void Critical()
        {
            lock (this)
            {                
                Assert.True(Historiographer.Instance.IsEnabled());
                var data = Guid.NewGuid().ToString();
                using var write = new CustomWriter(this._testOutput);
                write.SetData(data);
                Grapher.textWriter = write;
                Historiographer.Instance.Critical(Guid.NewGuid(), data);
                Assert.True(write.Result);
            }
        }
    }
}
