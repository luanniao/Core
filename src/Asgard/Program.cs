using System.Net;

using LuanNiao.Yggdrasil;
using LuanNiao.Yggdrasil.Cache;
using LuanNiao.Yggdrasil.DB;
using LuanNiao.Yggdrasil.Job;
using LuanNiao.Yggdrasil.Logger;
using LuanNiao.Yggdrasil.Logger.AbsInfos;
using LuanNiao.Yggdrasil.MQ;
using LuanNiao.Yggdrasil.PluginLoader;

namespace Asgard
{
    /// <summary>
    /// 程序入口
    /// </summary>
    internal partial class Program
    {

        /// <summary>
        /// 当前启动事件ID
        /// </summary>
        private static readonly string _eventID = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 数据库实例
        /// </summary>
        private static DataBaseManager? _db;
        /// <summary>
        /// 日志服务
        /// </summary>
        private static AbsLNLogger? _logger;
        /// <summary>
        /// 本地缓存实例
        /// </summary>
        private static CacheManager? _localCache;
        /// <summary>
        /// MQ服务
        /// </summary>
        private static AbsMQManager? _mq;


        /// <summary>
        /// 插件管理器
        /// </summary>
        internal static PluginLoaderManager? _pluginManager;
        /// <summary>
        /// 日志提供器
        /// </summary>
        internal static LNLoggerProvider? _loggerProvider;

        /// <summary>
        /// job管理器
        /// </summary>
        internal static JobManager? _jobManager;

        /// <summary>
        /// 上下文构造器
        /// </summary>
        internal static Func<AbsYggdrasilContext>? _contextCreator;

        internal static long _workID = 12L;
        internal static long _dataCenter = 12L;



        /// <summary>
        /// 程序主入口
        /// </summary>
        public static void /* async Task<int>*/ Main()
        {
            try
            {
                using var configFile = File.OpenText(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
                var configStr = configFile.ReadToEnd();
                var config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(configStr);
                if (config is null || !config.TryGetValue("pointName", out var pointName) || string.IsNullOrWhiteSpace(pointName))
                {
                    Console.WriteLine($"Point name was empty.");
                    return;
                }
                if (!config.TryGetValue("configCenter", out var configCenterStr)
                    || string.IsNullOrWhiteSpace(configCenterStr)
                    || IPEndPoint.TryParse(configCenterStr, out var ipEndPoint)
                    )
                {
                    Console.WriteLine($"Config center address error, got the string:{(configCenterStr ?? "")}");
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Read asgard config faild! ex:{ex.Message}");
                return;
            }

        }
    }
}