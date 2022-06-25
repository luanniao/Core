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
        /// <summary>
        /// total number of this message trigged
        /// </summary>
        public int TotalTriggerTimes { get; set; }
        /// <summary>
        /// create time stamp
        /// </summary>
        public long Created { get; set; }
        /// <summary>
        /// last trigger time stamp
        /// </summary>
        public long LastTriggerTime { get; set; }
        /// <summary>
        /// hide this message time
        /// </summary>
        public int HideMS { get; set; }
        /// <summary>
        /// is long live message?
        /// </summary>
        public bool LongLive { get; set; }
    }
}
