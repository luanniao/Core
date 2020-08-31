using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace LuanNiao.Service.Grapher
{
    public sealed partial class Grapher : EventListener
    {
        private Grapher() {

            BeginConsoleJob();
        }
        private static Grapher _instance = null;
        private static readonly object _lock = 1;
        private readonly Dictionary<string, GrapherOptions> _handlerOptions = new Dictionary<string, GrapherOptions>();
        private bool _disposed = false;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Semaphore _consoleSemaphore = new Semaphore(0, int.MaxValue);


        public override void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _consoleSemaphore.Dispose();
            base.Dispose();
        }

        protected override void OnEventSourceCreated(EventSource source)
        {
            if (!_handlerOptions.TryGetValue(source.Name, out var handler))
            {
                return;
            }
            EnableEvents(source, handler.Level, handler.Keywords, handler.Arguments);
        }
      
    }
}
