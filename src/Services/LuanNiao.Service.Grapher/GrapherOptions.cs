#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace LuanNiao.Service.Grapher
{
    public class GrapherOptions
    {
        public string? SourceName { get; set; }
        public EventLevel Level { get; set; } = EventLevel.LogAlways;
        public EventKeywords Keywords { get; set; } = EventKeywords.All;
        public IDictionary<string, string?>? Arguments { get; set; } = null;
        public Dictionary<EventLevel, GrapherOutput> OutPutsSettings { get; set; } = new Dictionary<EventLevel, GrapherOutput>();
        public Dictionary<EventLevel, bool> AsyncSettings { get; set; } = new Dictionary<EventLevel, bool>();
        public Dictionary<EventKeywords, string> EventKeywordsDescription { get; set; } = new Dictionary<EventKeywords, string>();



        private Dictionary<EventLevel, GrapherOutput>? _outputsSettingDetail = null;
        public bool GetOutPutSetting(EventLevel level, out GrapherOutput outputInfo)
        {
            if (_outputsSettingDetail == null)
            {
                lock (this)
                {
                    _outputsSettingDetail = new Dictionary<EventLevel, GrapherOutput>()
                    {
                        {EventLevel.LogAlways, GrapherOutput.Console },
                        {EventLevel.Critical, GrapherOutput.Console },
                        {EventLevel.Error, GrapherOutput.Console },
                        {EventLevel.Warning, GrapherOutput.Console },
                        {EventLevel.Informational, GrapherOutput.Console },
                        {EventLevel.Verbose, GrapherOutput.Console }
                    };
                    if (OutPutsSettings is { })
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            var eventLevel = (EventLevel)i;
                            if (OutPutsSettings.TryGetValue(eventLevel, out var grapherOutput))
                            {
                                for (int j = i; j < 6; j++)
                                {
                                    _outputsSettingDetail[(EventLevel)j] = grapherOutput;
                                }
                            }
                        }
                    }
                }
            }
            return _outputsSettingDetail.TryGetValue(level, out outputInfo);
        }
    }

    [Flags]
    public enum GrapherOutput
    {
        Console = 1,
        File = 2,
        Sqlite = 4
    }
}
