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
            if (handler.GetOutPutSetting(eventData.Level, out var outputSetting))
            {
                if (outputSetting.HasFlag(GrapherOutput.Console))
                {
                    WriteToConsole(handler, eventData);
                }
                else if (outputSetting.HasFlag(GrapherOutput.File))
                {
                    WriteToFile(handler, eventData);
                }
                else if (outputSetting.HasFlag(GrapherOutput.Sqlite))
                {
                    WriteToDB(handler, eventData);
                }
                else
                {
                    TextWriter?.WriteLine(MessageBuilder(handler, eventData));
                }
            }
            else
            {
                TextWriter?.WriteLine(MessageBuilder(handler, eventData));
            }
        }
    }
}
