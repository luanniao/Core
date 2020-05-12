using System;
using System.Collections.Generic;
using System.Text;

namespace LuanNiao.Core.SourcePool
{
    /// <summary>
    /// default ben mq item
    /// </summary>
    public class DefaultResourceItem : IResourceItem
    {
        public int TotalTriggerTimes { get; set; }
        public long Created { get; set; }
        public long LastTriggerTime { get; set; }
        public int HideMS { get; set; }
        public bool LongLive { get; set; }
    }
}
