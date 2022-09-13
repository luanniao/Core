using LuanNiao.Yggdrasil.Logger.AbsInfos;


namespace LuanNiao.Yggdrasil.MQ
{
    /// <summary>
    /// MQ管理器
    /// </summary>
    public abstract class AbsMQManager
    {
        /// <summary>
        /// 日志
        /// </summary>
        public AbsLNLogger Logger { get; private set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="provider"></param>
        public AbsMQManager(AbsLNLogger logger)
        {
            Logger = logger;// provider.CreateLogger<AbsMQManager>();
        }


    }
}
