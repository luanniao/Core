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
                        WriteToConsole(handler, eventData);
                        break;
                    case GrapherOutput.File:
                        break;
                    case GrapherOutput.Sqlite:
                        WriteToDB(handler, eventData);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                TextWriter.WriteLine(MessageBuilder(handler, eventData));
            }
        }

       
    }
}
