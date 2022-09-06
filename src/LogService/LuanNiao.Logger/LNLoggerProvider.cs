
using System.Collections.Concurrent;

using LuanNiao.Logger.AbsInfos;
using LuanNiao.Logger.DBModel;
using LuanNiao.Logger.LogRealizations;
using LuanNiao.Logger.Models;

namespace LuanNiao.Logger
{
    /// <summary>
    /// 容器日志服务
    /// </summary>
    public class LNLoggerProvider : AbsLNLoggerProvider
    {

        /// <summary>
        /// 日志实例
        /// </summary>
        private static LNLoggerProvider? _instance;

        /// <summary>
        /// 懒汉锁
        /// </summary>
        private static readonly string _instanceLocker = Guid.NewGuid().ToString("");

        /// <summary>
        /// 当发生了消息写入时,订阅这个来自己做实现
        /// </summary>
        public event Action<(string moduleName, string? scope, LogLevelEnum level, string text, string? eventID, Exception? exception, string filePath, int num, string name)>? OnWriteMessage;

        /// <summary>
        /// 强制获取日志实例,很可能是空
        /// </summary>
        public static LNLoggerProvider? Instance { get => _instance; }

        /// <summary>
        /// 数据库日志
        /// </summary>
        private DBLog? DBLogInstance { get; set; }

        /// <summary>
        /// 日志的实例缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, AbsLNLogger> _loggerPool = new();

        /// <summary>
        /// 文件日志对象
        /// </summary>
        private readonly FileLog? _fileLogInstance;

        /// <summary>
        /// 日志的配置信息
        /// </summary>
        private readonly LogOptions _options = new();

        /// <summary>
        /// 获取日志实例
        /// </summary>
        public static LNLoggerProvider GetInstance(LogOptions options)
        {
            if (_instance == null)
            {
                lock (_instanceLocker)
                {
                    if (_instance is null)
                    {
                        _instance = new LNLoggerProvider(options);
                    }
                }

            }
            return _instance;
        }

        /// <summary>
        /// 创建一个日志器
        /// </summary>
        /// <param name="categoryName">分类/模块名称</param>
        /// <returns></returns>
        public override AbsLNLogger CreateLogger(string categoryName)
        {
            return _loggerPool.GetOrAdd(categoryName, (key) =>
            {
                return new LogCenter(categoryName, _options, _fileLogInstance);
            });

        }

        /// <summary>
        /// 根据类型创建一个日志器
        /// </summary> 
        /// <returns></returns>
        public override AbsLNLogger CreateLogger<T>()
        {
            return CreateLogger(typeof(T).FullName ?? "Error type");
        }

        /// <summary>
        /// 创建一个局部日志器
        /// </summary>
        /// <param name="categoryName">分类/模块名称</param>
        /// <param name="scopeName">局部自定义名称</param>
        /// <returns></returns>
        public override AbsLNLogger? CreateScopeLogger(string categoryName, string scopeName)
        {
            if (!_options.FileLog.EnableFileLog)
            {
                return null;
            }
            var flInfo = new FileLog(new FileLogOptions() { FolderPath = _options.FileLog.FolderPath }, scopeName);
            return new LogCenter(categoryName, _options, flInfo, scopeName);

        }

        /// <summary>
        /// 根据类型创建一个局部日志器
        /// </summary> 
        /// <param name="scopeName">局部自定义名称</param>
        /// <returns></returns>
        public override AbsLNLogger? CreateScopeLogger<T>(string scopeName)
        {
            return CreateScopeLogger(typeof(T).FullName ?? "Error type", scopeName);
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options">日志参数</param>
        private LNLoggerProvider(LogOptions options)
        {
            _options = options;
            if (options.FileLog.EnableFileLog)
            {
                _fileLogInstance = new FileLog(options.FileLog);
            }
            if (_options.DBLog.EnableDBLog)
            {
                DBLogInstance = new DBLog(_options.DBLog);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="scope">范围</param>
        /// <param name="level">级别</param>
        /// <param name="text">文本</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="exception">异常</param>
        /// <param name="filePath">文件名</param>
        /// <param name="name">函数名</param>
        /// <param name="num">行数</param>
        internal void WriteLog(string moduleName, string? scope, LogLevelEnum level, string text, string? eventID, Exception? exception, string filePath, int num, string name)
        {
            try
            {
                DBLogInstance?.TryWrite(new LogInfo()
                {
                    Created = DateTime.Now,
                    EventID = eventID ?? "",
                    Exception = exception is null ? "" : exception.Message,
                    Level = (int)level,
                    ModuleName = moduleName,
                    Text = text,
                    Scope = scope ?? "",
                    FilePath = filePath,
                    MethodName = name,
                    RowNumber = num
                });
                OnWriteMessage?.Invoke((moduleName, scope, level, text, eventID, exception, filePath, num, name));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Call OnWriteMessage error.Message:{ex.Message}");
            }
        }

#pragma warning disable CA1822 // Mark members as static
        public void Dispose()
#pragma warning restore CA1822 // Mark members as static
        {

        }
    }
}
