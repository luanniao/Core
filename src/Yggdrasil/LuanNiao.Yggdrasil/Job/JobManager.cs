using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LuanNiao.Yggdrasil.Logger.AbsInfos;

namespace LuanNiao.Yggdrasil.Job
{
    /// <summary>
    /// 任务管理器
    /// </summary>
    public class JobManager
    {
        /// <summary>
        /// 日志提供器
        /// </summary>
        private readonly AbsLNLoggerProvider _provider;
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly AbsLNLogger _logger;

        /// <summary>
        /// 所有的任务对象
        /// </summary>
        private readonly List<JobInfoItem> _allJobItems = new();

        /// <summary>
        /// 取消Token
        /// </summary>
        private readonly CancellationTokenSource _cancellationToken = new();

        /// <summary>
        /// 创建上下文函数
        /// </summary>
        public Func<AbsYggdrasilContext>? CreateContextAction;

        /// <summary>
        /// 默认构造
        /// </summary>
        public JobManager(AbsLNLoggerProvider provider)
        {
            _provider = provider;
            _logger = provider.CreateLogger<JobManager>();
        }




        /// <summary>
        /// 推送一个新的任务内容
        /// </summary>
        /// <param name="jobType">job服务的类型</param> 
        public void PushNewJobInfo(Type jobType)
        {
            if (jobType.BaseType == typeof(LNJobBase))
            {
                var constructor = jobType.GetConstructor(new Type[] { typeof(AbsLNLogger) });
                if (constructor is null)
                {
                    _logger.Information($"Job's default constructor not found, skip load.");
                    return;
                }

                var loggerInstance = _provider.CreateLogger(jobType.FullName ?? jobType.Name);
                try
                {
                    var instance = constructor.Invoke(new object[] { loggerInstance });
                    if (instance is null)
                    {
                        _logger.Information($"Create job instance failed, skip load.");
                        return;
                    }
                    if (instance is not LNJobBase jobBase)
                    {
                        _logger.Information($"Job is not AbsJobBase, skip load.");
                        return;
                    }
                    var item = new JobInfoItem(constructor)
                    {
                        JobTypeFullName = jobType.FullName ?? jobType.Name,
                        Interval = jobBase.Interval,
                        JobType = jobBase.JobType,
                        TimerType = jobBase.TimerType
                    };
                    if (jobBase.JobType == LNJobTypeEnum.Singleton)
                    {
                        item.JobInstance = jobBase;
                    }
                    _allJobItems.Add(item);
                    _logger.Information($"Found job Name:{jobBase.Name}" +
                        $" Type:{Enum.GetName(jobBase.JobType)}" +
                        $" TimerType:{Enum.GetName(jobBase.TimerType)}" +
                        $" Interval:{(jobBase.Interval == null ? "run one time" : jobBase.Interval.ToString())}");
                }
                catch (Exception ex)
                {
                    _logger.Information($"Create job instance failed, skip load.", exception: ex);
                    return;
                }

            }
        }

        /// <summary>
        /// 开始服务
        /// </summary>
        public void Start()
        {
            if (CreateContextAction is null)
            {
                return;
            }
            _allJobItems.ForEach(item =>
            {
                _ = Task.Run(async () =>
                {
                    while (!_cancellationToken.Token.IsCancellationRequested)
                    {
                        if (item.JobType == LNJobTypeEnum.Scoped)
                        {
                            var loggerInstance = _provider.CreateLogger(item.JobTypeFullName);
                            var instance = item.Constructor.Invoke(new object[] { loggerInstance });

                            if (instance is not LNJobBase baseInstance)
                            {
                                _logger.Information($"Create job:{item.JobTypeFullName} instance completed but it's not AbsJobBase, skip running.");
                                return;
                            }
                            item.JobInstance = baseInstance;

                            if (item.JobInstance is null)
                            {
                                _logger.Information($"Create job:{item.JobTypeFullName} instance failed, skip running.");
                                return;
                            }
                        }

                        if (item.JobInstance is null)
                        {
                            _logger.Information($"Job:{item.JobTypeFullName} instance was null, skip running.");
                            break;
                        }
                        if (item.TimerType == LNJobTimerTypeEnum.Independent)
                        {
                            _ = Task.Run(async () =>
                            {
                                using var context = CreateContextAction();
                                await item.JobInstance.Start(context);
                                if (item.JobType == LNJobTypeEnum.Scoped)
                                {
                                    item.JobInstance.Dispose();
                                }
                            });
                        }
                        else
                        {
                            using var context = CreateContextAction();
                            await item.JobInstance.Start(context);

                            if (item.JobType == LNJobTypeEnum.Scoped)
                            {
                                item.JobInstance.Dispose();
                            }
                        }
                        if (item.Interval is null)
                        {
                            break;
                        }
                        else if (item.JobType == LNJobTypeEnum.Scoped)
                        {
                            await Task.Delay(item.Interval.Value, _cancellationToken.Token);
                        }
                        else if (item.JobInstance.Interval is null)
                        {
                            break;
                        }
                        else if (item.JobType == LNJobTypeEnum.Singleton && item.JobInstance is not null)
                        {
                            await Task.Delay(item.JobInstance.Interval.Value, _cancellationToken.Token);
                        }


                    }
                }, _cancellationToken.Token);
            });
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
            _allJobItems.ForEach(item =>
            {
                _ = item.JobInstance?.Stop();
            });
        }
    }
}
