using System;

using LuanNiao.Yggdrasil.Auth;
using LuanNiao.Yggdrasil.Cache;
using LuanNiao.Yggdrasil.DB;
using LuanNiao.Yggdrasil.Logger.AbsInfos;
using LuanNiao.Yggdrasil.MQ;
using LuanNiao.Yggdrasil.PluginLoader;


namespace LuanNiao.Yggdrasil
{
    /// <summary>
    /// 容器上下文
    /// </summary>
    public sealed class AbsYggdrasilContext : IDisposable
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pointName"></param>
        /// <param name="authHeader"></param>
        /// <param name="loggerProvider"></param>
        /// <param name="db"></param>
        /// <param name="plugin"></param>
        /// <param name="mq"></param>
        /// <param name="auth"></param>
        /// <param name="eventID"></param>
        public AbsYggdrasilContext(CacheManager cache,
            string pointName,
            AbsLNLoggerProvider loggerProvider,
            DataBaseManager db,
            PluginLoaderManager plugin,
            AbsMQManager mq,
            AuthManager auth,
            string? eventID)
        {
            Cache = cache;
            PointName = pointName;
            LoggerProvider = loggerProvider;
            DB = db;
            MQ = mq;
            Auth = auth;
            Plugins = plugin;
            if (!string.IsNullOrWhiteSpace(eventID))
            {
                EventID = eventID;
            }
        }


        /// <summary>
        /// 日志提供器
        /// </summary>
        public AbsLNLoggerProvider LoggerProvider { get; init; }

        /// <summary>
        /// 事件ID,每次请求/事件都会发生变化
        /// </summary>
        public string EventID { get; private set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 当前节点名称
        /// </summary>
        public string PointName { get; init; }


        /// <summary>
        /// 当前容器插件
        /// </summary>
        public PluginLoaderManager Plugins { get; init; }

        /// <summary>
        /// 缓存实例对象
        /// </summary>
        public CacheManager Cache { get; init; }

        /// <summary>
        /// 数据库模块
        /// </summary>
        public DataBaseManager DB { get; init; }

        /// <summary>
        /// MQ模块
        /// </summary>
        public AbsMQManager MQ { get; init; }

        /// <summary>
        /// 权限模块
        /// </summary>
        public AuthManager Auth { get; init; }


        #region 析构部分

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
        public void Dispose(bool _)
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
        #endregion

    }

}
