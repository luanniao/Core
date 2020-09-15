using System;
using System.Collections.Concurrent;
using System.Diagnostics.Tracing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LuanNiao.Service.Grapher
{
    public sealed partial class Grapher
    {
        private readonly ConcurrentQueue<string> _fileWriterQueue = new ConcurrentQueue<string>();
        private void WriteToFile(GrapherOptions options, EventWrittenEventArgs data)
        {
            if (options.AsyncSettings.TryGetValue(data.Level, out var isAsync) && isAsync)
            {
                _fileWriterQueue.Enqueue(MessageBuilder(options, data));
                _fileSemaphore.Release();
            }
            else
            {
                FileWriter.Write(Encoding.UTF8.GetBytes(MessageBuilder(options, data)).AsSpan());
            }
        }


        private void BeginFileJob()
        {
            Task.Factory.StartNew(() =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    if (_consoleWriterQueue.TryDequeue(out var msg))
                    {
                        FileWriter.Write(Encoding.UTF8.GetBytes(msg).AsSpan());
                    }
                    _fileSemaphore.WaitOne();
                }
            }
            , _cancellationTokenSource.Token
            , TaskCreationOptions.LongRunning
            , TaskScheduler.Default);
        }
    }
}
