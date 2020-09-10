using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LuanNiao.Service.Grapher
{
    public sealed partial class Grapher
    {
        private readonly ConcurrentQueue<string> _dbWriterQueue = new ConcurrentQueue<string>();
        private void WriteToDB(GrapherOptions options, EventWrittenEventArgs data)
        {
            if (options.AsyncSettings.TryGetValue(data.Level, out var isAsync) && isAsync)
            {
                _dbWriterQueue.Enqueue(MessageBuilder(options, data));
                _dbSemaphore.Release();
            }
            else
            {
                var customPayload = new List<string>();
                foreach (var item in data.Payload)
                {
                    customPayload.Add(item.ToString());
                }
                DBWriter.Write(data.EventId
                    , data.TimeStamp.Ticks
                    , Enum.GetName(typeof(EventLevel), data.Level)
                    , GetKeywords(options, data).ToArray()
                    , data.Message
                    , Enum.GetName(typeof(EventOpcode), data.Opcode)
                    , data.ActivityId.ToString()
                    , data.RelatedActivityId.ToString()
                    , customPayload.ToString());
            }
        }

        private List<string> GetKeywords(GrapherOptions options, EventWrittenEventArgs data)
        {
            var res = new List<string>();
            foreach (var item in options.EventKeywordsDescription)
            {
                if (data.Keywords.HasFlag(item.Key))
                {
                    res.Add($"{item.Value}");
                }
            }
            foreach (EventKeywords item in Enum.GetValues(typeof(EventKeywords)))
            {
                if (data.Keywords.HasFlag(item))
                {
                    res.Add($"{Enum.GetName(typeof(EventKeywords), item)}");
                }
            }
            return res;
        }

        private void BeginDBWriterJob()
        {
            Task.Factory.StartNew(() =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    if (_consoleWriterQueue.TryDequeue(out var msg))
                    {
                        TextWriter.WriteLine(msg);
                    }
                    _dbSemaphore.WaitOne();
                }
            }
            , _cancellationTokenSource.Token
            , TaskCreationOptions.LongRunning
            , TaskScheduler.Default);
        }
    }
}
