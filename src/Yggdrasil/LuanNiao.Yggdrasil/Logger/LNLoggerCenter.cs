using LuanNiao.Yggdrasil.Logger.AbsInfos;

namespace LuanNiao.Yggdrasil.Logger
{
    /// <summary>
    /// 泛型日志实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LNLoggerCenter<T> : AbsLNLogger<T> where T : class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        public LNLoggerCenter(string moduleName) : base(LNLoggerProvider.Instance!.CreateLogger(moduleName))
        {
        }

        /// <summary>
        /// 默认构造函数,用类型反推
        /// </summary>
        public LNLoggerCenter() : this(typeof(T).FullName ?? "Error type")
        {
        }
    }
}
