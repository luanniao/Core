using System;
using System.Collections.Generic;
using System.Text;

namespace LuanNiao.Core.SourcePool
{
    /// <summary>
    /// source item
    /// </summary>
    public interface IResourceItem
    {
        /// <summary>
        /// trigger times
        /// </summary>
        public int TotalTriggerTimes { get; set; }
        /// <summary>
        /// created ticks
        /// </summary>
        public long Created { get; set; }
        /// <summary>
        /// last triger time
        /// </summary>
        public long LastTriggerTime { get; set; }

        /// <summary>
        /// hide ms
        /// </summary>
        public int HideMS { get; set; }

        /// <summary>
        /// if this property is true, the message will in pool always
        /// </summary>
        public bool LongLive { get; set; }
    }
}
