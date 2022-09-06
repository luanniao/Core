using System.Threading.Channels;

using FreeSql;

using LuanNiao.Logger.DBModel;
using LuanNiao.Logger.Models;

namespace LuanNiao.Logger.LogRealizations
{
    /// <summary>
    /// 数据库日志
    /// </summary>
    internal class DBLog
    {
        /// <summary>
        /// 数据库日志池
        /// </summary>
        private readonly Channel<LogInfo> _dbLogPool = Channel.CreateUnbounded<LogInfo>();

        /// <summary>
        /// 日志数据库
        /// </summary>
        private readonly IFreeSql? _logDB;

        public DBLog(DBLogOptions opt)
        {
            Console.WriteLine($"Try create db log instance.{opt.TableName}");
            _logDB = new FreeSqlBuilder().UseConnectionString((DataType)opt.DBType, opt.DBAddress).Build();

            if (!_logDB.DbFirst.ExistsTable(opt.TableName))
            {
                _logDB.CodeFirst.SyncStructure(typeof(LogInfo), opt.TableName);
            }
            Console.WriteLine($"DBLoad init completed.{opt.TableName}");
            DBLogJob(opt);
        }


        /// <summary>
        /// 数据库日志任务
        /// </summary>
        private void DBLogJob(DBLogOptions opt)
        {
            if (_logDB is null)
            {
                return;
            }
            _ = Task.Factory.StartNew(async () =>
            {
                var reader = _dbLogPool.Reader;
                while (true)
                {
                    var asd = await reader.WaitToReadAsync();
                    var count = reader.Count;
                    var items = new List<LogInfo>();
                    while (items.Count < count)
                    {
                        items.Add(await reader.ReadAsync());
                    }
                    try
                    {
                        if (_logDB.Select<LogInfo>().AsTable((_, _) => opt.TableName).Count() > 10_000)
                        {
                            var min = _logDB.Select<LogInfo>().AsTable((_, _) => opt.TableName).Skip(1_000).First();
                            _ = _logDB.Delete<LogInfo>().AsTable((_) => opt.TableName).Where(item => item.Created <= min.Created).ExecuteAffrows();

                        }
                        _ = _logDB.Insert(items).AsTable((_) => opt.TableName).ExecuteAffrows();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Save log data to db error. ex:{ex}");
                        continue;
                    }
                }

            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// DBLogInfo
        /// </summary>
        /// <param name="logInfo"></param>
        public void TryWrite(LogInfo logInfo)
        {
            _ = _dbLogPool.Writer.TryWrite(logInfo);
        }
    }
}
