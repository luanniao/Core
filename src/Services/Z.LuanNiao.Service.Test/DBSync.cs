using LuanNiao.Service.Grapher;
using LuanNiao.Service.Grapher.Extends.Sqlite;
using LuanNiao.Service.History;
using LuanNiao.Service.History.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Z.LuanNiao.Service.Test
{
    [Collection("DBWriter")]
    public class DBSync
    {
        private readonly ITestOutputHelper _testOutput;
        public DBSync(ITestOutputHelper testOutput)
        {
            this._testOutput = testOutput;

            GrapherSqliteExtends.Init("./test.db");
            var op = new GrapherOptions()
            {
                SourceName = Constants.LOGGER_EVENT_SOURCE_NAME,
                Arguments = null,
                Keywords = EventKeywords.All,
                Level = EventLevel.LogAlways,
                AsyncSettings = new Dictionary<EventLevel, bool>() {
                    { EventLevel.LogAlways, false }
                },
                OutPutsSettings = new Dictionary<EventLevel, GrapherOutput>() {
                     { EventLevel.LogAlways, GrapherOutput.Sqlite },
                     { EventLevel.Verbose, GrapherOutput.Sqlite },
                     { EventLevel.Informational, GrapherOutput.Sqlite },
                     { EventLevel.Warning, GrapherOutput.Sqlite },
                     { EventLevel.Error, GrapherOutput.Sqlite },
                    { EventLevel.Critical, GrapherOutput.Sqlite }
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
                var data = Guid.NewGuid().ToString();
                var checker = new DBResultChecker();
                Historiographer.Instance.Trace(Guid.NewGuid(), data);
                checker.Check(data);
                Assert.True(checker.Result);
            }
        }

        //[Fact(DisplayName = "调试级别", Timeout = 1000)]
        //public void Debug()
        //{
        //    lock (this)
        //    {
        //        var data = Guid.NewGuid().ToString();
        //        Historiographer.Instance.Debug(Guid.NewGuid(), data);
        //        Assert.True(write.Result);
        //    }
        //}

        //[Fact(DisplayName = "信息级别", Timeout = 1000)]
        //public void Info()
        //{
        //    lock (this)
        //    {
        //        var data = Guid.NewGuid().ToString();
        //        Historiographer.Instance.Info(Guid.NewGuid(), data);
        //        Assert.True(write.Result);
        //    }
        //}

        //[Fact(DisplayName = "警告级别", Timeout = 1000)]
        //public void Warning()
        //{
        //    lock (this)
        //    {
        //        var data = Guid.NewGuid().ToString();
        //        Historiographer.Instance.Warning(Guid.NewGuid(), data);
        //        Assert.True(write.Result);
        //    }
        //}

        //[Fact(DisplayName = "错误级别", Timeout = 1000)]
        //public void Error()
        //{
        //    lock (this)
        //    {
        //        var data = Guid.NewGuid().ToString();
        //        Historiographer.Instance.Error(Guid.NewGuid(), data);
        //        Assert.True(write.Result);
        //    }
        //}

        //[Fact(DisplayName = "灾难级别", Timeout = 1000)]
        //public void Critical()
        //{
        //    lock (this)
        //    {
        //        var data = Guid.NewGuid().ToString();
        //        Historiographer.Instance.Critical(Guid.NewGuid(), data);
        //        Assert.True(write.Result);
        //    }
        //}
    }
}
