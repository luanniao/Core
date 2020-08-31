using LuanNiao.Service.Grapher;
using LuanNiao.Service.History;
using LuanNiao.Service.History.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

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
                 OutPutsSettings= new Dictionary<EventLevel, GrapherOutput>() {
                     { EventLevel.Informational, GrapherOutput.Console }
                 }
            };

            Grapher.Init(new[] { op });

            var begin = DateTime.Now.Ticks;
            for (int i = 0; i < 100000; i++)
            {
                Historiographer.Instance.Trace($"{i}");
            }
            var els = new TimeSpan(DateTime.Now.Ticks - begin); 

            Console.ReadLine();
        }
    }
}
