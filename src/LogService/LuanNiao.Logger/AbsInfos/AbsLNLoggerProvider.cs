namespace LuanNiao.Logger.AbsInfos
{
    /// <summary>
    /// 日志提供器的抽象基类
    /// </summary>
    public abstract class AbsLNLoggerProvider
    {

        /// <summary>
        /// 创建一个日志器
        /// </summary>
        /// <param name="categoryName">分类/模块名称</param>
        /// <returns></returns>
        public abstract AbsLNLogger CreateLogger(string categoryName);

        /// <summary>
        /// 根据类型创建一个日志器
        /// </summary> 
        /// <returns></returns>
        public abstract AbsLNLogger CreateLogger<T>();
        /// <summary>
        /// 创建一个局部日志器
        /// </summary>
        /// <param name="categoryName">分类/模块名称</param>
        /// <param name="scopeName">局部自定义名称</param>
        /// <returns></returns>
        public abstract AbsLNLogger? CreateScopeLogger(string categoryName, string scopeName);

        /// <summary>
        /// 根据类型创建一个局部日志器
        /// </summary> 
        /// <param name="scopeName">局部自定义名称</param>
        /// <returns></returns>
        public abstract AbsLNLogger? CreateScopeLogger<T>(string scopeName);
    }
}
