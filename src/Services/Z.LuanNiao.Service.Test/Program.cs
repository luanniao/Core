using LuanNiao.Service.Grapher;
using LuanNiao.Service.History;
using LuanNiao.Service.History.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace Z.LuanNiao.Service.Test
{
    public class Program
    {
        public static void Main(string[] _)
        {
            var op = new GrapherOptions()
            {
                SourceName = Constants.LOGGER_EVENT_SOURCE_NAME,
                Arguments = null,
                Keywords = EventKeywords.All,
                Level = EventLevel.LogAlways,
                AsyncSettings = new Dictionary<EventLevel, bool>() { { EventLevel.Informational, true } },
                OutPutsSettings = new Dictionary<EventLevel, GrapherOutput>() {
                     { EventLevel.Informational, GrapherOutput.Console }
                 },
                 EventKeywordsDescription=new Dictionary<EventKeywords, string>() {
                     {Historiographer.Keywords.LUANNIAO_HISTORY,nameof(Historiographer.Keywords.LUANNIAO_HISTORY)}
                 }
            };
             

            Grapher.Init(new[] { op });

            var begin = DateTime.Now.Ticks;
            //for (int i = 0; i < 4; i++)
            //{
            //    Task.Run(() =>
            //    {
            //        var mastID = Guid.NewGuid();
            //        EventSource.SetCurrentThreadActivityId(mastID);
            //        for (int i = 0; i < 10; i++)
            //        {
            //            Historiographer.Instance.Trace(Guid.NewGuid(), $"Do 了 something.");
            //        }
            //    });
            //}
            for (int i = 0; i < 1_000; i++)
            {
                Historiographer.Instance.Trace(Guid.NewGuid(), $"Do 了 something.");
            }
            var els = new TimeSpan(DateTime.Now.Ticks - begin);
            Console.WriteLine(els);
            Console.ReadLine();
        }
    }
}
