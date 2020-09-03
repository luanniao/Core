using System;
using System.Collections.Concurrent;
using System.Diagnostics.Tracing;
using System.Threading;
using System.Threading.Tasks;

namespace LuanNiao.Service.Grapher
{
    public sealed partial class Grapher
    {
        private readonly ConcurrentQueue<string> _consoleWriterQueue = new ConcurrentQueue<string>();
        private void ConsoleWriter(GrapherOptions options, EventWrittenEventArgs data)
        {
            if (options.AsyncSettings.TryGetValue(data.Level, out var isAsync) && isAsync)
            {
                _consoleWriterQueue.Enqueue(MessageBuilder(options, data));
                _consoleSemaphore.Release();
            }
            else
            {
                TextWriter.WriteLine(MessageBuilder(options, data));
            }
        }


        private void BeginConsoleJob()
        {
            Task.Factory.StartNew(() =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    if (_consoleWriterQueue.TryDequeue(out var msg))
                    {
                        TextWriter.WriteLine(msg);
                    }
                    _consoleSemaphore.WaitOne(10);
                }
            }
            , _cancellationTokenSource.Token
            , TaskCreationOptions.LongRunning
            , TaskScheduler.Default);
        }
    }
}
