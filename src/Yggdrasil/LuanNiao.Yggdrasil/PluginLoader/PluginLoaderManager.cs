

using System;
using System.Collections.Generic;
using System.Reflection;

using LuanNiao.Yggdrasil.DB;
using LuanNiao.Yggdrasil.Logger.AbsInfos;

namespace LuanNiao.Yggdrasil.PluginLoader
{
    /// <summary>
    /// 管理器
    /// </summary>
    public partial class PluginLoaderManager
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly AbsLNLogger _logger;
        /// <summary>
        /// 所有的插件实例
        /// </summary>
        public List<PluginInstance> AllPluginInstance { get => this._items; }

        /// <summary>
        /// 当前依赖的抽象版本
        /// </summary>
        private readonly Version _absVersion = new("10000.0.0.0");
        /// <summary>
        /// 依赖的抽象库名称
        /// </summary>
        private readonly string _absName = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        public PluginLoaderManager(AbsLNLoggerProvider provider)
        {
            _logger = provider.CreateLogger<PluginLoaderManager>();
            if (Assembly.GetAssembly(typeof(AbsEntrance)) is Assembly asb)
            {
                if (asb.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute)) is AssemblyInformationalVersionAttribute asbVerInfo)
                {
                    _absVersion = new Version(asbVerInfo.InformationalVersion);
                    _absName = asb.GetName().Name ?? "";
                }
            }

        }

        /// <summary>
        /// DB初始化完毕
        /// </summary>
        public void DBInitiated(DataBaseManager db)
        {
            _logger.Trace("Start to invoke SetDBInstance method.");
            this._items.ForEach(item =>
            {
                try
                {
                    item.EnteranceInstance?.SetDBInstance(db);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Call {item.Name}'s SetDBInstance error.", exception: ex);
                }
            });

            _logger.Trace("Invoke SetDBInstance method completed.");
        }

        /// <summary>
        /// 系统启动成功,异步通知.
        /// </summary>
        /// <param name="contex"></param>
        public void SystemStarted(AbsYggdrasilContext contex)
        {
            _logger.Trace("Start to invoke OnSystemStarted method.");
            this._items.ForEach(item =>
            {
                try
                {
                    item.EnteranceInstance?.OnSystemStarted(contex);
                    _logger.Trace($"Invoke  {item.Name}'s OnSystemStarted method completed.");

                }
                catch (Exception ex)
                {
                    _logger.Error($"Call {item.Name}'s OnSystemStarted error.", exception: ex);
                    item.StartExceptionMessage = ex.Message;
                }

            });
        }

        /// <summary>
        /// 系统启动成功,同步通知,可以阻塞容器.
        /// </summary> 
        public void SystemStoping()
        {
            _logger.Trace("Start to invoke SystemTryShutDown method sync.");
            this._items.ForEach(item =>
            {
                try
                {
                    item.EnteranceInstance?.SystemTryShutDown();
                }
                catch (Exception ex)
                {
                    _logger.Error($"Call {item.Name}'s OnSystemStarted error.", exception: ex);
                }
            });
            _logger.Trace("Invoke SystemTryShutDown method completed.");
        }
    }
}
