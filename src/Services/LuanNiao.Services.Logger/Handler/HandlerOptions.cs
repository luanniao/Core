#nullable enable
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace LuanNiao.Services.Logger.Handler
{
    public class HandlerOptions
    {
        public string? SourceName { get; set; }
        public EventLevel Level { get; set; } = EventLevel.Error;
        public EventKeywords Keywords { get; set; } = EventKeywords.All;
        public IDictionary<string, string?>? Arguments { get; set; } = null;
    }
}
