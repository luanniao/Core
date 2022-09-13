﻿using System;
using System.Reflection;

namespace LuanNiao.Yggdrasil.Job
{
    /// <summary>
    /// job服务item
    /// </summary>
    internal class JobInfoItem
    {
        /// <summary>
        /// Job构造函数
        /// </summary>
        public ConstructorInfo Constructor { get; set; }
        /// <summary>
        /// job类型
        /// </summary>
        public LNJobTypeEnum JobType { get; set; }


        /// <summary>
        /// 间隔,默认为空,为空时,只会触发一次,然后销毁对象,可以修改,每次运行会获取,如果是null,改成数字,则失效,只有数字改才生效
        /// </summary>
        public TimeSpan? Interval { get; set; } = null;

        /// <summary>
        /// 类型
        /// </summary>
        public LNJobTimerTypeEnum TimerType { get; set; } = LNJobTimerTypeEnum.Independent;
        /// <summary>
        /// job得类型全名称
        /// </summary>
        public string JobTypeFullName { get; set; } = "";

        /// <summary>
        /// 实例对象
        /// </summary>
        private LNJobBase? _jobInstance;

        /// <summary>
        /// Job实例对象
        /// </summary>
        public LNJobBase? JobInstance
        {
            get => _jobInstance;
            set
            {
                if (JobType == LNJobTypeEnum.Singleton && _jobInstance is not null)
                {
                    _jobInstance = value;
                }
                else
                {
                    _jobInstance = value;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="constructor">目标的构造函数</param>
        public JobInfoItem(ConstructorInfo constructor)
        {
            Constructor = constructor;
        }


    }
}
