using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LuanNiao.Core.SourcePool
{
    /// <summary>
    /// the simple SourcePool use to handle something need to queue in your program
    /// </summary>
    public sealed class ResourceQueue : IDisposable
    {
        #region single pattern
        private ResourceQueue(string name) { this._queueName = name; }
        ~ResourceQueue()
        {
            this.Dispose(false);
        }

        private static readonly ConcurrentDictionary<string, ResourceQueue> _instance = new ConcurrentDictionary<string, ResourceQueue>();
        public static ResourceQueue Instance(string name = "Default") => _instance.GetOrAdd(name, (key) => new ResourceQueue(key));
        #endregion

        #region disposable

        private bool _disposed = false;

        private void Dispose(bool flage)
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            _ = _instance.Remove(this._queueName, out _);
            _items.Writer.Complete();
            if (flage)
            {
                this._queueName = null;
            }
            GC.Collect();
            _ = GC.WaitForFullGCComplete();
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// SourcePool's name
        /// </summary>
        private string _queueName;

        /// <summary>
        /// all items
        /// </summary>
        private readonly Channel<IResourceItem> _items = Channel.CreateBounded<IResourceItem>(int.MaxValue);
        /// <summary>
        /// push the item to target queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool PushItem<T>([NotNull] T item) where T : IResourceItem
        {
            item.LastTriggerTime = item.Created = DateTime.Now.Ticks;
            item.TotalTriggerTimes = 0;
            _ = _items.Writer.WriteAsync(item);

            return true;
        }

        /// <summary>
        /// try get the SourcePoolitem
        /// </summary>
        /// <param name="func">call back method</param>
        public async Task Fetch<T>([NotNull] Func<T, bool> func) where T : IResourceItem
        {
            if (func == null)
            {
                throw new ArgumentNullException(string.Format(Thread.CurrentThread.CurrentCulture, $"[NSourcePool] Your call back method is empty! parameter name:{nameof(func)}"));
            }
            var item = await _items.Reader.ReadAsync();
            item.TotalTriggerTimes++;
            item.LastTriggerTime = DateTime.Now.Ticks;
            var result = func((T)item);

            if (item.LongLive || !result)
            {
                item.LastTriggerTime = DateTime.Now.Ticks;
                ++item.TotalTriggerTimes;
                await _items.Writer.WriteAsync(item);
            }
        }

    }
}
