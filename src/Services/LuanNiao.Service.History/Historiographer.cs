using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace LuanNiao.Service.History
{
    [EventSource(Name = Common.Constants.LOGGER_EVENT_SOURCE_NAME)]
    public sealed partial class Historiographer : EventSource
    {
        private Historiographer() { }

        public static readonly Historiographer Instance = new Historiographer();



        public const int EVENTID_TRACE = 1;
        public const int EVENTID_DEBUG = 2;
        public const int EVENTID_INFO = 3;
        public const int EVENTID_WARNING = 4;
        public const int EVENTID_ERROR = 5;
        public const int EVENTID_CRITICAL = 6;


        public const EventKeywords EKW_TRACE = (EventKeywords)1;
        public const EventKeywords EKW_DEBUG = (EventKeywords)2;
        public const EventKeywords EKW_INFO = (EventKeywords)3;
        public const EventKeywords EKW_WARNING = (EventKeywords)4;
        public const EventKeywords EKW_ERROR = (EventKeywords)5;
        public const EventKeywords EKW_CRITICAL = (EventKeywords)6;
    }
}
