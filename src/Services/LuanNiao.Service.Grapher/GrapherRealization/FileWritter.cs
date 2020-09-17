using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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
            }
            public string FileName
            {
                get
                {
                    var extName = "txt";
                    var fileName = "log";
                    var isNewFile = false;
                    if (this.DateFormat)
                    {
                        fileName = $"{fileName}_{DateTime.Today.ToString("yyyy-MM-dd")}";
                    }
                    if (this.MaxLenth > 0)
                    {
                        fileName = $"{fileName}.";
                        var dirct = new DirectoryInfo(this.Path);
                        var logLength = 0;
                        var lastFile = dirct.GetFiles($"*{fileName}*.{extName}").OrderByDescending(t => t.LastWriteTime).FirstOrDefault();
                        if (lastFile != null && lastFile.Exists)
                        {
                            fileName = lastFile.Name;
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
