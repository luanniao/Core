using Microsoft.Extensions.Caching.Distributed;

namespace LuanNiao.Yggdrasil.Cache
{
    /// <summary>
    /// 缓存实例
    /// </summary>
    public class CacheInstance
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="instance"></param>
        public CacheInstance(IDistributedCache instance)
        {
            Instance = instance;
        }
        /// <summary>
        /// 缓存实例
        /// </summary>
        public IDistributedCache Instance { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 越大级别越高 默认0 
        /// </summary>
        public int Priority { get; set; } = 0;
        /// <summary>
        /// 可用
        /// </summary>
        public bool Available { get; set; } = false;
    }
}
