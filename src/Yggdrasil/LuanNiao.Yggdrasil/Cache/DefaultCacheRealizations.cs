using System;

using CSRedis;

using LuanNiao.Yggdrasil.Logger.AbsInfos;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;

namespace LuanNiao.Yggdrasil.Cache
{
    public class DefaultCacheRealization
    {

        /// <summary>
        /// 初始化一级缓存
        /// </summary>
        public static void InitLocalMemoryCahce(CacheManager cache)
        {
            var options = Options.Create(new MemoryDistributedCacheOptions());
            var localMemoryCache = new MemoryDistributedCache(options);
            cache.PushNewClient(new CacheInstance(localMemoryCache)
            {
                Available = true,
                Name = "Local MemoryDistributedCache",
                Priority = 0
            });
        }

        /// <summary>
        /// 初始化本地的Redis服务
        /// </summary>
        /// <param name="cache">缓存服务</param>
        public static void InitLocalRedisFromConfigCenter(CacheManager cache, string redisConnString, AbsLNLogger logger)
        {
            if (string.IsNullOrEmpty(redisConnString))
            {
                logger.Warning($" Redis feature disabled.");
                return;
            }
            try
            {
                try
                {

                    var cacheInstance = new CSRedisCache(new CSRedisClient(redisConnString));
                    cacheInstance.Refresh("test");
                    cache.PushNewClient(new CacheInstance(cacheInstance)
                    {
                        Available = true,
                        Name = "Local RedisCache",
                        Priority = 1
                    });
                }
                catch (Exception ex)
                {
                    logger.Critical($"Redis server error, skip redis client.", exception: ex);
                }
            }
            catch
            {
                logger.Warning($"Can not find redis server config from config center. Redis feature disabled.");
            }
        }
    }
}
