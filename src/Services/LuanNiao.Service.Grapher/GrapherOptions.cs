#nullable enable
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace LuanNiao.Service.Grapher
{
    public class GrapherOptions
    {
        public string? SourceName { get; set; }
        public EventLevel Level { get; set; } = EventLevel.Error;
        public EventKeywords Keywords { get; set; } = EventKeywords.All;
        public IDictionary<string, string?>? Arguments { get; set; } = null;
        public Dictionary<EventLevel, GrapherOutput> OutPutsSettings { get; set; } = new Dictionary<EventLevel, GrapherOutput>();
        public Dictionary<EventLevel, bool> AsyncSettings { get; set; } = new Dictionary<EventLevel, bool>();

        public Dictionary<EventKeywords, string> EventKeywordsDescription { get; set; } = new Dictionary<EventKeywords, string>();


    }

    public enum GrapherOutput
    {
        Console = 0,
        File = 1,
    }
}
