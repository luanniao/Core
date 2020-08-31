#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace LuanNiao.Service.Grapher
{
    public class GrapherOptions
    {
        public string? SourceName { get; set; }
        public EventLevel Level { get; set; } = EventLevel.Error;
        public EventKeywords Keywords { get; set; } = EventKeywords.All;
        public IDictionary<string, string?>? Arguments { get; set; } = null;
    }
}
