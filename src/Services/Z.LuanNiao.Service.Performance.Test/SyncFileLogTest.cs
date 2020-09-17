using BenchmarkDotNet.Attributes;
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

namespace Z.LuanNiao.Service.Performance.Test
{
    
    public class SyncFileLogTest
    {
        [GlobalSetup]
        public void Setup()
        {
            GrapherSqliteExtends.Init("c:/test.db");
            var op = new GrapherOptions()
            {
                SourceName = Constants.LOGGER_EVENT_SOURCE_NAME,
                Arguments = null,
                Keywords = EventKeywords.All,
                Level = EventLevel.LogAlways,
                AsyncSettings = new Dictionary<EventLevel, bool>() {
                    { EventLevel.LogAlways, true }, 
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

        [Benchmark]
        public void T1()
        {
          
            Historiographer.Instance.Trace(Guid.NewGuid(), $"Do 了 something.");
        }
    }
}
