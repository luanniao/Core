using System;

using FreeSql;

using LuanNiao.Yggdrasil.Logger.AbsInfos;

namespace LuanNiao.Yggdrasil.DB
{
    /// <summary>
    /// 数据库管理器的抽象
    /// </summary>
    public class DataBaseManager
    {

        /// <summary>
        /// sql对象
        /// </summary>
        public IFreeSql Default { get; protected set; }

        private readonly AbsLNLogger _logger;

        /// <summary>
        /// 用户自定义的数据库实例
        /// </summary>
        private readonly IdleBus<IFreeSql> _customDBInstance = new(TimeSpan.FromMinutes(5));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="provider"></param> 
        public DataBaseManager(AbsLNLoggerProvider provider, string defaultDBStr, int defaultDBType, string[] defaultDBReadStr)
        {
            _logger = provider.CreateLogger<DataBaseManager>();

            Default = new FreeSqlBuilder()
          .UseConnectionString((DataType)defaultDBType, defaultDBStr)
          .UseSlave(defaultDBReadStr)
          .Build();
            Default.CodeFirst.IsAutoSyncStructure = false;
#if DEBUG
            Default.Aop.CurdAfter += (s, e) =>
            {
                _logger.Trace(e.Sql);
            };
#endif
        }


        /// <summary>
        /// 获取我的数据库
        /// </summary>
        /// <param name="key">数据库key</param> 
        /// <param name="tID">商户ID</param>
        /// <param name="token">当前token</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IFreeSql? GetMyDB(string idKey, string connectionStr, int type, string[] readDB)
        {
            var keyValue = idKey;
            try
            {
                _ = _customDBInstance.TryRegister(keyValue, () =>
                {
                    try
                    {
                        var builder = new FreeSqlBuilder().UseConnectionString((DataType)type, connectionStr);

                        builder = builder.UseSlave(readDB);
                        var instance = builder.Build();
                        instance.CodeFirst.IsAutoSyncStructure = false;
#if DEBUG
                        instance.Aop.CurdAfter += (s, e) =>
                        {
                            _logger.Trace(e.Sql);
                        };
#endif
                        return instance;
                    }
                    catch (Exception ex)
                    {
                        _logger.Critical("Try GetDBInstance failed.", exception: ex);
                        throw new Exception("DB Instance was null.");
                    }
                });
                return _customDBInstance.Get(keyValue);
            }
            catch
            {
                return null;
            }
        }



    }
}
