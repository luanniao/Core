using System;
using System.Diagnostics.Tracing;

namespace LuanNiao.Service.Grapher
{
    public sealed partial class Grapher
    {
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (!_handlerOptions.TryGetValue(eventData.EventSource.Name, out var handler))
            {
                return;
            }
            if (handler.OutPutsSettings.TryGetValue(eventData.Level, out var outputSetting))
            {
                switch (outputSetting)
                {
                    case GrapherOutput.Console:
                        ConsoleWriter(handler, eventData);
                        break;
                    case GrapherOutput.File:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                textWriter.WriteLine(MessageBuilder(handler, eventData));
            }
        }

       
    }
}
