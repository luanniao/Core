using System.Text;
using System.Text.Json;

using LuanNiao.JsonConverterExtends;
using LuanNiao.Logger;
using LuanNiao.Logger.AbsInfos;

using Microsoft.Extensions.Caching.Distributed;


namespace LuanNiao.Cache
{
    /// <summary>
    /// 缓存管理对象
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly AbsLNLogger _logger;
        /// <summary>
        /// 所有的缓存客户端
        /// </summary>
        private readonly Dictionary<int, List<CacheInstance>> _allClient = new();
        /// <summary>
        /// 所有的优先级
        /// </summary>
        private List<int> _allPriority = new();
        /// <summary>
        /// 是否锁定
        /// </summary>
        private bool _locked;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="loggerProvider">日志提供器</param>
        public Manager(LNLoggerProvider loggerProvider)
        {
            _logger = loggerProvider.CreateLogger<Manager>();
        }

        /// <summary>
        /// 推送一个新的缓存实例
        /// </summary>
        /// <param name="instance">实例</param>
        public void PushNewClient(CacheInstance instance)
        {
            if (_locked)
            {
                return;
            }

            if (instance is null || instance.Instance is null)
            {
                return;
            }
            lock (_allClient)
            {
                if (_allClient.ContainsKey(instance.Priority))
                {
                    _allClient[instance.Priority].Add(instance);
                }
                else
                {
                    _allClient[instance.Priority] = new List<CacheInstance>() { instance };
                }
                _allPriority = _allClient.Keys.OrderByDescending(item => item).ToList();
            }
        }

        /// <summary>
        /// 获取当前系统设置的所有的优先级
        /// </summary>
        /// <returns></returns>
        public int[] AllPriority()
        {
            return _allPriority.ToArray();
        }

        /// <summary>
        /// 锁定
        /// </summary>
        public void Lock()
        {
            _locked = true;
        }
        /// <summary>
        /// 解锁,可能会造成多线程问题
        /// </summary>
        public void UnLock()
        {
            _locked = false;
        }
        /// <summary>
        /// 根据key获取某一个缓存,自己用优先级判定
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>从缓存获取的值,可能为空</returns>
        public T? TryGet<T>(string key)
        {
            foreach (var priorityItem in _allPriority)
            {
                foreach (var item in _allClient[priorityItem])
                {
                    if (item.Available)
                    {
                        try
                        {
                            var res = item.Instance.Get(key);
                            if (res is not null)
                            {
                                return JsonSerializer.Deserialize<T>(res, CommonSerializerOptions.CamelCaseChineseNameCaseInsensitive)!;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.Information($"Try get cahce use key{key} error.", exception: ex);
                        }
                    }

                }
            }
            return default;
        }

        /// <summary>
        /// 根据key获取某一个缓存,自己用优先级判定
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>从缓存获取的值,可能为空</returns>
        public string? TryGetString(string key)
        {
            foreach (var priorityItem in _allPriority)
            {
                foreach (var item in _allClient[priorityItem])
                {
                    if (item.Available)
                    {
                        try
                        {
                            var rawBytes = item.Instance.Get(key);
                            if (rawBytes is null)
                            {
                                return null;
                            }
                            return Encoding.UTF8.GetString(rawBytes);
                        }
                        catch (Exception ex)
                        {
                            _logger?.Information($"Try get cahce use key{key} error.", exception: ex);
                        }
                    }
                }
            }
            return default;
        }

        /// <summary>
        /// 根据Key获取,如果获取不到则新增
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="onSet">当获取不到的时候从哪获取值</param>
        /// <param name="settings">过期时间</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns></returns>
        public T GetOrSet<T>(string key, Func<T> onSet, CacheItemSettings settings)
        {
            var res = TryGet<T>(key);
            if (res is not null)
            {
                return res;
            }
            else
            {
                var data = onSet();
                Set(key, data, settings);
                return data;
            }

        }

        /// <summary>
        /// 设置一个值到缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="data">数据</param>
        /// <param name="settings">过期时间</param>
        public void Set<T>(string key, T data, CacheItemSettings settings)
        {

            foreach (var priorityItem in _allPriority)
            {
                foreach (var item in _allClient[priorityItem])
                {
                    if (item.Available)
                    {
                        try
                        {

                            var cacheSettings = new DistributedCacheEntryOptions()
                            {
                                AbsoluteExpiration = settings.AbsoluteExpiration,
                                AbsoluteExpirationRelativeToNow = settings.AbsoluteExpirationRelativeToNow,
                                SlidingExpiration = settings.SlidingExpiration,
                            };
                            if (data is string str)
                            {
                                var bytes = Encoding.UTF8.GetBytes(str);
                                item.Instance.Set(key, bytes, cacheSettings);
                            }
                            else
                            {
                                var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data, CommonSerializerOptions.CamelCaseChineseNameCaseInsensitive));
                                item.Instance.Set(key, bytes, cacheSettings);
                            }
                            if (!settings.SaveToAll)
                            {
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.Information($"Try set cahce use key{key} error.", exception: ex);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 移除一个缓存
        /// </summary>
        /// <param name="key">key</param>
        public void Remove(string key)
        {
            foreach (var priorityItem in _allPriority)
            {
                foreach (var item in _allClient[priorityItem])
                {
                    if (item.Available)
                    {
                        try
                        {
                            item.Instance.Remove(key);
                        }
                        catch (Exception ex)
                        {
                            _logger?.Information($"Try remove cahce use key{key} error.", exception: ex);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 刷新一下某个缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="cancellationToken">取消token</param>
        public void Refresh(string key)
        {
            foreach (var priorityItem in _allPriority)
            {
                foreach (var item in _allClient[priorityItem])
                {
                    if (item.Available)
                    {
                        try
                        {
                            item.Instance.Refresh(key);
                        }
                        catch (Exception ex)
                        {
                            _logger?.Information($"Try refresh cahce use key{key} error.", exception: ex);
                        }
                    }
                }
            }
        }


    }
}
