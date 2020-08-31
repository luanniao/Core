using System;
using System.Diagnostics.Tracing;
using System.Text;

namespace LuanNiao.Service.Grapher
{
    public sealed partial class Grapher
    {
        private string MessageBuilder(GrapherOptions options, EventWrittenEventArgs data)
        {
            var builder = new StringBuilder($"Event ID:{data.EventId}");
            builder.Append($" Timestamp:{DateTime.Now.Ticks} UTC:{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}");
            builder.Append($" Level:{Enum.GetName(typeof(EventLevel), data.Level)} ");
            builder.Append($" Keywords:{(options.EventKeywordsDescription.TryGetValue(data.Keywords, out var des) ? des : "Unknow")}");
            if (!string.IsNullOrWhiteSpace(data.Message))
            {
                builder.Append($" Message:{data.Message}");
            }
            builder.Append($" OP:{Enum.GetName(typeof(EventOpcode), data.Opcode)}");
            builder.Append($" Custom PayLoad:");
            foreach (var item in data.Payload)
            {
                builder.Append($" {item} ");
            }
            return builder.ToString();
        }
    }
}
