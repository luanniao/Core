using LuanNiao.Yggdrasil.DB;
using LuanNiao.Yggdrasil.Logger.AbsInfos;

using Microsoft.AspNetCore.Builder;

namespace LuanNiao.Yggdrasil.PluginLoader
{
    /// <summary>
    /// 入口抽象
    /// </summary>
    public abstract class AbsEntrance
    {
        /// <summary>
        /// 数据库管理实例
        /// </summary>
        public static DataBaseManager? DBManager { get; private set; }

        /// <summary>
        /// 日志提供器
        /// </summary>
        public static AbsLNLoggerProvider? TTPLoggerProvider { get; set; }


        /// <summary>
        /// 当获取到数据库上下文后会调用
        /// </summary>
        public abstract void OnGetDBInstance();

        /// <summary>
        /// 设置数据库实例
        /// </summary>
        /// <param name="instance"></param>
        public void SetDBInstance(DataBaseManager instance)
        {
            DBManager = instance;
            OnGetDBInstance();
        }

        /// <summary>
        /// 当系统启动完成后会触发,给予一次当前的系统上下文
        /// </summary>
        /// <param name="context"></param>
        public abstract void OnSystemStarted(AbsYggdrasilContext context);

        /// <summary>
        /// 当系统正在BuildApp时
        /// </summary>
        /// <param name="builder"></param>
        public abstract void OnBuildWebApp(IApplicationBuilder builder);



        /// <summary>
        /// 系统将要关闭
        /// </summary>
        public abstract void SystemTryShutDown();
    }
}
