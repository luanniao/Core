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

        private class DBRecordItem
        {
            public int EventID { get; set; }
            public long Tickets { get; set; }
            public string Level { get; set; }
            public string[] Keywords { get; set; }
            public string Message { get; set; }
            public string Op { get; set; }
            public string ActivityId { get; set; }
            public string RelatedActivityId { get; set; }
            public List<string> CustomPayLoad { get; set; }
        }

        private readonly ConcurrentQueue<DBRecordItem> _dbWriterQueue = new ConcurrentQueue<DBRecordItem>();
        private void WriteToDB(GrapherOptions options, EventWrittenEventArgs data)
        {
            var customPayload = new List<string>();
            foreach (var item in data.Payload)
            {
                customPayload.Add(item.ToString());
            }
            if (options.AsyncSettings.TryGetValue(data.Level, out var isAsync) && isAsync)
            {
                _dbWriterQueue.Enqueue(new DBRecordItem()
                {
                    EventID = data.EventId,
                    Tickets = data.TimeStamp.Ticks,
                    Level = Enum.GetName(typeof(EventLevel), data.Level),
                    Keywords = GetKeywords(options, data).ToArray(),
                    Message = data.Message,
                    Op = Enum.GetName(typeof(EventOpcode), data.Opcode),
                    ActivityId = data.ActivityId.ToString(),
                    RelatedActivityId = data.RelatedActivityId.ToString(),
                    CustomPayLoad = customPayload,
                });
                _dbSemaphore.Release();
            }
            else
            {
                DBWriter.Write(data.EventId
                    , data.TimeStamp.Ticks
                    , Enum.GetName(typeof(EventLevel), data.Level)
                    , GetKeywords(options, data).ToArray()
                    , data.Message
                    , Enum.GetName(typeof(EventOpcode), data.Opcode)
                    , data.ActivityId.ToString()
                    , data.RelatedActivityId.ToString()
                    , customPayload);
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
                    if (_dbWriterQueue.TryDequeue(out var msg))
                    {
                        DBWriter.Write(msg.EventID
                                      , msg.Tickets
                                      , msg.Level
                                      , msg.Keywords
                                      , msg.Message
                                      , msg.Op
                                      , msg.ActivityId
                                      , msg.RelatedActivityId
                                      , msg.CustomPayLoad);
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
