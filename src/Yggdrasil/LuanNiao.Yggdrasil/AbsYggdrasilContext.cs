using System;

namespace LuanNiao.Yggdrasil
{
    /// <summary>
    /// 容器上下文
    /// </summary>
    public abstract class AbsYggdrasilContext : IDisposable
    {
        /// <summary>
        /// 是否已经销毁
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 析构函数
        /// </summary>
        ~AbsYggdrasilContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose(bool flag)
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// 唯一序号
        /// </summary>
        protected string _eventID = Guid.NewGuid().ToString("N");



        /// <summary>
        /// 事件ID,每次请求/事件都会发生变化
        /// </summary>
        public string EventID { get => _eventID; }


        /// <summary>
        /// 当前节点名称
        /// </summary>
        public abstract string PointName { get; }



        /// <summary>
        /// 获取当前的认证的Token头
        /// </summary> 
        /// <returns>头</returns>
        public abstract string AuthHeader { get; }



    }

}
